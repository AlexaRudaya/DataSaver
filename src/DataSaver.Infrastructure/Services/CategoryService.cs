namespace DataSaver.Infrastructure.Services
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

        public async Task<CategoryViewModel> CreateAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            category.DateCreated = DateTime.Now;

            await _categoryRepository.CreateAsync(category);

            return categoryViewModel;
        }

        public async Task<CategoryViewModel> DeleteAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            await _categoryRepository.DeleteAsync(category);

            return categoryViewModel;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var categoryList = await _categoryRepository.GetAllAsync();

            if (categoryList == null)
            {
                var exception = new CategoryNotFoundException("No categories were found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var categoriesViewModelList = _mapper.Map<IEnumerable<CategoryViewModel>>(categoryList);

            return categoriesViewModelList;
        }

        public Task<IEnumerable<CategoryViewModel>> GetAllWithFiltersAsync(int? categoryId, int? topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryViewModel> GetByIdAsync(int categoryId)
        {
            var entity = await _categoryRepository.GetByIdAsync(categoryId);

            if (entity == null)
            {
                var exception = new CategoryNotFoundException($"No category with id: {categoryId} was found");
                _logger.LogError(exception, exception.Message);

                throw exception;
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(entity);

            return categoryViewModel;
        }

        public async Task<CategoryViewModel> UpdateAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            var modelFromDb = await _categoryRepository.GetByIdAsync(category.Id);
            var modelFromDbCreated = modelFromDb!.DateCreated;
            category.DateCreated = modelFromDbCreated;

            await _categoryRepository.UpdateAsync(category);

            return categoryViewModel;
        }
    }
}
