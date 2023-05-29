using Basilisk.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class DetailProductViewModel
    {
        public long ProductId { get; set; }
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string StringPrice { get; set; }
        public int Qty { get; set; }
        public bool Checked { get; set; }
        public decimal TotalHarga { get; set; }
        public string StringTotalHarga { get; set; }
    }
}
