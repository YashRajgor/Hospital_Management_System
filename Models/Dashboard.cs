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
    }
}
