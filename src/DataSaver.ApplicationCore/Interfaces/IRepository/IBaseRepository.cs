using Microsoft.EntityFrameworkCore.Query;

namespace DataSaver.ApplicationCore.Interfaces.IRepository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<T?> GetByIdAsync(int id);

        public Task CreateAsync(T entity);

        public Task<T> UpdateAsync(T entity);

        public Task DeleteAsync(T entity);
    }
}
