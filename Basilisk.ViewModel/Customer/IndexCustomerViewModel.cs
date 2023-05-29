using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class IndexCustomerViewModel
    {
        public string SearchName { get; set; }
        public IEnumerable<GridCustomerViewModel> Grid { get; set; }
        public int TotalHalaman { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalData { get; set; }
    }
}
