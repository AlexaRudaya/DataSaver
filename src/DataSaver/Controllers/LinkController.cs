using Newtonsoft.Json;

namespace DataSaver.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILinkService _linkService;
        private readonly ICategoryService _categoryService;
        private readonly ITopicService _topicService;
        private readonly ILogger<LinkController> _logger;

        public LinkController(ILinkService linkService,
            ICategoryService categoryService,
            ITopicService topicService,
            ILogger<LinkController> logger)
        {
            _linkService = linkService;
            _categoryService = categoryService;
            _topicService = topicService;
            _logger = logger;
        }

        /// <summary>
        /// Redirects to the Index action after clearing the HttpContext Session data.
        /// </summary>
        /// <returns>The Index view.</returns>
        [HttpGet]
        public IActionResult PreIndex()
        { 
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the Index page.
        /// </summary>
        /// <param name="pageNumber">The page number to display, default - 1.</param>
        /// <returns>The Index view.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            FilterViewModel viewModel = new();

            var categories = await _categoryService.GetAllAsync();
            var topics = await _topicService.GetAllAsync();
            var links = await _linkService.GetAllAsync();

            viewModel.CategoriesList = categories.Select(_ => new SelectListItem
            {
                Value = _.Id.ToString(),
                Text = _.Name
            });

            viewModel.TopicsList = topics.Select(_ => new SelectListItem
            {
                Value = _.Id.ToString(),
                Text = _.Name
            });

            string? jsonFilter = HttpContext.Session.GetString("Filter");

            if (jsonFilter is not null)
            {
                viewModel.ResponseViewModel = JsonConvert.DeserializeObject<ResponseViewModel>(jsonFilter);

                links = await _linkService.GetAllByFilterAsync(
                    viewModel.ResponseViewModel!.CategoryId,
                    viewModel.ResponseViewModel!.TopicId,
                    viewModel.ResponseViewModel!.SearchTerm);
            }

            var count = links.Count();
            viewModel.PageViewModel = new(count, pageNumber);
            viewModel.Links = links.Skip((pageNumber - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize);

            return View(viewModel);
        }

        /// <summary>
        /// Sets a filter for the Index page and redirects to the page with the filtered results.
        /// </summary>
        /// <param name="viewModel">The view model containing the filter data.</param>
        /// <param name="pageNumber">The page number to display.</param>
        /// <returns>The redirection to the Index page with the filtered results.</returns>
        [HttpPost]
        public IActionResult Index(FilterViewModel viewModel, int pageNumber = 1)
        {
            ResponseViewModel responseViewModel = new()
            {
                CategoryId = viewModel.ResponseViewModel!.CategoryId,
                TopicId = viewModel.ResponseViewModel!.TopicId,
                SearchTerm = viewModel.ResponseViewModel!.SearchTerm,
            };

            string jsonFilter = JsonConvert.SerializeObject(responseViewModel);

            HttpContext.Session.SetString("Filter", jsonFilter);

            return RedirectToAction(nameof(Index), new {pageNumber});        
        }

        /// <summary>
        ///  Retrieves all categories and topics and passes them to the view model. 
        /// </summary>
        /// <returns>The view for link creation.</returns>
        [HttpGet]
        public async Task<IActionResult> CreateLink()
        {
            var categories = await _categoryService.GetAllAsync();

            var topics = await _topicService.GetAllAsync();

            LinkForCreateViewModel model = new()
            {
                LinkVM = new(),

                Categories = categories.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                }),

                Topics = topics.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                })
            };

            return View(model);
        }

        /// <summary>
        /// Creates a new link. 
        /// </summary>
        /// <param name="linkForCreateVM">Contains the LinkViewModel and lists of categories and topics.</param>
        /// <returns>
        /// A redirect to the Index page, if the model state is valid, 
        /// or the view with the received LinkForCreateViewModel otherwise.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateLink(LinkForCreateViewModel linkForCreateVM)
        {
            if (ModelState.IsValid)
            {
                var linkViewModel = linkForCreateVM.LinkVM;

                await _linkService.CreateAsync(linkViewModel!);

                TempData[Constants.Success] = "You successfully created a new link";

                return RedirectToAction("Index");
            }
            else
            {
                return View(linkForCreateVM);
            }
        }

        /// <summary>
        /// Creates a view for updating a link with the certain ID along with its categories and topics.
        /// </summary>
        /// <param name="id">The Id of the link to update.</param>
        /// <returns>The view for the link to update.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateLink(int id)
        {
            var categories = await _categoryService.GetAllAsync();

            var topics = await _topicService.GetAllAsync();

            LinkForCreateViewModel model = new()
            {
                LinkVM = await _linkService.GetByIdAsync(id),

                Categories = categories.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                }),

                Topics = topics.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                })
            };

            return View(model);
        }

        /// <summary>
        /// Updates an existing link.
        /// </summary>
        /// <param name="linkForCreateVM">The link view model to update.</param>
        /// <returns>
        /// A redirect to the Index page, if the model state is valid, 
        /// or the view with the received LinkForCreateViewModel otherwise.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> UpdateLink(LinkForCreateViewModel linkForCreateVM)
        {
            var modelForUpdate = linkForCreateVM.LinkVM;

            if (ModelState.IsValid)
            {
                var updatedLink = await _linkService.UpdateAsync(modelForUpdate!);

                return RedirectToAction("Index");
            }

            else return View(linkForCreateVM);
        }

        /// <summary>
        /// Creates a view for deleting a link with the certain ID along with its categories and topics.
        /// </summary>
        /// <param name="id">The Id of the link to be deleted.</param>
        /// <returns>The view for the link to remove.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var categories = await _categoryService.GetAllAsync();

            var topics = await _topicService.GetAllAsync();

            LinkForCreateViewModel model = new()
            {
                LinkVM = await _linkService.GetByIdAsync(id),

                Categories = categories.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                }),

                Topics = topics.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                })
            };

            return View(model);
        }

        /// <summary>
        /// Deletes a link.
        /// </summary>
        /// <param name="linkToRemoveVM">The view model containing the link to remove.</param>
        /// <returns>A redirect to the index page.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteLink(LinkForCreateViewModel linkToRemoveVM)
        {
            var linkToRemove = await _linkService.GetByIdAsync(linkToRemoveVM.LinkVM!.Id);
            await _linkService.DeleteAsync(linkToRemove);

            return RedirectToAction("Index");
        }
    }
}
