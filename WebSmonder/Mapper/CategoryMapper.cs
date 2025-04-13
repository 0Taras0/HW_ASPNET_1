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
        CreateMap<CategoryCreateViewModel, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryEditViewModel>();
        CreateMap<CategoryEditViewModel, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryDeleteViewModel>();
        CreateMap<CategoryDeleteViewModel, CategoryEntity>();
    }
}
