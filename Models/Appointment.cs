namespace Hospital_Management_System.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? AppointmentStatus { get; set; }
        public string? Description { get; set; }
        public string? SpecialRemarks { get; set; }
        public decimal TotalConsultedAmount {  get; set; }
    }
}
