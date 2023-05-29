using Basilisk.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Region
{
    public class UpsertRegionViewModel
    {
        //Display Name berfungsi untuk Labeling
        public long Id { get; set; }

        //[StringLength(maximumLength:10, ErrorMessage = "Max 10")]
        [DisplayName("City Name")]
        [Required(ErrorMessage = "*City harus diisi")]
        [UniqueCityName(ErrorMessage ="*City sudah ada")]
        public string City { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}
