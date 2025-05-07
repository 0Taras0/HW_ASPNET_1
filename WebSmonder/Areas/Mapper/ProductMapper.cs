using AutoMapper;
using WebSmonder.Areas.Admin.Models.Products;
using WebSmonder.Data.Entities;

namespace WebSmonder.Areas.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(x => x.Name)));

            CreateMap<ProductItemCreateModel, ProductEntity>()
                .ForMember(x => x.ProductImages, opt => opt.Ignore())
                .ForMember(x => x.CategoryId, opt => opt.Ignore());

        }
    }
}
