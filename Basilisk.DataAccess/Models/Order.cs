using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string InvoiceNumber { get; set; }
        public long CustomerId { get; set; }
        public string SalesEmployeeNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public long DeliveryId { get; set; }
        public decimal DeliveryCost { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationPostalCode { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual Salesman SalesEmployeeNumberNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
