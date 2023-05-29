using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Orders
{
    public class IndexOrderViewModel
    {
        public string SearchName { get; set; }
        public int TotalHalaman { get; set; }
        public int TotalData { get; set; }
        public int TotalOrder { get; set; }
        public IEnumerable<GridOrderViewModel> GridOrder { get; set; }
    }
}
