using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class CompleteProduct
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierCompanyName { get; set; }
        public decimal Price { get; set; }
    }
}
