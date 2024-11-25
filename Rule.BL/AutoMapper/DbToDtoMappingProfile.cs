using AutoMapper;
using Rule.BL.Models;
using Rule.DAL.Entities;

namespace Rule.BL.AutoMapper
{
    public class DbToDtoMappingProfile: Profile
    {
        public DbToDtoMappingProfile()
        {
            CreateMap<Foundations, FoundationsDTO>().ReverseMap();
            CreateMap<Posts, PostsDTO>().ReverseMap();
            CreateMap<StatusPost, StatusPostDTO>().ReverseMap();
            CreateMap<TypePost, TypePostDTO>().ReverseMap();
            CreateMap<Users,  UsersDTO>().ReverseMap();
        }
    }
}
