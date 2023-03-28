using Microsoft.EntityFrameworkCore.Query;

namespace DataSaver.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly LinkContext _dbContext;

        private readonly DbSet<T> _table;  

        public BaseRepository(LinkContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable <T> query = _table;

            if (include is not null)
            {
                query = include(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _table.AsNoTracking().FirstAsync(_=>_.Id.Equals(id));
        }

        public async Task CreateAsync(T entity)
        {
            await _table.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _table.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
