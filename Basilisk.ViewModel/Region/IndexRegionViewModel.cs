using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Region
{
    public class IndexRegionViewModel
    {
        public string SearchCity { get; set; }
        public IEnumerable<GridRegionViewModel> GridRegion { get; set; }
        public int TotalRegion { get; set; }
        public int TotalHalaman { get; set; }
        public int TotalData { get; set; }
    }
}
