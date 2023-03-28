using Microsoft.EntityFrameworkCore;

namespace DataSaver.Infrastructure.Services
{
    public sealed class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LinkService> _logger;

        public LinkService(ILinkRepository linkRepository,
                ICategoryRepository categoryRepository,
                ITopicRepository topicRepository,
                IMapper mapper, ILogger<LinkService> logger)
        {
            _linkRepository = linkRepository;
            _categoryRepository = categoryRepository;
            _topicRepository = topicRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LinkViewModel> CreateAsync(LinkViewModel linkViewModel)
        {
            await SetLinkPreviewAsync(linkViewModel);

            var link = _mapper.Map<Link>(linkViewModel);

            link.DateCreated = DateTime.Now;

            await _linkRepository.CreateAsync(link);

            return linkViewModel;
        }

        public async Task<LinkViewModel> DeleteAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);
            await _linkRepository.DeleteAsync(link);

            return linkViewModel;
        }

        public async Task<IEnumerable<LinkViewModel>> GetAllAsync(string? searchTerm)
        {
            var linksList = await _linkRepository.GetAllAsync(
                include: query => query     
                    .Include(_ => _.Category!)
                    .Include(_ => _.Topic!));

            if (linksList == null)
            {
                var exception = new LinkNotFoundException("No links were found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var linksViewModelList = _mapper.Map<IEnumerable<LinkViewModel>>(linksList);

            return linksViewModelList;
        }

        public Task<IEnumerable<LinkViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<LinkViewModel> GetByIdAsync(int linkId)
        {
            var entity = await _linkRepository.GetByIdAsync(linkId);    

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

            var modelFromDb = await _linkRepository.GetByIdAsync(link.Id);
            var modelFromDbCreated = modelFromDb!.DateCreated;
            link.DateCreated = modelFromDbCreated;

            await _linkRepository.UpdateAsync(link);

            return linkViewModel;
        }

        private async Task SetLinkPreviewAsync(LinkViewModel link)
        {
            var httpClient = new HttpClient();

            int i = default;

            while (true)
            {
                var response = await httpClient
                    .GetAsync($"https://api.linkpreview.net/?key=a4df3e5a7c2713eb4456f03e2b7cf2e1&q={link.UrlLink}");
                
                i++;

                if (i == 10)
                {
                    throw new InvalidOperationException("The amount of attempts is 10, please, reload a page");
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LinkPreview>(responseContent);

                    link.PreviewTitle = result.Title!;
                    link.PreviewImage = result.Image!;

                    _logger.LogInformation($"{i} times loaded");
                    break;
                }

                _logger.LogError(response.StatusCode.ToString());
            }     
        }

        //public async Task<List<LinkViewModel>> Search(string searchTerm)
        //{
        //    var allLinks = await _linkRepository.GetAllAsync();

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        allLinks = allLinks.Where(_ =>
        //            _.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            _.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            _.PreviewTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    return (List<LinkViewModel>)allLinks;
        //}
    }
}
