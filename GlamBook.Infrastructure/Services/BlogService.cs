using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Infrastructure.Services
{
    public class BlogService : IBlogService
    {
        private readonly GlamBookDbContext _db;
        private readonly IMapper _mapper;

        public BlogService(GlamBookDbContext db, IMapper mapper)
        {
            _db = db; _mapper = mapper;
        }

        public async Task<IEnumerable<BlogPostDto>> GetPublishedAsync()
        {
            return await _db.BlogPosts
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.PublishedAt)
                .ProjectTo<BlogPostDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


        public async Task<int> CreateAsync(BlogPostDto dto)
        {
            var entity = _mapper.Map<BlogPost>(dto);
            if (entity.PublishedAt == default) entity.PublishedAt = DateTime.UtcNow;
            _db.BlogPosts.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(BlogPostDto dto)
        {
            var entity = await _db.BlogPosts.FindAsync(dto.Id) ?? throw new KeyNotFoundException();
            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.BlogPosts.FindAsync(id) ?? throw new KeyNotFoundException();
            _db.BlogPosts.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
