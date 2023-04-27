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

        /// <summary>
        /// Creates a new topic.
        /// </summary>
        /// <param name="topicViewModel">The view model for the ctreated topic.</param>
        /// <returns></returns>
        public async Task<TopicViewModel> CreateAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            topic.DateCreated = DateTime.Now;
            
            await _topicRepository.CreateAsync(topic);

            return topicViewModel;
        }

        /// <summary>
        /// Removes an existing topic.
        /// </summary>
        /// <param name="topicViewModel">The view model representing the topic to delete</param>
        /// <returns>The view model representing the deleted topic.</returns>
        public async Task<TopicViewModel> DeleteAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            await _topicRepository.DeleteAsync(topic);

            return topicViewModel;
        }

        /// <summary>
        /// Gets the list of all topics.
        /// </summary>
        /// <returns>The view model representing the list of all topics.</returns>
        /// <exception cref="TopicNotFoundException">Thrown when no topics were found.</exception>
        public async Task<IEnumerable<TopicViewModel>> GetAllAsync()
        {
            var topicsList = await _topicRepository.GetAllByAsync();

            if (topicsList == null)
            {
                var exception = new TopicNotFoundException("No topics were found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var topicsViewModelList = _mapper.Map<IEnumerable<TopicViewModel>>(topicsList);

            return topicsViewModelList;
        }

        /// <summary>
        /// Retrieves a topic by it's ID.
        /// </summary>
        /// <param name="topicId">ID of the topic wanted to get.</param>
        /// <returns>The view model of a link with the given ID.</returns>
        /// <exception cref="TopicNotFoundException">Thrown when there there is no topic with such ID.</exception>
        public async Task<TopicViewModel> GetByIdAsync(int topicId)
        {
            var entity = await _topicRepository.GetOneByAsync(expression: _=>_.Id.Equals(topicId));

            if (entity == null)
            {
                var exception = new TopicNotFoundException($"No topic with id: {topicId} was found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var topicViewModel = _mapper.Map<TopicViewModel>(entity);

            return topicViewModel;
        }

        /// <summary>
        /// Updates an existing topic with the information provided in the topicViewModel.
        /// </summary>
        /// <param name="topicViewModel">Contains the updated information for the topic.</param>
        /// <returns>The updated view model of a topic.</returns>
        public async Task<TopicViewModel> UpdateAsync(TopicViewModel topicViewModel)
        {
            var topic = _mapper.Map<Topic>(topicViewModel);

            var modelFromDb = await _topicRepository.GetOneByAsync(expression: _ => _.Id.Equals(topicViewModel.Id));
            var modelFromDbCreated = modelFromDb!.DateCreated;
            topic.DateCreated = modelFromDbCreated;

            await _topicRepository.UpdateAsync(topic);

            return topicViewModel;
        }
    }
}
