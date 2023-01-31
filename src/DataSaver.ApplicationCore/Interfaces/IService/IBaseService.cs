namespace DataSaver.ApplicationCore.Interfaces.IService
{
    public interface IBaseService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int id);

        public Task<T> CreateAsync(T entity);

        public Task<T> UpdateAsync(T entity);

        public Task<T> DeleteAsync(T entity);
    }
}
