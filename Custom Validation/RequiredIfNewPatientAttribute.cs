using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Custom_Validation
{
    public class RequiredIfNewPatientAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var patient = (Patient)validationContext.ObjectInstance;

            if (patient.IsNew)
            {
                if (value is IFormFile file && file.Length > 0)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage ?? "Please upload an image");
            }

            return ValidationResult.Success;
        }
    }
}
