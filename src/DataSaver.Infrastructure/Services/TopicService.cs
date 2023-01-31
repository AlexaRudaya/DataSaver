using AutoMapper;
using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.Exceptions;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.ApplicationCore.Interfaces.IService;
using DataSaver.ApplicationCore.ViewModels;

namespace DataSaver.Infrastructure.Services
{
    public sealed class TopicService : ITopicService
    {
        private readonly IBaseRepository<Topic> _baseRepository;
        private readonly IMapper _mapper;

        public TopicService(IBaseRepository<Topic> baseRepository,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
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
                string errorMessage = "No topics were found";
                throw new TopicNotFoundException(errorMessage);
            }

            var topicsViewModelList = _mapper.Map<IEnumerable<TopicViewModel>>(topicsList);

            return topicsViewModelList;
        }

        public async Task<TopicViewModel> GetByIdAsync(int topicId)
        {
            var entity = await _baseRepository.GetByIdAsync(topicId);

            if (entity == null)
            {
                string errorMessage = $"No topic with id: {topicId} was found";
                throw new TopicNotFoundException(errorMessage);
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
