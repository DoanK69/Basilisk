using Basilisk.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Utility
{
    public class GlobalConfiguration
    {
        public static IEnumerable<MenuViewModel> Menus()
        {
            var data = new List<MenuViewModel>
            {
                new MenuViewModel { Role = "Administrator", Controller = "Account", Action ="Index", Title = "Account" },
                new MenuViewModel { Role = "Administrator", Controller = "Category", Action ="Index", Title = "Category" },
                new MenuViewModel { Role = "Administrator", Controller = "Supplier", Action ="Index", Title = "Supplier" },
                new MenuViewModel { Role = "Administrator", Controller = "Product", Action ="Index", Title = "Product" },
                new MenuViewModel { Role = "Administrator", Controller = "Salesmen", Action ="Index", Title = "Salesman" },
                new MenuViewModel { Role = "Administrator", Controller = "Region", Action ="Index", Title = "Region" },
                new MenuViewModel { Role = "Administrator", Controller = "Order", Action ="Index", Title = "Order" },
                new MenuViewModel { Role = "Administrator", Controller = "Shipper", Action ="Index", Title = "Shipper" },
                new MenuViewModel { Role = "Administrator", Controller = "Customer", Action ="Index", Title = "Customer" },

                //new MenuViewModel { Role = "Salesman", Controller = "Home", Action ="Index", Title = "Home" },
                new MenuViewModel { Role = "Salesman", Controller = "Customer", Action ="DetailSalesmenOrder", Title = "Daftar Order" },
                new MenuViewModel { Role = "Salesman", Controller = "Product", Action ="Index", Title = "Product" },

                //new MenuViewModel { Role = "Customer", Controller = "Home", Action ="Index", Title = "Home" },
                new MenuViewModel { Role = "Customer", Controller = "Customer", Action ="Cart", Title = "Cart" },
                new MenuViewModel { Role = "Customer", Controller = "Customer", Action ="DetailCustomerOrder", Title = "Pesanan" }
            };

            return data;
        }
    }
}
