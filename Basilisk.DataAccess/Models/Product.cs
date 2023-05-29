using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? SupplierId { get; set; }
        public long CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int OnOrder { get; set; }
        public bool Discontinue { get; set; }
        public string ImagePath { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
