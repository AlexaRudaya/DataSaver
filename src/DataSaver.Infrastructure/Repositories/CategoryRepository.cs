namespace DataSaver.Infrastructure.Repositories
{
    public sealed class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly LinkContext _linkContext;

        public CategoryRepository(LinkContext linkContext) : base(linkContext)
        {
            _linkContext = linkContext;
        }
    }
}
