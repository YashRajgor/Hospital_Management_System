using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Classes
{
    public class ManagePatient
    {
        DBHelper dbHelper;

        public ManagePatient()
        {
            dbHelper = new DBHelper();
        }
        public int addPatient(Patient patient, int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@Name",patient.PatientName),
                new SqlParameter("@dateOfBirth",patient.dateOfBirth),
                new SqlParameter("@gender",patient.gender),
                new SqlParameter("@email",patient.email),
                new SqlParameter("@phone",patient.phoneNumber),
                new SqlParameter("@address",patient.address),
                new SqlParameter("@city",patient.city),
                new SqlParameter("@state",patient.state),
                new SqlParameter("@image", (object?)patient.PatientImage ?? DBNull.Value),
                new SqlParameter("@userid",userId),
            };
            return dbHelper.ExecuteNonQuery("SP_Add_Patient", parameter);
        }

        public int checkPatient(string name)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@name",name)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_Patient", parameter);

            if (reader.HasRows)
            {
                reader.Close();
                return 1;
            }
            reader.Close();
            return 0;
        }

        public List<Patient> select_All_Patient()
        {
            PatientController.Patients.Clear();
            SqlDataReader reader = dbHelper.ExecuteReader("SP_Display_Patient");
            Patient? patient = null;

            while (reader.Read())
            {
                patient = new Patient();
                patient.patientId = Convert.ToInt32(reader["PatientId"]);
                patient.PatientName = reader["Name"].ToString();
                patient.dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                patient.gender = reader["Gender"].ToString();
                patient.email = reader["Email"].ToString();
                patient.phoneNumber = reader["Phone"].ToString();
                patient.address = reader["Address"].ToString();
                patient.city = reader["City"].ToString();
                patient.state = reader["State"].ToString();
                patient.isActive = Convert.ToInt32(reader["IsActive"]);
                patient.created = Convert.ToDateTime(reader["Created"]);
                patient.modified = Convert.ToDateTime(reader["Modified"]);
                patient.userName = reader["UserName"].ToString();
                patient.PatientImage = reader["PatientImage"].ToString();
                PatientController.Patients.Add(patient);
            }
            return PatientController.Patients;
        }

        public int deletePatient(int patientId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@PatientId",patientId)
            };

            return dbHelper.ExecuteNonQuery("SP_Delete_Patient", parameter);
        }

        public int checkBeforePatientUpdate(Patient patient)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@patientId",patient.patientId),
                new SqlParameter("@patientName",patient.PatientName)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_Before_Update", parameter);

            if (reader.HasRows)
            {
                reader.Close();
                return 1;
            }

            reader.Close();
            return 0;
        }

        public int editPatient(Patient patient, int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@patientId",patient.patientId),
                new SqlParameter("@name",patient.PatientName),
                new SqlParameter("@dateOfBirth",patient.dateOfBirth),
                new SqlParameter("@gender",patient.gender),
                new SqlParameter("@email",patient.email),
                new SqlParameter("@phone",patient.phoneNumber),
                new SqlParameter("@address",patient.address),
                new SqlParameter("@city",patient.city),
                new SqlParameter("@state",patient.state),
                new SqlParameter("@isActive",patient.isActive),
                new SqlParameter("@image", (object?)patient.PatientImage ?? DBNull.Value),
                new SqlParameter("@userId",userId),
            };
            return dbHelper.ExecuteNonQuery("SP_Edit_Patient", parameter);
        }
    }
}
