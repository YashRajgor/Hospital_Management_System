using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;

namespace Hospital_Management_System.Classes
{
    public class ManageDashboard
    {
        DBHelper dbHelper;
        ManageAppointment manageAppointment;
        public ManageDashboard()
        {
            dbHelper = new DBHelper();
            manageAppointment = new ManageAppointment();
        }

        public Dashboard GetDashboardCounts()
        {
            Dashboard model = new Dashboard();

            model.totalAppointment = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Appointment"));
            model.pendingAppointment = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Pending_Appointment"));
            model.processAppointment = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Process_Appointment"));
            model.completedAppointment = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Completed_Appointment"));
            model.totalDoctor = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Doctor"));
            model.totalPatient = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Patient"));
            model.totalDepartment = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_Departments"));
            model.totalUser = Convert.ToInt32(dbHelper.ExecuteScalar("SP_Count_User"));

            model.latestAppointments = manageAppointment.selectAllAppointment()
                .OrderByDescending(a => a.AppointmentId)
                .ToList();

            return model;
        }
    }
}
