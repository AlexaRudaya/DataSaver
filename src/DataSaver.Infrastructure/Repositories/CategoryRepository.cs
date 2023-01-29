using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.Infrastructure.Data;

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
