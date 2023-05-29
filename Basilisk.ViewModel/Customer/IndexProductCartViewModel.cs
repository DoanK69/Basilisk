using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class IndexProductCartViewModel
    {
        public long CustomerId { get; set; }
        public List<ProductCartViewModel>? ProductCart { get; set; }
    }
}
