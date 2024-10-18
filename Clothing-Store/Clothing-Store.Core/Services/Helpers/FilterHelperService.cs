namespace Clothing_Store.Core.Services.HelperServices
{
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Shared;
    public class FilterHelperService
    {
        protected static IQueryable<ProductViewModel> FilterProducts(
            PaginatedViewModel<ProductViewModel> model,
            IQueryable<ProductViewModel> products)
        {
            if (!string.IsNullOrWhiteSpace(model.SelectedProducts))
            {
                products = products.Where(x => model.SelectedProducts.Contains(x.Category));
            }


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
    }
}
