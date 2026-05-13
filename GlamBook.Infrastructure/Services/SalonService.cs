using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Infrastructure.Services
{
    public class SalonService : ISalonService
    {
        private readonly GlamBookDbContext _db;
        private readonly IMapper _mapper;

        public SalonService(GlamBookDbContext db, IMapper mapper)
        {
            _db = db; _mapper = mapper;
        }

        public async Task<IEnumerable<SalonDto>> GetAllAsync(int? categoryId = null)
        {
            var q = _db.Salons.AsQueryable();

            if (categoryId.HasValue)
            {
                q = q.Where(s => s.Services.Any(x => x.CategoryId == categoryId.Value));
            }

            return await q
                .ProjectTo<SalonDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<SalonDto?> GetAsync(int id)
        {
            var entity = await _db.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null) return null;

            var dto = _mapper.Map<SalonDto>(entity);
            dto.Services = entity.Services.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                DurationInMinutes = (int)s.Duration.TotalMinutes,
                CategoryId = s.CategoryId,
                SalonId = s.SalonId
            }).ToList();

            return dto;
        }

        public async Task<int> CreateAsync(SalonDto model)
        {
            var entity = _mapper.Map<Salon>(model);
            _db.Salons.Add(entity);
            await _db.SaveChangesAsync();

            foreach (var svc in model.Services.Where(s => !string.IsNullOrWhiteSpace(s.Name)))
            {
                _db.Services.Add(new Service
                {
                    SalonId = entity.Id,
                    Name = svc.Name,
                    Price = svc.Price,
                    Duration = TimeSpan.FromMinutes(svc.DurationInMinutes > 0 ? svc.DurationInMinutes : 30),
                    CategoryId = svc.CategoryId > 0 ? svc.CategoryId : model.CategoryId
                });
            }

            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(SalonDto dto)
        {
            var salon = await _db.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.Id == dto.Id)
                ?? throw new KeyNotFoundException();

            salon.Name = dto.Name;
            salon.Address = dto.Address;
            salon.Description = dto.Description;
            salon.ManagerUserId = dto.ManagerUserId;

            if (!string.IsNullOrEmpty(dto.ImageUrl))
                salon.ImageUrl = dto.ImageUrl;

            // Get incoming service IDs (existing ones have Id > 0)
            var incomingIds = dto.Services
                .Where(s => s.Id > 0)
                .Select(s => s.Id)
                .ToList();

            // Only delete services that have NO appointments
            var toDelete = salon.Services
                .Where(s => !incomingIds.Contains(s.Id))
                .ToList();

            foreach (var s in toDelete)
            {
                bool hasAppointments = await _db.Appointments.AnyAsync(a => a.ServiceId == s.Id);
                if (!hasAppointments)
                    _db.Services.Remove(s);
            }

            // Update existing services
            foreach (var incoming in dto.Services.Where(s => s.Id > 0))
            {
                var existing = salon.Services.FirstOrDefault(s => s.Id == incoming.Id);
                if (existing != null)
                {
                    existing.Name = incoming.Name;
                    existing.Price = incoming.Price;
                    existing.Duration = TimeSpan.FromMinutes(incoming.DurationInMinutes);
                    existing.CategoryId = incoming.CategoryId;
                }
            }

            // Add new services (Id == 0)
            foreach (var incoming in dto.Services.Where(s => s.Id == 0))
            {
                salon.Services.Add(new Domain.Entities.Service
                {
                    Name = incoming.Name,
                    Price = incoming.Price,
                    Duration = TimeSpan.FromMinutes(incoming.DurationInMinutes),
                    CategoryId = incoming.CategoryId,
                    SalonId = salon.Id
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Salons.FindAsync(id) ?? throw new KeyNotFoundException();
            _db.Salons.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
