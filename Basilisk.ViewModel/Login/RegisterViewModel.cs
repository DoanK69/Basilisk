using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Login
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Username harus diisi")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Username harus diisi")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Username harus diisi")]
        public string? Role { get; set; }

        public List<DropDownListViewModel> DropDownRole { get; set; }


    }
}
