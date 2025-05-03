using AutoMapper;
using WebSmonder.Data.Entities.Identity;
using WebSmonder.Models.Account;

namespace WebSmonder.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<UserEntity, AccountRegisterViewModel>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<AccountRegisterViewModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<UserEntity, AccountProfileViewModel>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.ViewImage, opt => opt.MapFrom(src => src.Image));

            CreateMap<AccountProfileViewModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.Image, opt => opt.MapFrom(src => src.ViewImage));

            CreateMap<UserEntity, UserLinkViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"))
                .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Image ?? $"default.webp"));
        }
    }
}
