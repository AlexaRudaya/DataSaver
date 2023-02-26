using DataSaver.ApplicationCore.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Security.Policy;

namespace DataSaver.Infrastructure.Services
{
    public sealed class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<Topic> _topicRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LinkService> _logger;

        public LinkService(ILinkRepository linkRepository,
                IBaseRepository<Category> categoryRepository,
                IBaseRepository<Topic> topicRepository,
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

        public async Task<IEnumerable<LinkViewModel>> GetAllAsync()
        {
            var linksList = await _linkRepository.GetAllAsync(
                include: query => query     //lazy loading
                    .Include(_ => _.Category!)
                    .Include(_ => _.Topic!));

            if (linksList == null)
            {
                var exception = new LinkNotFoundException("No links were found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var linksViewModelList = _mapper.Map<IEnumerable<LinkViewModel>>(linksList);

            await SetLinkPreviewAsync(linksViewModelList);

            return linksViewModelList;
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

        private async Task SetLinkPreviewAsync(IEnumerable<LinkViewModel> links)
        {
            var httpClient = new HttpClient();

            //var urls = links.Select(_ => _.UrlLink);

            foreach (var item in links)
            {
                //var response = await httpClient
                //    .GetAsync($"https://api.linkpreview.net/?key=a4df3e5a7c2713eb4456f03e2b7cf2e1&q={item.UrlLink}");

                //if (response.IsSuccessStatusCode)
                //{
                //    var responseContent = await response.Content.ReadAsStringAsync();
                //    item.LinkPreview = JsonConvert.DeserializeObject<LinkPreview>(responseContent);
                //}
                while (true) 
                {
                        var response = await httpClient
                            .GetAsync($"https://api.linkpreview.net/?key=a4df3e5a7c2713eb4456f03e2b7cf2e1&q={item.UrlLink}");

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            item.LinkPreview = JsonConvert.DeserializeObject<LinkPreview>(responseContent);
                            _logger.LogInformation("OK, loaded");
                            break;
                        }
                
                }
            }     
        }
    }
}
