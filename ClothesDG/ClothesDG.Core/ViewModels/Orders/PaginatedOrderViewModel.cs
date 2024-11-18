namespace ClothesDG.Views.Orders
{
    using ClothesDG.Core.ViewModels.Customers;
    using ClothesDG.Core.ViewModels.Shared;

    public class PaginatedOrderViewModel<T>
    {
        public PaginatedList<T> Models { get; set; }

        public CustomerViewModel CustomerModel { get; set; }
    }
}
