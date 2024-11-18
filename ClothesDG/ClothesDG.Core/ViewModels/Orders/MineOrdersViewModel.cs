using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClothesDG.Core.ViewModels.Customers;

namespace ClothesDG.Core.ViewModels.Orders
{
    public class MineOrdersViewModel
    {
        public IQueryable<OrderViewModel> OrdersModel { get; set; }
    }
}
