using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DataSaver.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly LinkContext _linkContext;

        private readonly DbSet<T> _dbSet;

        public BaseRepository(LinkContext linkContext)
        {
            _linkContext = linkContext;
            _dbSet = _linkContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);
            _dbSet.Remove(item);
        }

        public void UpdateAsync(T entity)
        {
            _linkContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _linkContext.SaveChangesAsync();
        }
    }
}
