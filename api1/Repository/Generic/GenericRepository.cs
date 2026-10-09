using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using api1.Models;

namespace api1.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MVCContext _context;

        public GenericRepository(MVCContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}