using Basilisk.ViewModel.Orders;
using Basilisk.ViewModel.Salesmen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class DetailOrderCustomerViewModel
    {
        public GridCustomerViewModel? Customer { get; set; }
        public GridSalesmenViewModel? Salesmen { get; set; }
        public List<GridOrderViewModel> ListOrderDetail { get; set; }

        public List<GridOrderViewModel> Order { get; set; }
    }
}
