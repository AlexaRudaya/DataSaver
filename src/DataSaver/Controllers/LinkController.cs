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

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{ 
        //    var previews = await _linkService.GetLinkPreviewAsync();
        //    return View(previews);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(string? response)
        {
            FilterViewModel filter = new();

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
                    categoryLinks = allLinks.Where(_ => _.CategoryId.Equals(filter.CategoryId));
                }

                var topicLinks = categoryLinks;

                if (filter.TopicId is not null && filter.TopicId != 0)
                {
                    topicLinks = categoryLinks.Where(_ => _.TopicId.Equals(filter.TopicId));
                }

                filter.Links = topicLinks;

                //filter.Links = allLinks.Where(_=>_.CategoryId.Equals(filter.CategoryId)
                //    && _.TopicId.Equals(filter.TopicId));

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

            return View(filter);
        }

        [HttpPost]
        public IActionResult Index(FilterViewModel filter)
        {
            ResponseViewModel responseViewModel = new()
            {
                CategoryId = filter.CategoryId,
                TopicId = filter.TopicId,
            };

            string response = JsonConvert.SerializeObject(responseViewModel);

            return RedirectToAction(nameof(Index), new { Response = response });
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
