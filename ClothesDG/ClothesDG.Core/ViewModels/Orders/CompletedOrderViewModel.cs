using ClothesDG.Core.ViewModels.Customers;

namespace ClothesDG.Core.ViewModels.Orders
{
    public class CompletedOrderViewModel : CustomerViewModel
    {
        public string OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public IEnumerable<ProductOrderViewModel> ProductOrderModel { get; set; } = new List<ProductOrderViewModel>();
    }
}
