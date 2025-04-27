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
                .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<AccountRegisterViewModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<UserEntity, AccountProfileViewModel>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.ViewImage, opt => opt.MapFrom(src => src.Image));

            CreateMap<AccountProfileViewModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.Image, opt => opt.MapFrom(src => src.ViewImage));
        }
    }
}
