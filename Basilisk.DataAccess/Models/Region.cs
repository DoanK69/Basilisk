using System;
using System.Collections.Generic;

#nullable disable

namespace Basilisk.DataAccess.Models
{
    public partial class Region
    {
        public Region()
        {
            SalesmenRegions = new HashSet<SalesmenRegion>();
        }

        public long Id { get; set; }
        public string City { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<SalesmenRegion> SalesmenRegions { get; set; }
    }
}
