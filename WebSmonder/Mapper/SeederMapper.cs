using AutoMapper;
using WebSmonder.Data.Entities;
using WebSmonder.Data.Entities.Identity;
using WebSmonder.Models.Seeder;

namespace WebSmonder.Mapper;

public class SeederMapper : Profile
{
    public SeederMapper() 
    {
        CreateMap<SeederCategoryModel, CategoryEntity>()
            .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Image));
        CreateMap<SeederUserModel, UserEntity>()
            .ForMember(x => x.Image, opt => opt.Ignore());
    }
}
