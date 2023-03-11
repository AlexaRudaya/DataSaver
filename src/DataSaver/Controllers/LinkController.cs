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
        public async Task<IActionResult> Index(int page = 1, int categoryId = 0, int topicId = 0)
        {
            int pageSize = 3;

            FilterViewModel filter = new();

            string? response = HttpContext.Session.GetString("Filter");

            if (response is null)
            {
                var categories = await _categoryService.GetAllAsync();
                var topics = await _topicService.GetAllAsync();

                filter.Links = await _linkService.GetAllAsync();

                filter.Categories = categories.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                });

                filter.Topics = topics.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name
                });
            }
            else
            {
                filter = JsonConvert.DeserializeObject<FilterViewModel>(response)!;

                var categories = await _categoryService.GetAllAsync();
                var topics = await _topicService.GetAllAsync();
                var allLinks = await _linkService.GetAllAsync();

                var categoryLinks = allLinks;

                if (filter.CategoryId is not null && filter.CategoryId != 0)
                {
                    categoryLinks = allLinks.Where(_ => _.CategoryId.Equals(categoryId));
                }

                var topicLinks = categoryLinks;

                if (filter.TopicId is not null && filter.TopicId != 0)
                {
                    topicLinks = categoryLinks.Where(_ => _.TopicId.Equals(topicId));
                }

                filter.Links = topicLinks;

                filter.Categories = categories.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name,
                    Selected = _.Id == categoryId
                });

                filter.Topics = topics.Select(_ => new SelectListItem
                {
                    Value = _.Id.ToString(),
                    Text = _.Name,
                    Selected = _.Id == topicId
                });
            }

            var totalCount = filter.Links.Count();

            var items = filter.Links.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(totalCount, page, pageSize);

            var viewModel = new FilterViewModel
            {
                PageViewModel = pageViewModel,
                Links = items,
                Categories = filter.Categories,
                Topics = filter.Topics,
                CategoryId = categoryId,
                TopicId = topicId,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(FilterViewModel filter, int page = 1)
        {
            ResponseViewModel responseViewModel = new()
            {
                CategoryId = filter.CategoryId,
                TopicId = filter.TopicId,
            };

            string response = JsonConvert.SerializeObject(responseViewModel);

            HttpContext.Session.SetString("Filter", response);

            return RedirectToAction(nameof(Index), new 
            { 
                Response = response, 
                page, 
                pageSize = 3,
                filter.CategoryId,
                filter.TopicId
            });
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
