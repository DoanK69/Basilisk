using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Category
{
    public class IndexCategoryViewModel
    {
        public string SearchName { get; set; }
        public IEnumerable<GridCategoryViewModel> Grid { get; set; }

        public int TotalHalaman { get; set; }
        public int TotalData { get; set; }
        public int TotalCategory { get; set; }
    }
}
