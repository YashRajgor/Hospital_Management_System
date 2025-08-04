using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Classes
{
    public class ManageAppointment
    {
        List<Patient> patientList = new List<Patient>();
        List<Department> departmenttList = new List<Department>();
        List<Doctor> doctortList = new List<Doctor>();

        DBHelper dbHelper;
        public ManageAppointment()
        {
            dbHelper = new DBHelper();
        }

        public int addAppointment(Appointment appointment, int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@departmentId",appointment.DepartmentId),
                new SqlParameter("@doctorId",appointment.DoctorId),
                new SqlParameter("@patientId",appointment.PatientId),
                new SqlParameter("@appointmentDate",appointment.AppointmentDate),
                new SqlParameter("@description",appointment.Description),
                new SqlParameter("@specialRemark",appointment.SpecialRemarks),
                new SqlParameter("@userId",userId)
            };

            return dbHelper.ExecuteNonQuery("SP_Add_Appointment", parameter);
        }

        public List<Patient> getPatientName()
        {
            patientList.Clear();

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Get_Patient_Name_And_Id");

            Patient? patient;
            while (reader.Read())
            {
                patient = new Patient();
                patient.patientId = Convert.ToInt32(reader["PatientId"]);
                patient.PatientName = reader["Name"].ToString() ?? "";
                patientList.Add(patient);
            }

            reader.Close();
            return patientList;
        }

        public List<Department> getDepartmentList()
        {
            departmenttList.Clear();

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Get_Department_Name_And_Id");

            Department? department;
            while (reader.Read())
            {
                department = new Department();
                department.departmentId = Convert.ToInt32(reader["DepartmentId"]);
                department.DepartmentName = reader["DepartmentName"].ToString() ?? "";
                departmenttList.Add(department);
            }

            reader.Close();
            return departmenttList;
        }

        public List<Doctor> getDoctorByDepartment(int departmentId)
        {
            doctortList.Clear();
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@departmentId",departmentId)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Get_Doctor_Name_And_Id", parameter);

            Doctor? doctor;
            while (reader.Read())
            {
                doctor = new Doctor();
                doctor.doctorId = Convert.ToInt32(reader["DoctorId"]);
                doctor.doctorName = reader["Name"].ToString() ?? "";
                doctortList.Add(doctor);
            }

            reader.Close();
            return doctortList;
        }

        public List<Appointment> selectAllAppointment()
        {
            AppointmentController.appointments.Clear();

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Select_All_Appointment");

            Appointment? appointment;

            while (reader.Read())
            {
                appointment = new Appointment();
                appointment.AppointmentId = Convert.ToInt32(reader["AppointmentId"]);
                appointment.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                appointment.DepartmentName = reader["DepartmentName"].ToString();
                appointment.DoctorId = Convert.ToInt32(reader["DoctorId"]);
                appointment.DoctorName = reader["Name"].ToString();
                appointment.PatientId = Convert.ToInt32(reader["PatientId"]);
                appointment.PatientName = reader["Name"].ToString();
                appointment.AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                appointment.AppointmentStatus = reader["AppointmentStatus"].ToString();
                appointment.Description = reader["Description"].ToString();
                appointment.SpecialRemarks = reader["SpecialRemarks"].ToString();
                appointment.Created = Convert.ToDateTime(reader["Created"]);
                appointment.Modified = Convert.ToDateTime(reader["Modified"]);
                appointment.userName = reader["UserName"].ToString();
                appointment.TotalConsultedAmount = Convert.ToDecimal(reader["TotalConsultedAmount"]);
            }

            reader.Close();
            return AppointmentController.appointments;
        }
    }
}
