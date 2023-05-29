using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Salesmen
{
    public class IndexSalesViewModel
    {
        public string SearchName { get; set; }
        public int TotalSales { get; set; }
        public int TotalData { get; set; }
        public int TotalHalaman { get; set; }
        public IEnumerable<GridSalesmenViewModel> GridSales { get; set; }
    }
}
