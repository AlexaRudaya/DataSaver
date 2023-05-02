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

        /// <summary>
        /// Displays the Index page with the list of all topics.
        /// </summary>
        /// <returns>The Index view for topics.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var topicsList = await _topicService.GetAllAsync();

            return View(topicsList);
        }

        /// <summary>
        /// Displays a view for creating a topic.
        /// </summary>
        /// <returns>The create topic view.</returns>
        [HttpGet]
        public IActionResult CreateTopic()
        {
            return View(); 
        }

        /// <summary>
        /// Creates a new topic. 
        /// </summary>
        /// <param name="topicViewModel">The TopicViewModel to create a new topic.</param>
        /// <returns>
        /// If the model state is valid, it creates a new topic and redirects to the Index page. 
        /// Otherwise, it returns the Create topic view.
        /// </returns>
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

        /// <summary>
        /// Creates a view for updating a topic with the certain ID.
        /// </summary>
        /// <param name="id">The ID of the topic to update.</param>
        /// <returns>The view for the topic to update.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateTopic(int id)
        { 
            var topic = await _topicService.GetByIdAsync(id);

            return View(topic);
        }

        /// <summary>
        /// Updates an existing topic.
        /// </summary>
        /// <param name="topicViewModel">The topic view model to update.</param>
        /// <returns>
        /// A redirect to the Index page, if the model state is valid, 
        /// or the view with the received TopicViewModel otherwise.
        /// </returns>
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

        /// <summary>
        /// Creates a view for deleting a topic with the certain ID.
        /// </summary>
        /// <param name="id">The Id of the topic to be deleted.</param>
        /// <returns>The view for the topic to remove.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topicToRemove = await _topicService.GetByIdAsync(id);

            return View(topicToRemove);
        }

        /// <summary>
        ///  Deletes a topic.
        /// </summary>
        /// <param name="topicVMForRemove">The view model containing the topic to remove.</param>
        /// <returns>A redirect to the index page.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteTopic(TopicViewModel topicVMForRemove) 
        {
            var topicToRemove = await _topicService.GetByIdAsync(topicVMForRemove.Id);
            await _topicService.DeleteAsync(topicToRemove);

            return RedirectToAction("Index");
        }
    }
}
