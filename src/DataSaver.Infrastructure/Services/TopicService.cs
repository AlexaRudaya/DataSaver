namespace DataSaver.Infrastructure.Services
{
    public sealed class TopicService : ITopicService
    {
        private readonly IBaseRepository<Topic> _baseRepository;
        private readonly IMapper _mapper;
        //private readonly IAppLogger<TopicService> _logger;

        public TopicService(IBaseRepository<Topic> baseRepository,
            IMapper mapper/*, IAppLogger<TopicService> logger*/)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            //_logger = logger;
        }

        public async Task<TopicViewModel> CreateAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            await _baseRepository.CreateAsync(topic);

            return topicViewModel;
        }

        public async Task<TopicViewModel> DeleteAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            await _baseRepository.DeleteAsync(topic);

            return topicViewModel;
        }

        public async Task<IEnumerable<TopicViewModel>> GetAllAsync()
        {
            var topicsList = await _baseRepository.GetAllAsync();

            if (topicsList == null)
            {
                var exception = new TopicNotFoundException("No topics were found");
                //_logger.LogError(exception, exception.Message);

                throw exception;
            }

            var topicsViewModelList = _mapper.Map<IEnumerable<TopicViewModel>>(topicsList);

            return topicsViewModelList;
        }

        public async Task<TopicViewModel> GetByIdAsync(int topicId)
        {
            var entity = await _baseRepository.GetByIdAsync(topicId);

            if (entity == null)
            {
                var exception = new TopicNotFoundException($"No topic with id: {topicId} was found");
                //_logger.LogError(exception, exception.Message);

                throw exception;
            }

            var topicViewModel = _mapper.Map<TopicViewModel>(entity);

            return topicViewModel;
        }

        public async Task<TopicViewModel> UpdateAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);
            await _baseRepository.UpdateAsync(topic);

            return topicViewModel;
        }
    }
}
