using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Product
{
    public class IndexProductViewModel
    {
        public string SearchProduct { get; set; }
        public string SearchCategory { get; set; }
        public string SearchSupplier { get; set; }
        public IEnumerable<GridProductViewModel> GridProduct { get; set; }
        public int TotalKeseluruhan { get; set; }
        public int TotalData { get; set; }
        public int TotalHalaman { get; set; }
    }
}
