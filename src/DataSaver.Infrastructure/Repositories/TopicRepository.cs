namespace DataSaver.Infrastructure.Repositories
{
    public sealed class TopicRepository : BaseRepository<Topic>, ITopicRepository
    {
        private readonly LinkContext _linkContext;

        public TopicRepository(LinkContext linkContext) : base(linkContext) 
        {
            _linkContext = linkContext;
        }
    }
}
