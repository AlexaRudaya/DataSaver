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

        /// <summary>
        /// Displays the Index page with the list of all categories.
        /// </summary>
        /// <returns>The Index view for categories.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoriesList = await _categoryService.GetAllAsync();

            return View(categoriesList);
        }

        /// <summary>
        /// Displays a view for creating a category.
        /// </summary>
        /// <returns>The create category view.</returns>
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        /// <summary>
        /// Creates a new category. 
        /// </summary>
        /// <param name="categoryViewModel">The CategoryViewModel to create a new category.</param>
        /// <returns>
        /// If the model state is valid, it creates a new category and redirects to the Index page. 
        /// Otherwise, it returns the Create category view.
        /// </returns>
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

        /// <summary>
        /// Creates a view for updating a category with the certain ID.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <returns>The view for the category to update.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return View(category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryViewModel">The category view model to update.</param>
        /// <returns>
        /// A redirect to the Index page, if the model state is valid, 
        /// or the view with the received CategoryViewModel otherwise.
        /// </returns>
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

        /// <summary>
        /// Creates a view for deleting a category with the certain ID.
        /// </summary>
        /// <param name="id">The Id of the category to be deleted.</param>
        /// <returns>The view for the category to remove.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryToRemove = await _categoryService.GetByIdAsync(id);

            return View(categoryToRemove);        
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="categoryVMForRemove">The view model containing the category to remove.</param>
        /// <returns>A redirect to the index page.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(CategoryViewModel categoryVMForRemove)
        {
            var categoryToRemove = await _categoryService.GetByIdAsync(categoryVMForRemove.Id);
            await _categoryService.DeleteAsync(categoryToRemove);

            return RedirectToAction("Index");
        }
    }
}
