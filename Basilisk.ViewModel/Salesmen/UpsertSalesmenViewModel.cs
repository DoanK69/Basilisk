using Basilisk.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Salesmen
{
    public class UpsertSalesmenViewModel
    {
        [Required()]
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Level { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HiredDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        [ValidationPhone(ErrorMessage = "Nomor handphone min: 11 and max: 13")]
        public string? Phone { get; set; }
        public string? SuperiorEmployeeNumber { get; set; }
        public List<DropDownListViewModel>? DropDownSuperiorEmployeeNumber { get; set; }
    }
}
