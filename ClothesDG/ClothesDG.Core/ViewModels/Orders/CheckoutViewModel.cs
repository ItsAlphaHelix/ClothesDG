namespace ClothesDG.Core.ViewModels.Orders
{
    using ClothesDG.Core.ViewModels.Bags;
    using ClothesDG.Core.ViewModels.Customers;

    public  class CheckoutViewModel
    {
        public CustomerViewModel CustomerModel { get; set; }

        public IEnumerable<ProductBagViewModel> ProductsInBag { get; set; } = new List<ProductBagViewModel>();

    }
}
