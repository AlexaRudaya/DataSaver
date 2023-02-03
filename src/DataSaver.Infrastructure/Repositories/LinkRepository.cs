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
