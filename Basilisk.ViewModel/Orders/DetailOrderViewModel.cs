using Basilisk.ViewModel.Deliveries;
using Basilisk.ViewModel.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Orders
{
    public class DetailOrderViewModel
    {
        public GridOrderViewModel? GridOrder { get; set; }
        public IEnumerable<GridOrderDetailsViewModel> GridOrderDetails { get; set; } 

    }
}
