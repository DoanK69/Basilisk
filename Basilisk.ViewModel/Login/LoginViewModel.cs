using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username harus diisi")]
        [StringLength(20, ErrorMessage = "Max. 20 karakter")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        [StringLength(20, ErrorMessage = "Max. 20 karakter")]
        public string? Password { get; set; }
    }
}
