using AutoMapper;
using OnlineShop.Api.Data.Entities;
using OnlineShop.Api.DTOs;

namespace OnlineShop.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CreateUpdateUserDto>();
            CreateMap<CreateUpdateUserDto, User>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
