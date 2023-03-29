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

        [HttpGet]
        public IActionResult PreIndex()
        { 
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }

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

        [HttpPost]
        public async Task<IActionResult> DeleteLink(LinkForCreateViewModel linkToRemoveVM)
        {
            var linkToRemove = await _linkService.GetByIdAsync(linkToRemoveVM.LinkVM!.Id);
            await _linkService.DeleteAsync(linkToRemove);

            return RedirectToAction("Index");
        }
    }
}
