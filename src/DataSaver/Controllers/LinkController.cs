namespace DataSaver.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILinkService _linkService;
        private readonly ILogger<LinkController> _logger;

        public LinkController(ILinkService linkService,
            ILogger<LinkController> logger)
        {
            _linkService = linkService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var linksList = await _linkService.GetAllAsync();
            return View(linksList);
        }
    }
}
