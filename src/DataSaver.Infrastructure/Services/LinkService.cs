using AutoMapper;
using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.Exceptions;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.ApplicationCore.Interfaces.IService;
using DataSaver.ApplicationCore.ViewModels;

namespace DataSaver.Infrastructure.Services
{
    public sealed class LinkService : ILinkService
    {
        private readonly IBaseRepository<Link> _baseRepository;
        private readonly IMapper _mapper; 

        public LinkService(IBaseRepository<Link> baseRepository,
                IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public async Task<LinkViewModel> CreateAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);

            await _baseRepository.CreateAsync(link);

            return linkViewModel;
        }

        public async Task<LinkViewModel> DeleteAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);
            await _baseRepository.DeleteAsync(link);

            return linkViewModel;
        }

        public async Task<IEnumerable<LinkViewModel>> GetAllAsync()
        {
            var linksList = await _baseRepository.GetAllAsync();

            if (linksList == null)
            {
                string errorMessage = $"No links were found";
                throw new LinkNotFoundException(errorMessage);
            }

            var linksViewModelList = _mapper.Map<IEnumerable<LinkViewModel>>(linksList);

            return linksViewModelList;
        }

        public async Task<LinkViewModel> GetByIdAsync(int linkId)
        {
            var entity = await _baseRepository.GetByIdAsync(linkId);    

            if (entity == null) 
            {
                string errorMessage = $"No link with id: {linkId} was found";
                throw new LinkNotFoundException(errorMessage);
            }

            var linkViewModel = _mapper.Map<LinkViewModel>(entity);

            return linkViewModel;   
        }

        public async Task<LinkViewModel> UpdateAsync(LinkViewModel linkViewModel)
        {
            var link = _mapper.Map<Link>(linkViewModel);
            await _baseRepository.UpdateAsync(link);

            return linkViewModel;
        }
    }
}
