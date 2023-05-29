using Basilisk.ViewModel.Region;
using Basilisk.ViewModel.Salesmen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.SalesmanRegions
{
    public class GridSalesmanRegionsViewModel
    {
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }
        public long RegionId { get; set; }
        public string RegionCity { get; set; }
        public string RegionRemark { get; set; }
    }
}
