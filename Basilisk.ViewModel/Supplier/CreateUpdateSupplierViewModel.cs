using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Supplier
{
    public class CreateUpdateSupplierViewModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CurrentPage { get; set; }
    }
}
