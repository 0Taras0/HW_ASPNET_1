using AutoMapper;
using WebSmonder.Data.Entities;
using WebSmonder.Models.Category;
using WebSmonder.Models.Product;
namespace WebSmonder.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductEntity, ProductItemViewModel>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(x => x.Name)));

        CreateMap<ProductItemCreateModel, ProductEntity>()
            .ForMember(x => x.DescriptionImages, opt => opt.Ignore());

        CreateMap<ProductEntity, DeleteProductViewModel>()
            .ForMember(x => x.ProductImageNames, opt => opt.MapFrom(x => x.ProductImages.Select(img => img.Name)))
            .ReverseMap();
    }
}
