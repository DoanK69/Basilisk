using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Deliveries
{
    public class GridDeliveryViewModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public decimal Cost { get; set; }
        public string InvoiceNumber { get; set; }
        public string StringCost { get; set; }
    }
}
