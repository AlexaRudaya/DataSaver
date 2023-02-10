using System;

namespace DataSaver.Infrastructure.Services
{
    public sealed class LinkService : ILinkService
    {
        private readonly IBaseRepository<Link> _baseRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<Topic> _topicRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LinkService> _logger;

        public LinkService(IBaseRepository<Link> baseRepository,
                IBaseRepository<Category> categoryRepository,
                IBaseRepository<Topic> topicRepository,
                IMapper mapper, ILogger<LinkService> logger)
        {
            _baseRepository = baseRepository;
            _categoryRepository = categoryRepository;
            _topicRepository = topicRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LinkViewModel> CreateAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);

            link.DateCreated = DateTime.Now; 

            await _baseRepository.CreateAsync(link);

            return linkViewModel;
        }

        public async Task<LinkViewModel> DeleteAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);
            await _baseRepository.DeleteAsync(link);

            return linkViewModel;
        }

        public async Task<IEnumerable<LinkViewModel>> GetAllAsync()
        {
            var linksList = await _baseRepository.GetAllAsync(
                include:query => query     //lazy loading
                    .Include(_=>_.Category!)
                    .Include(_=>_.Topic!));

            if (linksList == null)
            {
                var exception = new LinkNotFoundException("No links were found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var linksViewModelList = _mapper.Map<IEnumerable<LinkViewModel>>(linksList);

            return linksViewModelList;
        }

        public async Task<LinkViewModel> GetByIdAsync(int linkId)
        {
            var entity = await _baseRepository.GetByIdAsync(linkId);    

            if (entity == null) 
            {
                var exception = new LinkNotFoundException($"No link with id: {linkId} was found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var linkViewModel = _mapper.Map<LinkViewModel>(entity);

            var category = await _categoryRepository.GetByIdAsync(linkViewModel.CategoryId);
            var topic = await _topicRepository.GetByIdAsync(linkViewModel.TopicId);

            linkViewModel.Category = category;
            linkViewModel.Topic = topic;

            return linkViewModel;   
        }

        public async Task<LinkViewModel> UpdateAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);

            var modelFromDb = await _baseRepository.GetByIdAsync(link.Id);
            var modelFromDbCreated = modelFromDb!.DateCreated;
            link.DateCreated = modelFromDbCreated;

            await _baseRepository.UpdateAsync(link);

            return linkViewModel;
        }
    }
}
