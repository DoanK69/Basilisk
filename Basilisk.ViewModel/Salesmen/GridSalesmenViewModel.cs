using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Salesmen
{
    public class GridSalesmenViewModel
    {
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HiredDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }     
        public string? SuperiorEmployeeNumber { get; set; }
    }
}
