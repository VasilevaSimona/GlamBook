using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly GlamBookDbContext _db;
        private readonly IMapper _mapper;

        public ServiceService(GlamBookDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            return await _db.Services
                .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ServiceDto?> GetAsync(int id)
        {
            var entity = await _db.Services.FindAsync(id);
            return entity == null ? null : _mapper.Map<ServiceDto>(entity);
        }

        public async Task<IEnumerable<ServiceDto>> GetBySalonAsync(int salonId)
        {
            return await _db.Services
                .Where(s => s.SalonId == salonId)
                .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task CreateAsync(ServiceDto dto)
        {
            var entity = _mapper.Map<Service>(dto);
            entity.Duration = TimeSpan.FromMinutes(dto.DurationInMinutes);

            _db.Services.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServiceDto dto)
        {
            var entity = await _db.Services.FindAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException();

            _mapper.Map(dto, entity);
            entity.Duration = TimeSpan.FromMinutes(dto.DurationInMinutes);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Services.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            _db.Services.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
