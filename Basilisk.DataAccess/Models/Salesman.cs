using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class Salesman
    {
        public Salesman()
        {
            InverseSuperiorEmployeeNumberNavigation = new HashSet<Salesman>();
            Orders = new HashSet<Order>();
            SalesmenRegions = new HashSet<SalesmenRegion>();
        }

        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HiredDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string SuperiorEmployeeNumber { get; set; }

        public virtual Salesman SuperiorEmployeeNumberNavigation { get; set; }
        public virtual ICollection<Salesman> InverseSuperiorEmployeeNumberNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<SalesmenRegion> SalesmenRegions { get; set; }
    }
}
