using System.Linq.Expressions;

namespace OnCallApp.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[] includeProperties);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, params string[] includeProperties);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
