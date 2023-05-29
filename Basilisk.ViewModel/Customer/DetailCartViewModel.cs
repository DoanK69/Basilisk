using Basilisk.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class DetailCartViewModel
    {
        public long CustomerId { get; set; }
        public string CompanyName { get; set; }
        public List<DetailProductViewModel> Products { get; set; }
        public bool ChekedAll { get; set; }
    }
}
