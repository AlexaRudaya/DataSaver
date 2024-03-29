﻿namespace DataSaver.ApplicationCore.Mapper
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