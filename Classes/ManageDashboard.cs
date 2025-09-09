using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;

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

        public Dashboard GetDashboardCounts(Dashboard? dashboard = null)
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

        public List<Appointment> getFilteredData(Dashboard dashboard)
        {
            List<Appointment> appointments = new List<Appointment>();

            SqlParameter[] parameter = new SqlParameter[]
            {
               new SqlParameter("@name", string.IsNullOrEmpty(dashboard.Name) ? DBNull.Value : dashboard.Name),
               new SqlParameter("@fromDate", dashboard.fromDate == DateTime.MinValue ? DBNull.Value : dashboard.fromDate),
               new SqlParameter("@toDate", dashboard.toDate == DateTime.MinValue ? DBNull.Value : dashboard.toDate),
               new SqlParameter("@status", string.IsNullOrEmpty(dashboard.status) ? DBNull.Value : dashboard.status)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Filter_Data", parameter);

            Appointment appointment;
            while (reader.Read())
            {
                appointment = new Appointment();
                appointment.AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                appointment.AppointmentStatus = reader["AppointmentStatus"].ToString();
                appointment.PatientName = reader["PatientName"].ToString();
                appointment.DoctorName = reader["DoctorName"].ToString();
                appointments.Add(appointment);
            }

            return appointments;
        }
    }
}
