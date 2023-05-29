using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.OrderDetails
{
    public class GridOrderDetailsViewModel
    {
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string Harga { get; set; }
        public string Diskon { get; set; }
    }
}
