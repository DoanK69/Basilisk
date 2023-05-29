using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Category
{
    public class CreateUpdateViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "*Harus diisi")]
        [MinLength(3, ErrorMessage = "*Min 3 character")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
