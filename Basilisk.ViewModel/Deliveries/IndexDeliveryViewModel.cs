using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Deliveries
{
    public class IndexDeliveryViewModel
    {
        public string SearchName { get; set; }
        public int TotalHalaman { get; set; }
        public int TotalData { get; set; }
        public int TotalDelivery { get; set; }
        public string Judul { get; set; }
        public IEnumerable<GridDeliveryViewModel> GridDelivery { get; set; }
    }
}
