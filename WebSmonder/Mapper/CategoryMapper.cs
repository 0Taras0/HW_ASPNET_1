using AutoMapper;
using WebSmonder.Data.Entities;
using WebSmonder.Models.Category;
namespace WebSmonder.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper() 
    {
        CreateMap<CategoryEntity, CategoryItemViewModel>()
            .ForMember(x => x.Image, opt => opt.MapFrom(x => x.ImageUrl));

        CreateMap<CategoryCreateViewModel, CategoryEntity>()
            .ForMember(x => x.ImageUrl, opt => opt.Ignore());

        CreateMap<CategoryEntity, CategoryEditViewModel>()
            .ForMember(x => x.ViewImage, opt => opt.MapFrom(x => string.IsNullOrEmpty(x.ImageUrl) ? "/picture/default.jpg" : $"/images/400_{x.ImageUrl}"))
            .ForMember(x => x.ImageFile, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<CategoryEntity, CategoryDeleteViewModel>()
            .ReverseMap();
    }
}
