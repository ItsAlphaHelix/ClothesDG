namespace ClothesDG.Core.Contracts
{
    using ClothesDG.Core.ViewModels.Bags;
    using ClothesDG.Core.ViewModels.Products;

    public interface IBagsService
    {
        IQueryable<ProductBagViewModel> GetAllProductsInBagAsQueryable(string userId);

        Task AddProductToBagAsync(int productId, string sizeName, int quantity, string userId);

        Task<decimal> CalculateTotalPrice(string userId);

        Task<int> CountOfProductsInBagAsync(string userId);

        string GetOrCreateTemporaryUserId();

        Task DeleteProductFromBagAsync(int productId, string sizeName, string userId);

        Task DeleteAllProductsFromBagAsync(string userId);

        Task<int> GetTotalQuantityOfSizeOfProduct(string sizeName, int productId);

        Task DecrementQuantityOfProductAsync(string sizeName, int productId, string userId);

        Task IncrementQuantityOfProductAsync(string sizeName, int productId, string userId, int currentQuantity);

        Task<IEnumerable<ProductViewModel>> GetRecommendedProductsInBag(string userId);
    }
}
