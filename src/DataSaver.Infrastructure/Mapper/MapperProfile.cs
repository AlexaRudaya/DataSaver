using DataSaver.ApplicationCore.Entities;
using DataSaver.ApplicationCore.ViewModels;

namespace DataSaver.Infrastructure.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Link, LinkViewModel>().ReverseMap();
            CreateMap<Topic, TopicViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
        }
    }
}
