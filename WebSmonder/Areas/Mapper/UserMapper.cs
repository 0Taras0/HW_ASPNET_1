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

            CreateMap<UserEntity, UserItemEditModel>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(x => x.Image, opt => opt.Ignore())
            .ForMember(x => x.ViewImage, opt => opt.MapFrom(src => src.Image))
            .ReverseMap();
        }
    }
}
