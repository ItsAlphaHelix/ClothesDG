﻿namespace ClothesDG.Core.Contracts
{
    using ClothesDG.Core.ViewModels.Products;
    using ClothesDG.Core.ViewModels.Reviews;
    using ClothesDG.Core.ViewModels.Shared;

    public interface IProductsService
    {
        /// <summary>
        /// Getting current product by his id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductViewModel> GetProductByIdAsync(int productId);

        /// <summary>
        /// Getting current product's information.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<DetailsViewModel> GetProductDetailsByIdAsync(int productId, int pageNumber, int pageSize);

        /// <summary>
        /// Getting all products by gender.
        /// </summary>
        /// <returns></returns>
        IQueryable<ProductViewModel> GetAllProductsAsQueryable(PaginatedViewModel<ProductViewModel> model, bool? isMale, string? productName);

        Task<List<string>> GetAllSizesAsync();

        Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId);

        Task<IEnumerable<ProductViewModel>> GetRecommendedProductsAsync(int productId);

        Task<List<string>> GetProductNamesAsync();

        IQueryable<ProductViewModel> FilterProductsAsQueryable(PaginatedViewModel<ProductViewModel> model);

        IQueryable<ProductViewModel> SearchProductsByQueryAsQueryable(PaginatedViewModel<ProductViewModel> model, string searchBy);
    }
}
