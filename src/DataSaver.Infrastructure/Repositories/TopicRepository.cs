using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.Infrastructure.Data;

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
