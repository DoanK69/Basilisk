using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class DetailCheckOutViewModel
    {
        public long CustomerId { get; set; }
        public long ProductId { get; set; }
        public string SalesEmployeeNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DueDate { get; set; }
        public long DeliveryId { get; set; }
        public string DeliveryCost { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationPostalCode { get; set; }
        public string CompanyName { get; set; }
        public decimal TotalHarga { get; set; }
        public string StringTotalHarga { get; set; }

        public List<DetailCartViewModel> CartList { get; set; }
        public List<DropDownListViewModel> DropDownDelivery { get; set; }
    }
}
