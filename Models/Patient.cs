using Hospital_Management_System.Custom_Validation;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Patient
    {

        public int patientId { get; set; }
        [Required(ErrorMessage = "Enter Patient Name")] public string? PatientName { get; set; }
        [Required(ErrorMessage = "Enter Patient DateOfBirth")] public DateTime dateOfBirth { get; set; }
        [Required(ErrorMessage = "Select Gender")] public string? gender { get; set; }
        [Required(ErrorMessage = "Enter Patient Email")] public string? email { get; set; }

        [Required(ErrorMessage = "Enter Phone number")]
        [Phone]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone Number must be exactly 10 digits and contain only numbers.")] 
        public string? phoneNumber { get; set; }
        [Required(ErrorMessage = "Enter Patient Addresss")] public string? address { get; set; }
        [Required(ErrorMessage = "Enter Patient City")] public string? city { get; set; }
        [Required(ErrorMessage = "Enter Patient State")] public string? state { get; set; }
        public int isActive { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string? userName {  get; set; }
        public string? PatientImage { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool IsNew { get; set; }
    }
}
