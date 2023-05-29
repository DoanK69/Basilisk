using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class IndexCartViewModel
    {
        public long CustomerId { get; set; }
        public List<DetailCartViewModel>? DetailCart { get; set; }
        public decimal TotalHarga { get; set; }
        public string StringTotalHarga { get; set; }
    }
}
