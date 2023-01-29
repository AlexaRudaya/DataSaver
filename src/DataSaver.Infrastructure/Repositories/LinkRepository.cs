using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.Infrastructure.Data;

namespace DataSaver.Infrastructure.Repositories
{
    public sealed class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        private readonly LinkContext _linkContext;

        public LinkRepository(LinkContext linkContext) : base(linkContext) 
        {
            _linkContext = linkContext;
        }
    }
}
