namespace DataSaver.ApplicationCore.Interfaces.IService;

public interface ILinkService : IBaseService<LinkViewModel>
{
    Task<IEnumerable<LinkViewModel>> GetAllByFilterAsync(
        int? categoryId = null,
        int? topicId = null,
        string? search = null);

    Task<IEnumerable<LinkViewModel>> GetAllBySortAsync(
        int? sortOrderId, 
        string? sortOrder);
}
