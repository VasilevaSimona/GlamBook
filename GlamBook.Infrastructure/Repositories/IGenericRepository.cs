using System.Linq.Expressions;

namespace GlamBook.Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(int id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                     Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<IReadOnlyList<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? queryShaper = null);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
