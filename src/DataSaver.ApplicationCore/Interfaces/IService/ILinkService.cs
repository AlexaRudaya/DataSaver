namespace DataSaver.ApplicationCore.Interfaces.IService
{
    public interface ILinkService : IBaseService<LinkViewModel>
    {
        public Task<IEnumerable<LinkViewModel>> GetAllAsync(string searchTerm);
    }
}
