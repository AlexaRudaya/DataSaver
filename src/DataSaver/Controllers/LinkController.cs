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
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Here we get the list of the links");

            var linksList = await _linkService.GetAllAsync();

            return View(linksList);
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

                return RedirectToAction("Index");
            }
            else
            {
                return View(linkForCreateVM);
            }
        }

    }
}
