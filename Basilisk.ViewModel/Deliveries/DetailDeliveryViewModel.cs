using Basilisk.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Deliveries
{
    public class DetailDeliveryViewModel
    {
        public GridDeliveryViewModel? Delivery { get; set; }
        public decimal TotalUangMasuk { get; set; }
        public IEnumerable<GridOrderViewModel> ListOrder { get; set; }
        public long CustomerId { get; set; }

    }
}
