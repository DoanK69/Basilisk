using Basilisk.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Supplier
{
    public class DetailSupplierViewModel
    {
        public GridSupplierViewModel? Supplier { get; set; }
        public IEnumerable<GridProductViewModel> GridProducts { get; set; }
    }
}
