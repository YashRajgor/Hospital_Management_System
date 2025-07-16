using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Doctor
    {
        public int doctorId {  get; set; }
        [Required(ErrorMessage ="Enter Doctor Name")]
        public string doctorName { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")] 
        public string doctorPhone { get; set; }
        [Required(ErrorMessage = "Enter Email")] 
        public string doctorEmail { get; set; }
        [Required(ErrorMessage = "Enter Specialization")]
        public string doctorSpecialization { get; set; }
        [Required(ErrorMessage = "Enter Qualification")]
        public string doctorQualification { get; set; }
        public int isActive {  get; set; }
        public DateTime created { get; set; }
        public DateTime Modified { get; set; }
        public int usedId {  get; set; }
        public string usedName {  get; set; }
    }
}
