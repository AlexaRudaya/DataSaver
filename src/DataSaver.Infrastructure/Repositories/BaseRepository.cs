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

        public async Task<IEnumerable<T>> GetAllByAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }
            if (include is not null)
            {
                query = include(query);
            } 
            
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByFilterAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            params Expression<Func<T, bool>>[] expressions)
        {
            IQueryable<T> query = _table;

            var expressionsList = expressions.ToList();
            expressionsList.ForEach(_ => query=query.Where(_));

            if (include is not null)
            {
                query = include(query);
            } 
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetOneByAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _table;

            if (expression != null)
            {
                query = query.Where(expression);
            } 

            if (include is not null)
            {
                query = include(query);
            } 

            var model = await query.AsNoTracking().FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return model;
#pragma warning restore CS8603 // Possible null reference return.
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
