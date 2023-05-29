using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class OrderDetail
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public long ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }

        public virtual Order InvoiceNumberNavigation { get; set; }
        public virtual Product Product { get; set; }
    }
}
