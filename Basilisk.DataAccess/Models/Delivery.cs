using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class Delivery
    {
        public Delivery()
        {
            Orders = new HashSet<Order>();
        }

        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public decimal Cost { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
