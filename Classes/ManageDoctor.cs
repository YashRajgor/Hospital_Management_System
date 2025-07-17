using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Collections.Specialized;

namespace Hospital_Management_System.Classes
{
    public class ManageDoctor
    {
        DBHelper dbHelper;
        //SqlConnection? connection = null;
        public ManageDoctor()
        {
            dbHelper = new DBHelper();
        }

        public int addDoctor(Doctor doctor, int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@Name",doctor.doctorName),
                new SqlParameter("@Phone",doctor.doctorPhone),
                new SqlParameter("@Email",doctor.doctorEmail),
                new SqlParameter("@Qualification",doctor.doctorQualification),
                new SqlParameter("@Specialization",doctor.doctorSpecialization),
                new SqlParameter("@UserId",userId)
            };

            return dbHelper.ExecuteNonQuery("SP_Add_Doctor", parameter);
        }

        public int check_Befor_Insert(string name)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@Name",name)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Select_Doctor_Name", parameter);

            if (reader.HasRows)
            {
                reader.Close();
                return 1;
            }
            else
            {
                reader.Close();
                return 0;
            }
        }

        public List<Doctor> select_All_Doctor()
        {
            DoctorController.doctorsList.Clear();
            SqlDataReader reader = dbHelper.ExecuteReader("SP_Select_All_Doctor");
            Doctor? doctor = null;

            while (reader.Read())
            {
                doctor = new Doctor();
                doctor.doctorId = Convert.ToInt32(reader["DoctorId"]);
                doctor.doctorName = reader["Name"].ToString();
                doctor.doctorPhone = reader["Phone"].ToString();
                doctor.doctorEmail = reader["Email"].ToString();
                doctor.doctorQualification = reader["Qualification"].ToString();
                doctor.doctorSpecialization = reader["Specialization"].ToString();
                doctor.isActive = Convert.ToInt32(reader["IsActive"]);
                doctor.created = Convert.ToDateTime(reader["Created"]);
                doctor.Modified = Convert.ToDateTime(reader["Modified"]);
                doctor.usedName = reader["UserName"].ToString();
                DoctorController.doctorsList.Add(doctor);
            }

            return DoctorController.doctorsList;
        }

        public int check_Doctor_Before_Update(int id, string name)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@DoctorId",id),
                new SqlParameter("@Name",name)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_Doctor_Before_Update", parameter);

            if (reader.HasRows)
            {
                reader.Close();
                return 1;
            }

            reader.Close();
            return 0;

        }

        public int update_Doctor(Doctor doctor, int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@doctorid",doctor.doctorId),
                new SqlParameter("@name",doctor.doctorName),
                new SqlParameter("@phone",doctor.doctorPhone),
                new SqlParameter("@email",doctor.doctorEmail),
                new SqlParameter("@qualification",doctor.doctorQualification),
                new SqlParameter("@specialization",doctor.doctorSpecialization),
                new SqlParameter("@isActive",doctor.isActive),
                new SqlParameter("@id",userId)
            };

            return dbHelper.ExecuteNonQuery("SP_Doctor_Update", parameter);
        }

        public int delete_Doctor(int doctorid)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@DoctorId",doctorid)
            };

            return dbHelper.ExecuteNonQuery("SP_Delete_Doctor", parameter);
        }

        #region MyRegion
        public List<SelectListItem> getDepartmentName()
        {
            List<SelectListItem> departmentList = new List<SelectListItem>();
            SqlDataReader reader = dbHelper.ExecuteReader("SP_Display_Department_And_Id");

            while (reader.Read())
            {
                departmentList.Add(new SelectListItem
                {
                    Value = reader["DepartmentId"].ToString(),
                    Text = reader["DepartmentName"].ToString()
                });
            }
            return departmentList;
        } 
        #endregion

        public int getDoctorId(string doctorName)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@name",doctorName)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Get_Doctor_Id",parameter);

            if(reader.Read())
            {
                int id =Convert.ToInt32(reader["DoctorId"]);
                reader.Close();
                return id;
            }
            reader.Close();
            return 0;
        }

        public int addDoctorDepartment(int doctorId,int departmentId,int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@doctorId",doctorId),
                new SqlParameter("@departmentId",departmentId),
                new SqlParameter("@userId",userId)
            };

            return dbHelper.ExecuteNonQuery("SP_Add_DoctorDepartment",parameter);
        }
    }
}
