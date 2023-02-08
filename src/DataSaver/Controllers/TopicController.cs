namespace DataSaver.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicController> _logger;

        public TopicController(ITopicService topicService, 
            ILogger<TopicController> logger)
        {
            _topicService = topicService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Here we get the topics list");

            var topicsList = await _topicService.GetAllAsync();

            return View(topicsList);
        }

        [HttpGet]
        public IActionResult CreateTopic()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(TopicViewModel topicViewModel)
        {
            if (ModelState.IsValid)
            {
                await _topicService.CreateAsync(topicViewModel);

                return RedirectToAction("Index");
            }

            else return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTopic(int id)
        { 
            var topic = await _topicService.GetByIdAsync(id);

            return View(topic);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTopic(TopicViewModel topicViewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedTopic = await _topicService.UpdateAsync(topicViewModel);

                return RedirectToAction("Index");
            }

            else return View(topicViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topicToRemove = await _topicService.GetByIdAsync(id);

            return View(topicToRemove);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTopic(TopicViewModel topicVMForRemove) 
        {
            var topicToRemove = await _topicService.GetByIdAsync(topicVMForRemove.Id);
            await _topicService.DeleteAsync(topicToRemove);

            return RedirectToAction("Index");
        }
    }
}
