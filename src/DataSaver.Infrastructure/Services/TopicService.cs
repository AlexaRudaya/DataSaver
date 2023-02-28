namespace DataSaver.Infrastructure.Services
{
    public sealed class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TopicService> _logger;

        public TopicService(ITopicRepository topicRepository,
            IMapper mapper, ILogger<TopicService> logger)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TopicViewModel> CreateAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            topic.DateCreated = DateTime.Now;
            
            await _topicRepository.CreateAsync(topic);

            return topicViewModel;
        }

        public async Task<TopicViewModel> DeleteAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            await _topicRepository.DeleteAsync(topic);

            return topicViewModel;
        }

        public async Task<IEnumerable<TopicViewModel>> GetAllAsync()
        {
            var topicsList = await _topicRepository.GetAllAsync();

            if (topicsList == null)
            {
                var exception = new TopicNotFoundException("No topics were found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var topicsViewModelList = _mapper.Map<IEnumerable<TopicViewModel>>(topicsList);

            return topicsViewModelList;
        }

        public Task<IEnumerable<TopicViewModel>> GetAllWithFiltersAsync(int? categoryId, int? topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<TopicViewModel> GetByIdAsync(int topicId)
        {
            var entity = await _topicRepository.GetByIdAsync(topicId);

            if (entity == null)
            {
                var exception = new TopicNotFoundException($"No topic with id: {topicId} was found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var topicViewModel = _mapper.Map<TopicViewModel>(entity);

            return topicViewModel;
        }

        public async Task<TopicViewModel> UpdateAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            var modelFromDb = await _topicRepository.GetByIdAsync(topic.Id);
            var modelFromDbCreated = modelFromDb!.DateCreated;
            topic.DateCreated = modelFromDbCreated;

            await _topicRepository.UpdateAsync(topic);

            return topicViewModel;
        }
    }
}
