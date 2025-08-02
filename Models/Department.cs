using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

namespace Hospital_Management_System.Models
{
    public class Department
    {
        public int departmentId { get; set; }
        [Required(ErrorMessage = "Please Enter Department Name")] public string? DepartmentName { get; set; }
        [Required(ErrorMessage = "Please Enter Description")] public string? DepartmentDescription { get; set; }
        public int IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string? userName { get; set; }
    }
}
