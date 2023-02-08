namespace DataSaver.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, 
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Here we get the categories list");

            var categoriesList = await _categoryService.GetAllAsync();

            return View(categoriesList);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateAsync(categoryViewModel);

                return RedirectToAction("Index");
            }

            else return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryViewModel categoryViewModel)
        { 
            if(ModelState.IsValid)
            {
                var updatedCategory = await _categoryService.UpdateAsync(categoryViewModel);

                return RedirectToAction("Index");
            }

            else return View(categoryViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryToRemove = await _categoryService.GetByIdAsync(id);

            return View(categoryToRemove);        
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(CategoryViewModel categoryVMForRemove)
        {
            var categoryToRemove = await _categoryService.GetByIdAsync(categoryVMForRemove.Id);
            await _categoryService.DeleteAsync(categoryToRemove);

            return RedirectToAction("Index");
        }
    }
}
