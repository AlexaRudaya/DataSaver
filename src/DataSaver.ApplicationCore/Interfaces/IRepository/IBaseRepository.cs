namespace DataSaver.ApplicationCore.Interfaces.IRepository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public Task<IEnumerable<T>> GetAllByAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null);

        public Task<IEnumerable<T>> GetAllByFilterAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            params Expression<Func<T, bool>>[] expressions);

        public Task<T> GetOneByAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null);

        public Task CreateAsync(T entity);

        public Task<T> UpdateAsync(T entity);

        public Task DeleteAsync(T entity);
    }
}
