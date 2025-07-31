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

        //public int addAppointment(Appointment appointment)
        //{
        //    SqlParameter[] parameter = new SqlParameter[]
        //    {
        //        new SqlParameter()
        //    };
        //}

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
    }
}
