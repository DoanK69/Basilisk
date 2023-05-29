using Basilisk.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Customer
{
    public class GridCustomerViewModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int TotalProduct { get; set; }
    }
}
