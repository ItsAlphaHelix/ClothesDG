﻿namespace Clothing_Store.Core.ViewModels.Shared
{
    using Clothing_Store.Core.ViewModels.Customers;
    using Clothing_Store.Core.ViewModels.Products;
    public class PaginatedViewModel<T>
    {
        public PaginatedList<T> Models { get; set; }

        public SortEnum Sorting { get; set; }

        public string SelectedProducts { get; set; }

        public string SelectedPrice { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string SelectedSizes { get; set; }

        public IEnumerable<ProductViewModel> RecommendedProducts { get; set; } = new List<ProductViewModel>();
    }
}