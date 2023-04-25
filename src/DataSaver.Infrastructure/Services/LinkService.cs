﻿using DataSaver.ApplicationCore.Entities;
using DataSaver.Infrastructure.Data.Migrations;

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
                IMapper mapper,
                ILogger<LinkService> logger)
        {
            _linkRepository = linkRepository;
            _categoryRepository = categoryRepository;
            _topicRepository = topicRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new link.
        /// </summary>
        /// <param name="linkViewModel">The view model for the link to be created.</param>
        /// <returns>The view model for the ctreated link.</returns>
        /// <remarks>
        /// This method sets the link preview using the link URL.
        /// </remarks>
        public async Task<LinkViewModel> CreateAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);

            link.DateCreated = DateTime.Now;

            await _linkRepository.CreateAsync(link);

            return linkViewModel;
        }

        /// <summary>
        /// Removes an existing link.
        /// </summary>
        /// <param name="linkViewModel">The view model representing the link to delete.</param>
        /// <returns>The view model representing the deleted link.</returns>
        public async Task<LinkViewModel> DeleteAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);
            await _linkRepository.DeleteAsync(link);

            return linkViewModel;
        }

        /// <summary>
        /// Gets a filtered list of links based on category, topic, search parameters.
        /// </summary>
        /// <param name="categoryId">Category ID to filter.</param>
        /// <param name="topicId">Topic ID to filter.</param>
        /// <param name="search">Search string to match name, category name, topic name, description and preview title.</param>
        /// <returns>The list of the links that match the provided filters.</returns>
        public async Task<IEnumerable<LinkViewModel>> GetAllByFilterAsync(
            int? categoryId = null,
            int? topicId = null,
            string? search = null)
        {
            var expressions = new List<Expression<Func<Link, bool>>>();

            if (categoryId is not null && categoryId != 0)
            {
                expressions.Add(_ => _.CategoryId.Equals(categoryId));
            }

            if (topicId is not null && topicId != 0)
            {
                expressions.Add(_ => _.TopicId.Equals(topicId));
            }

            if (!string.IsNullOrEmpty(search))
            {
                expressions.Add(_ =>
                        _.Name!.ToUpper().Contains(search.ToUpper()) ||
                        search.ToUpper().Contains(_.Name.ToUpper()) ||
                        _.Category!.Name!.ToUpper().Contains(search.ToUpper()) ||
                        search.ToUpper().Contains(_.Category.Name.ToUpper()) ||
                        _.Topic!.Name!.ToUpper().Contains(search.ToUpper()) ||
                        search.ToUpper().Contains(_.Topic.Name.ToUpper()) ||
                        _.Description!.ToUpper().Contains(search.ToUpper()) ||
                        search.ToUpper().Contains(_.Description.ToUpper()) ||
                        _.PreviewTitle!.ToUpper().Contains(search.ToUpper()) ||
                        search.ToUpper().Contains(_.PreviewTitle.ToUpper()));
            } 

            var links = await _linkRepository.GetAllByFilterAsync(

                include: query => query
                    .Include(_ => _.Category)
                    .Include(_ => _.Topic)!,
                expressions.ToArray());

            var linksViewModelList = _mapper.Map<IEnumerable<LinkViewModel>>(links);

            return linksViewModelList;
        }

        public async Task<IEnumerable<LinkViewModel>> SortedLinksAsync(
            int? categoryId = null,
            int? topicId = null,
            string? sortOrder = null)
        {
            var sortExpressions = new List<Expression<Func<Link, object>>>();

            if (categoryId.HasValue)
            {
                sortExpressions.Add(_ => _.CategoryId.Equals(categoryId.Value));
            }

            if (topicId.HasValue)
            {
                sortExpressions.Add(_ => _.TopicId.Equals(topicId.Value));
            }

            //var links = await _linkRepository.GetAllAsync(
            // include: query => query
            //     .Include(_ => _.Category!)
            //     .Include(_ => _.Topic!));

            //if (categoryId.HasValue)
            //{
            //    links = links.Where(_ => _.CategoryId == categoryId.Value);
            //}

            //if (topicId.HasValue)
            //{
            //    links = links.Where(_ => _.TopicId == topicId.Value);
            //}

            //switch (sortOrder!.ToLower())
            //{
            //    case "category":
            //        links = links.OrderByDescending(_ => _.Category!.Name).ToList();
            //        break;
            //    case "topic":
            //        links = links.OrderByDescending(_ => _.Topic!.Name).ToList();
            //        break;
            //    default:
            //        links = links.OrderByDescending(_ => _.Category!.Name).ToList();
            //        break;
            //}

            switch (sortOrder?.ToLower())
            {
                case "category":
                    sortExpressions.Add(_ => _.Category!.Name);
                    break;
                case "topic":
                    sortExpressions.Add(_ => _.Topic!.Name);
                    break;
                default:
                    sortExpressions.Add(_ => _.Category!.Name);
                    break;
            }

            var links = await _linkRepository.GetAllSortedAsync(
                include: query => query
                    .Include(_ => _.Category)
                    .Include(_ => _.Topic)!,
                sortExpressions.ToArray());

            var linksViewModelList = _mapper.Map<IEnumerable<LinkViewModel>>(links);

            return linksViewModelList;
        }

        /// <summary>
        /// Gets the list of all links.
        /// </summary>
        /// <returns>The view model representing the list of all links.</returns>
        /// <exception cref="LinkNotFoundException">Thrown when no links were found.</exception>
        public async Task<IEnumerable<LinkViewModel>> GetAllAsync()
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

        /// <summary>
        /// Retrieves a link by it's ID.
        /// </summary>
        /// <param name="linkId">ID of the link wanted to get.</param>
        /// <returns>The view model of a link with the given ID.</returns>
        /// <exception cref="LinkNotFoundException">Thrown when there is no link with such ID.</exception>
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

        /// <summary>
        /// Updates an existing link with the information provided in the linkViewModel.
        /// </summary>
        /// <param name="linkViewModel">Contains the updated information for the link.</param>
        /// <returns>The updated view model of a link.</returns>
        public async Task<LinkViewModel> UpdateAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);

            var modelFromDb = await _linkRepository.GetByIdAsync(link.Id);
            var modelFromDbCreated = modelFromDb!.DateCreated;
            link.DateCreated = modelFromDbCreated;

            await _linkRepository.UpdateAsync(link);

            return linkViewModel;
        }
    }
}
