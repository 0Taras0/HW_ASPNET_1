using AutoMapper;
using WebSmonder.Areas.Admin.Models.Users;
using WebSmonder.Data.Entities.Identity;

namespace WebSmonder.Areas.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserEntity, UserItemViewModel>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ReverseMap();


        }
    }
}
