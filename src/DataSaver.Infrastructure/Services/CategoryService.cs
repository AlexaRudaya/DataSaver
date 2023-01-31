using AutoMapper;
using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.Exceptions;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.ApplicationCore.Interfaces.IService;
using DataSaver.ApplicationCore.ViewModels;

namespace DataSaver.Infrastructure.Services
{
    public sealed class CategoryService : ICategoryService  
    {
        private readonly IBaseRepository<Category> _baseRepository;
        private readonly IMapper _mapper;

        public CategoryService(IBaseRepository<Category> baseRepository,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public async Task<CategoryViewModel> CreateAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            await _baseRepository.CreateAsync(category);

            return categoryViewModel;
        }

        public async Task<CategoryViewModel> DeleteAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            await _baseRepository.DeleteAsync(category);

            return categoryViewModel;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var categoryList = await _baseRepository.GetAllAsync();

            if (categoryList == null)
            {
                string errorMessage = $"No categories were found";
                throw new CategoryNotFoundException(errorMessage);
            }

            var categoriesViewModelList = _mapper.Map<IEnumerable<CategoryViewModel>>(categoryList);

            return categoriesViewModelList;
        }

        public async Task<CategoryViewModel> GetByIdAsync(int categoryId)
        {
            var entity = await _baseRepository.GetByIdAsync(categoryId);

            if (entity == null)
            {
                string errorMessage = $"No category with id: {categoryId} was found";
                throw new CategoryNotFoundException(errorMessage);
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(entity);

            return categoryViewModel;
        }

        public async Task<CategoryViewModel> UpdateAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);
            await _baseRepository.UpdateAsync(category);

            return categoryViewModel;
        }
    }
}
