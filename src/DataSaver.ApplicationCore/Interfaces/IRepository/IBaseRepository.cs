﻿namespace DataSaver.ApplicationCore.Interfaces.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        public Task CreateAsync(T entity);

        public Task<T> UpdateAsync(T entity);

        public Task DeleteAsync(T entity);
    }
}
