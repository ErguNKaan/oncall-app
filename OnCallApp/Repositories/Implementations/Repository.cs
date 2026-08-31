using Microsoft.EntityFrameworkCore;
using OnCallApp.Models;
using OnCallApp.Repositories.Interfaces;
using System.Linq.Expressions;

namespace OnCallApp.Repositories.Implementations
{
    // Generic repository implementation for database operations.
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbSet;

        // Initializes a new instance of the repository.
        public Repository(AppDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        // Retrieves all entities matching the optional filter and includes specified properties.
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        // Retrieves a single entity matching the filter and includes specified properties.
        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, params string[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        // Adds a new entity to the database set.
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        // Updates an existing entity in the database set.
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        // Removes an entity from the database set.
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        // Saves all changes made in this context to the database.
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
