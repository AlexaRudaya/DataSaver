namespace DataSaver.ApplicationCore.Interfaces.IService
{
    public interface ILinkService : IBaseService<LinkViewModel>
    {
        Task<IEnumerable<LinkViewModel>> GetAllByFilterAsync(
            int? categoryId = null,
            int? topicId = null,
            string? search = null);

        Task<IEnumerable<LinkViewModel>> SortedLinksAsync(
            int? categoryId = null, 
            int? topicId = null,
            string? sortOrder = null);
    }
}
