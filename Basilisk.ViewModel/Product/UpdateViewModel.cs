using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Product
{
    public class UpdateViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? SupplierId { get; set; }
        public long CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int OnOrder {get; set;}
        public bool Discontinue { get; set;}

        public List<SelectListItem> DropDownSupplier { get; set; }
        public List<SelectListItem> DropDownCategory { get; set; }

        public List<DropDownListViewModel> DropDownSupplierCustom { get; set; }
        public List<DropDownListViewModel> DropDownCategoryCustom { get; set; }

    }
}
