using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly GlamBookDbContext _db;
        private readonly IMapper _mapper;

        public CategoryService(GlamBookDbContext db, IMapper mapper)
        {
            _db = db; _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(CategoryDto dto)
        {
            // unique name enforced by DB, but we pre-check for nicer error
            var exists = await _db.Categories.AnyAsync(c => c.Name == dto.Name);
            if (exists) throw new InvalidOperationException("Category name already exists.");

            var entity = _mapper.Map<Category>(dto);
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(CategoryDto dto)
        {
            var entity = await _db.Categories.FindAsync(dto.Id) ?? throw new KeyNotFoundException();

            // if name changed, validate uniqueness
            if (!string.Equals(entity.Name, dto.Name, StringComparison.Ordinal))
            {
                var exists = await _db.Categories.AnyAsync(c => c.Name == dto.Name);
                if (exists) throw new InvalidOperationException("Category name already exists.");
            }

            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Categories.FindAsync(id) ?? throw new KeyNotFoundException();
            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
