using AutoMapper;
using ClothesDG.Core.ViewModels.Products;
using ClothesDG.Core.ViewModels.Reviews;
using ClothesDG.Data.Data.Models;

namespace Clothing_Store.Core.Services.AutoMapper
{

    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductViewModel>()
            .ForMember(x => x.AverageRating,
                       x => x.MapFrom(x => x.ProductReviews.Any()
                                                ? x.ProductReviews.Average(r => r.Rating)
                                                : 0))
            .ForMember(x => x.Images,
                       x => x.MapFrom(x => x.Images.Select(x => x.Url).Take(2).ToList()))
            .ForMember(x => x.ProductSizes,
                       x => x.MapFrom(x => x.ProductSizes
                                                  .Where(x => x.Count != 0)
                                                  .Select(x => x.Size.Name)
                                                  .ToList()));

            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.IsProductInStock,
                           x => x.MapFrom(x => x.ProductSizes.Any(x => x.Count != 0)))
                .ForMember(x => x.ProductSizes,
                           x => x.MapFrom(x => x.ProductSizes
                                                      .Where(x => x.Count != 0)
                                                      .Select(x => x.Size.Name)
                                                      .ToList()))
                .ForMember(x => x.Images,
                           x => x.MapFrom(x => x.Images.Select(x => x.Url).ToList()));


            CreateMap<PostProductReviewViewModel, ProductReviews>()
                .ForMember(x => x.Date,
                           x => x.MapFrom(_ => DateTime.Now));
        }
    }
}
