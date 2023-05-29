using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basilisk.ViewModel.Supplier;

namespace Basilisk.ViewModel.Customer
{
    public class ProductCartViewModel
    {
        public int Id { get; set; }
        public long CustomerId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string Harga { get; set; }
    }
}
