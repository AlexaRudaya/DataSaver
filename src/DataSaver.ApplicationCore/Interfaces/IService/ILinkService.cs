namespace DataSaver.ApplicationCore.Interfaces.IService
{
    public interface ILinkService : IBaseService<LinkViewModel>
    {
        public Task<IEnumerable<LinkViewModel>> GetAllByFilterAsync(
            int? categoryId = null,
            int? topicId = null,
            string? search = null);
    }
}
