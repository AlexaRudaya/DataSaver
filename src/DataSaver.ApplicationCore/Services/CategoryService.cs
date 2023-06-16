namespace DataSaver.ApplicationCore.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryViewModel">The view model for the category to be created.</param>
        /// <returns>The view model for the ctreated category.</returns>
        public async Task<CategoryViewModel> CreateAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            category.DateCreated = DateTime.UtcNow;

            await _categoryRepository.CreateAsync(category);

            return categoryViewModel;
        }

        /// <summary>
        /// Removes an existing category.
        /// </summary>
        /// <param name="categoryViewModel">The view model representing the category to delete.</param>
        /// <returns>The view model representing the deleted category.</returns>
        public async Task<CategoryViewModel> DeleteAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            await _categoryRepository.DeleteAsync(category);

            return categoryViewModel;
        }

        /// <summary>
        /// Gets all of the categories.
        /// </summary>
        /// <returns>The view model representing the list of all categories.</returns>
        /// <exception cref="CategoryNotFoundException">Thrown when no categories were found.</exception>
        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var categoryList = await _categoryRepository.GetAllByAsync();

            if (categoryList is null)
            {
                var exception = new CategoryNotFoundException("No categories were found");
                throw exception;
            }

            var categoriesViewModelList = _mapper.Map<IEnumerable<CategoryViewModel>>(categoryList);

            return categoriesViewModelList;
        }

        /// <summary>
        ///  Retrieves a category by it's ID. 
        /// </summary>
        /// <param name="categoryId">ID of the category wanted to get.</param>
        /// <returns>The view model of a category with the given ID.</returns>
        /// <exception cref="CategoryNotFoundException">Thrown when there is no category with such ID.</exception>
        public async Task<CategoryViewModel> GetByIdAsync(int categoryId)
        {
            var entity = await _categoryRepository.GetOneByAsync(expression: _ => _.Id.Equals(categoryId));

            if (entity is null)
            {
                var exception = new CategoryNotFoundException($"No category with id: {categoryId} was found");
                throw exception;
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(entity);

            return categoryViewModel;
        }

        /// <summary>
        /// Updates an existing category with the information provided in the categoryViewModel.
        /// </summary>
        /// <param name="categoryViewModel">Contains the updated information for the category.</param>
        /// <returns>The updated view model of a category.</returns>
        public async Task<CategoryViewModel> UpdateAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);
            var modelFromDb = await _categoryRepository.GetOneByAsync(expression: _ => _.Id.Equals(category.Id));
            var modelFromDbCreated = modelFromDb!.DateCreated;
            category.DateCreated = modelFromDbCreated;

            await _categoryRepository.UpdateAsync(category);

            return categoryViewModel;
        }
    }
}