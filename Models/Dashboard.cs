namespace Hospital_Management_System.Models
{
    public class Dashboard
    {
        public int totalAppointment { get; set; }
        public int pendingAppointment { get; set; }
        public int processAppointment { get; set; }
        public int completedAppointment { get; set; }
        public int totalDoctor { get; set; }
        public int totalDepartment { get; set; }
        public int totalPatient { get; set; }
        public int totalUser { get; set; }
        public string? Name { get; set; }
        public DateTime? fromDate{ get; set; }
        public DateTime? toDate{ get; set; }
        public string? status { get; set; }
        public List<Appointment> latestAppointments { get; set; } = new List<Appointment>();
        public List<Appointment>? filteredAppointments { get; set; }
    }
}
