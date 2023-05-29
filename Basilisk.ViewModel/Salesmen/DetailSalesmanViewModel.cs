using Basilisk.ViewModel.Product;
using Basilisk.ViewModel.Region;
using Basilisk.ViewModel.SalesmanRegions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Salesmen
{
    public class DetailSalesmanViewModel
    {
        public GridSalesmenViewModel? GridSales { get; set; }
        public IEnumerable<GridSalesmanRegionsViewModel> GridRegions { get; set; }
        public int TotalRegion { get; set; }
    }
}
