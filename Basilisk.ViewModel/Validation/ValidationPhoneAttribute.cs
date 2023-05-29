using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.ViewModel.Validation
{
    public class ValidationPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null)
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    var cek = value.ToString().Length <= 13 && value.ToString().Length >= 11;
                    if (!cek)
                    {
                        return new ValidationResult(ErrorMessage);
                    }

                }
            }          
            return ValidationResult.Success;
        }
    }
}
