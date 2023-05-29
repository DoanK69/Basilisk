using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public long CustomerId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public bool Checked { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
