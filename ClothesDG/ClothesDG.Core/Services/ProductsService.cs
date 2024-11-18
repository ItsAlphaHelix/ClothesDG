namespace ClothesDG.Core.Services
{
    using AutoMapper;
    using ClothesDG.Core.Contracts;
    using ClothesDG.Core.ViewModels.Products;
    using ClothesDG.Core.ViewModels.Reviews;
    using ClothesDG.Core.ViewModels.Shared;
    using ClothesDG.Data.Data.Models;
    using ClothesDG.Data.Repositories;
    using global::AutoMapper;
    using global::AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<ProductReviews> productReviewsRepository;
        private readonly IRepository<Size> sizesRepository;
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IMapper mapper;
        public ProductsService(
            IRepository<Product> productsRepository,
            IRepository<ProductReviews> productReviewsRepository,
            IRepository<Size> sizesRepository,
            UserManager<ApplicationUser> usersManager,
            IMapper mapper)
        {
            this.productsRepository = productsRepository;
            this.productReviewsRepository = productReviewsRepository;
            this.sizesRepository = sizesRepository;
            this.usersManager = usersManager;
            this.mapper = mapper;
        }
        public IQueryable<ProductViewModel> GetAllProductsAsQueryable(
            PaginatedViewModel<ProductViewModel> model,
            bool? isMale,
            string? productName)
        {
            IQueryable<ProductViewModel> products = GetAllProductsFromDbAsQueryable(isMale, productName);

            return products;
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int productId)
        {

            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == productId)
                .ProjectTo<ProductViewModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            //var product = await this.productsRepository
            //    .AllAsNoTracking()
            //    .Where(x => x.Id == productId)
            //    .Select(x => new ProductViewModel()
            //    {
            //        Id = x.Id,
            //        Category = x.Category,
            //        Price = x.Price,
            //        IsProductInStock = x.ProductSizes.Any(x => x.Count != 0),
            //        ProductSizes = x.ProductSizes
            //        .Where(x => x.Count != 0)
            //        .Select(x => x.Size.Name)
            //        .ToList(),
            //        Images = x.Images.Select(x => x.Url)
            //        .ToList(),

            //    })
            //    .FirstOrDefaultAsync();

            return product;
        }

        public async Task<DetailsViewModel> GetProductDetailsByIdAsync(int productId, int pageNumber, int pageSize)
        {
            int countOfReviews = await this.GetProductReviewsCountAsync(productId);
            double averageRatingOfProduct = await this.CalculateAverageOfCurrentProduct(productId);

            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == productId)
                .Select(x => new DetailsViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Description = x.Description,
                    ClearInfo = x.ClearInfo,
                    IsMale = x.IsMale,
                    CountOfReviews = countOfReviews,
                    FiveStarts = x.ProductReviews.Where(x => x.Rating == 5.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 5.0).Count() : 100,
                    FourStarts = x.ProductReviews.Where(x => x.Rating == 4.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 4.0).Count() : 100,
                    ThreeStars = x.ProductReviews.Where(x => x.Rating == 3.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 3.0).Count() : 100,
                    TwoStars = x.ProductReviews.Where(x => x.Rating == 2.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 2.0).Count() : 100,
                    OneStar = x.ProductReviews.Where(x => x.Rating == 1.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 1.0).Count() : 100,
                    AverageRating = averageRatingOfProduct,
                    PercentageOfAverageStars = double.Parse(averageRatingOfProduct.ToString("F1")) * 20,
                    IsProductInStock = x.ProductSizes.Any(x => x.Count != 0),
                    Reviews = x.ProductReviews
                    .OrderByDescending(x => x.Date)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new GetProductReviewViewModel()
                    {
                        ProductId = x.ProductId,
                        UserFullName = x.UserFullName,
                        Rating = x.Rating,
                        Message = x.Message,
                        Date = x.Date,
                        UserProfileImageUrl = x.UserProfileImageUrl
                    }),
                    Sizes = x.ProductSizes
                    .Where(x => x.Count != 0)
                    .Select(x => new SizeViewModel()
                    {
                        SizeName = x.Size.Name
                    }),
                    Images = x.Images.Select(x => x.Url)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId)
        {
            var review = mapper.Map<ProductReviews>(productReview);

            //var review = new ProductReviews()
            //{
            //    ProductId = productReview.ProductId,
            //    UserFullName = productReview.UserFullName,
            //    Rating = productReview.Rating,
            //    Message = productReview.Message,
            //    Date = DateTime.Now,
            //    UserProfileImageUrl = productReview.UserProfileImageUrl,
            //};

            var product = await this.productsRepository
                .All()
                .Include(x => x.ProductReviews)
                .FirstOrDefaultAsync(x => x.Id == productReview.ProductId);
            product.ProductReviews.Add(review);
            await this.productReviewsRepository.SaveChangesAsync();
        }


        public async Task<List<string>> GetAllSizesAsync()
        {
            var sizes = await this.sizesRepository
                .AllAsNoTracking()
                .Select(x => x.Name)
                .ToListAsync();

            return sizes;
        }

        private async Task<double> CalculateAverageOfCurrentProduct(int productId)
        {
            var productReviews = await this.productReviewsRepository
                .AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            double averageRatingOfProduct = productReviews.Any() ? (productReviews.Sum(x => x.Rating) / productReviews.Count) : 0;

            return averageRatingOfProduct;
        }
        private async Task<int> GetProductReviewsCountAsync(int productId)
        {
            var countOfReviews = await this.productReviewsRepository.AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .CountAsync();

            return countOfReviews;
        }

        public async Task<IEnumerable<ProductViewModel>> GetRecommendedProductsAsync(int productId)
        {
            var product = await this.GetProductDetailsByIdAsync(productId, 1, 3);

            var recommendedProducts = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.IsMale == product.IsMale &&
                            x.Id != product.Id &&
                            x.Price <= product.Price &&
                            x.Category == product.Category &&
                            x.ProductSizes.Any(x => x.Count != 0))
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url).Take(2).ToList(),
                    ProductSizes = x.ProductSizes
                    .Where(x => x.Count != 0)
                    .Select(x => x.Size.Name)
                    .ToList()
                })
                .OrderBy(x => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            return recommendedProducts;
        }

        public async Task<List<string>> GetProductNamesAsync()
        {
            var productNames = await this.productsRepository
                .AllAsNoTracking()
                .Select(x => x.Category)
                .Distinct()
                .ToListAsync();

            return productNames;
        }

        public IQueryable<ProductViewModel> FilterProductsAsQueryable(PaginatedViewModel<ProductViewModel> model)
        {
              var  products = GetAllProductsFromDbAsQueryable(null, null);

                if (!string.IsNullOrWhiteSpace(model.SelectedSizes))
                {
                    string[] splitSelectedSizes = model.SelectedSizes.Split(",");
                    products = products.Where(x => x.ProductSizes.Any(x => splitSelectedSizes.Contains(x)));
                }

                if (model.MinPrice.HasValue)
                {
                    products = products.Where(p => Math.Round(p.Price) >= model.MinPrice.Value);
                }

                if (model.MaxPrice.HasValue)
                {
                    products = products.Where(p => Math.Round(p.Price) <= model.MaxPrice.Value);
                }

                switch (model.Sorting)
                {
                    case SortEnum.Default: products = products.AsQueryable(); break;
                    case SortEnum.AverageRating: products = products.OrderByDescending(x => x.AverageRating); break;
                    case SortEnum.PriceAsc: products = products.OrderBy(x => x.Price); break;
                    case SortEnum.PriceDesc: products = products.OrderByDescending(x => x.Price); break;
                }
           
            return products;
        }

        public IQueryable<ProductViewModel> SearchProductsByQueryAsQueryable(PaginatedViewModel<ProductViewModel> model, string searchBy)
        {
            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                var baseForm = searchBy.ToLower().Substring(0, Math.Min(4, searchBy.Length));

                var searchTerm = $"%{baseForm}%";

                var searchProducts = this.productsRepository
                   .AllAsNoTracking()
                   .OrderByDescending(x => x.ProductReviews.Average(x => x.Rating))
                   .Where(x => EF.Functions.Like(x.Description.ToLower(), searchTerm))
                   .ProjectTo<ProductViewModel>(this.mapper.ConfigurationProvider)
                   .AsQueryable();

                //var searchProducts = this.productsRepository
                //    .AllAsNoTracking()
                //    .Where(x => EF.Functions.Like(x.Description.ToLower(), searchTerm))
                //    .Select(x => new ProductViewModel()
                //    {
                //        Id = x.Id,
                //        Category = x.Category,
                //        Price = x.Price,
                //        AverageRating = x.ProductReviews.Any() ? (x.ProductReviews.Sum(x => x.Rating) / x.ProductReviews.Count) : 0,
                //        Images = x.Images.Select(x => x.Url).Take(2).ToList(),
                //        ProductSizes = x.ProductSizes
                //        .Where(x => x.Count != 0)
                //        .Select(x => x.Size.Name)
                //        .ToList()
                //    })
                //    .AsQueryable();

                return searchProducts;
            }

            return null;
        }

        private IQueryable<ProductViewModel> GetAllProductsFromDbAsQueryable(bool? isMale, string? productName)
        {

            var products = this.productsRepository
                                .AllAsNoTracking()
                                .OrderByDescending(x => x.ProductReviews.Average(x => x.Rating))
                                .Where(x =>
                                    (!isMale.HasValue || x.IsMale == isMale) && // Check if isMale is not null
                                    (string.IsNullOrWhiteSpace(productName) || x.Category == productName) // Check if productName is not empty
                                )
                                .ProjectTo<ProductViewModel>(this.mapper.ConfigurationProvider)
                                .AsQueryable();

            //var products = this.productsRepository
            //                    .AllAsNoTracking()
            //                    .Where(x =>
            //                        (!isMale.HasValue || x.IsMale == isMale) && // Check if isMale is not null
            //                        (string.IsNullOrWhiteSpace(productName) || x.Category == productName) // Check if productName is not empty
            //                    )
            //                    .Select(x => new ProductViewModel()
            //                    {
            //                        Id = x.Id,
            //                        Category = x.Category,
            //                        Price = x.Price,
            //                        AverageRating = x.ProductReviews.Any() ? x.ProductReviews.Average(x => x.Rating) : 0,
            //                        Images = x.Images.Select(img => img.Url).Take(2).ToList(),
            //                        ProductSizes = x.ProductSizes
            //                            .Where(size => size.Count != 0)
            //                            .Select(size => size.Size.Name)
            //                            .ToList()
            //                    })
            //                    .OrderByDescending(x => x.AverageRating)
            //                    .AsQueryable();
            return products;
        }
    }
}
