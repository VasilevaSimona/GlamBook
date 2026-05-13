using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GlamBook.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly GlamBookDbContext _db;
        public GenericRepository(GlamBookDbContext db) => _db = db;

        public async Task<T?> GetAsync(int id) => await _db.Set<T>().FindAsync(id);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> q = _db.Set<T>().Where(predicate);
            if (include != null) q = include(q);
            return await q.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? queryShaper = null)
        {
            IQueryable<T> q = _db.Set<T>();
            if (queryShaper != null) q = queryShaper(q);
            return await q.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
