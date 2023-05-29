using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class SalesmenRegion
    {
        public string SalesmanEmployeeNumber { get; set; }
        public long RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual Salesman SalesmanEmployeeNumberNavigation { get; set; }
    }
}
