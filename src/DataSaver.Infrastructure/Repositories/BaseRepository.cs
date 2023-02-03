namespace DataSaver.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly LinkContext _dbContext;

        private readonly DbSet<T> _table;  // separate table

        public BaseRepository(LinkContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);
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
