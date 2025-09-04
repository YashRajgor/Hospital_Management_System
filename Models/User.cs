using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required] public string? userName { get; set; }
        [Required]public string? password { get; set; }
        [Required] public string? email { get; set; }

        [Required(ErrorMessage = "Enter Phone number")]
        [Phone]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone Number must be exactly 10 digits and contain only numbers.")] 
        public string? phoneNumber { get; set; }
        public int isActive { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
    }
}
