using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Doctor
    {
        public int doctorId {  get; set; }
        [Required(ErrorMessage ="Enter Doctor Name")]
        public string? doctorName { get; set; }

        [Required(ErrorMessage = "Enter Phone number")]
        [Phone]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone Number must be exactly 10 digits and contain only numbers.")]
        public string? doctorPhone { get; set; }
        [Required(ErrorMessage = "Enter Email")] 
        public string? doctorEmail { get; set; }
        [Required(ErrorMessage = "Enter Specialization")]
        public string? doctorSpecialization { get; set; }
        [Required(ErrorMessage = "Enter Qualification")]
        public string? doctorQualification { get; set; }
        public int isActive {  get; set; }
        public DateTime created { get; set; }
        public DateTime Modified { get; set; }
        public int usedId {  get; set; }
        public string? usedName {  get; set; }

        public List<Department> doctorDepartments { get; set; } = new List<Department>();
    }
}
