using System;
using Basilisk.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Orders
{
    public class GridOrderViewModel
    {
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string SalesName { get; set; }
        public string OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string StringDueDate { get; set; }
        public string StringShippedDate { get; set; }
        public string DesAddress { get; set; }
        public string DesCity { get; set; }
        public string DesPostalCode { get; set; }
        public string DesCost { get; set; }
        public string TotalPembayaran { get; set; }

        public string ProductName { get; set; }
        public decimal Ongkir { get; set; }
        public string DeliveryName { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
