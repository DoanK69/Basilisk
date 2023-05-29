using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basilisk.ViewModel.Product
{
    public class GridProductViewModel
    {
        //        id
        //product name
        //supplier name
        //category name
        //description
        //price(format)
        //stok
        //on order
        //discontinued(kata kata string)
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
        public int OnOrder { get; set; }
        public string Discontinue { get; set; }


        
    }
}
