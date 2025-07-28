using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management_System.Classes
{
    public class ManageDepartment
    {
        DBHelper dbHelper;
        //SqlConnection? connection = null;
        public ManageDepartment()
        {
            dbHelper = new DBHelper();
        }

        public int Add_Department(Department department, DateTime Modified, int userid)
        {
            int result = check_Department(department.DepartmentName);
            if (result > 0)
            {
                return 2;
            }
            else
            {
                SqlParameter[] peramater = new SqlParameter[]
                {
                new SqlParameter("@DepartName", department.DepartmentName),
                new SqlParameter("@description", department.DepartmentDescription),
                new SqlParameter("@Modified", Modified),
                new SqlParameter("@UserId", userid)
                };

                result = dbHelper.ExecuteNonQuery("SP_Add_Department", peramater);

                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            //string query = "SP_Add_Department";
            //SqlCommand sqlCommand = new SqlCommand(query, connection);
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            //sqlCommand.Parameters.Add(new SqlParameter("@DepartName", Department));
            //sqlCommand.Parameters.Add(new SqlParameter("@description", Description));
            //sqlCommand.Parameters.Add(new SqlParameter("@Modified", Modified));
            //sqlCommand.Parameters.Add(new SqlParameter("@UserId", userid));
            //if (connection.State != ConnectionState.Open)
            //{
            //    connection.Open();
            //}
            //int result = sqlCommand.ExecuteNonQuery();
            //return result;
        }

        public int check_Department(string departmentName)
        {
            SqlParameter[] perameter = new SqlParameter[]
            {
                new SqlParameter("@DepartmentName",departmentName)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_Department", perameter);

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

        public List<Department> getAllDepartment()
        {
            AdminController.departmentList.Clear();
            SqlDataReader reader = dbHelper.ExecuteReader("SP_Display_Department");
            Department? department = null;
            while (reader.Read())
            {
                department = new Department();
                department.departmentId = Convert.ToInt32(reader["DepartmentId"]);
                department.DepartmentName = reader["DepartmentName"].ToString() ?? "";
                department.DepartmentDescription = reader["Description"].ToString() ?? "";
                department.IsActive = Convert.ToInt32(reader["IsActive"]);
                department.Created = Convert.ToDateTime(reader["Created"]);
                department.Modified = Convert.ToDateTime(reader["Modified"]);
                department.userName = reader["UserName"].ToString()??"";
                AdminController.departmentList.Add(department);
            }


            return AdminController.departmentList;
        }

        public int DeleteDepartment(int id)
        {
            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentId",id)
            };
            int result = dbHelper.ExecuteNonQuery("SP_Delete_Department", Parameters);
            return result;
        }

        public int check_Department_Before_update(Department department)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@DepartmentId",department.departmentId),
                new SqlParameter("@DepartmentName",department.DepartmentName)
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_Department_Before_Update", parameter);

            if (reader.HasRows)
            {
                reader.Close();
                return 1;
            }
            reader.Close();
            return 0;
        }

        public int update_Department(Department department, int userid)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@DepartmentId",department.departmentId),
                new SqlParameter("@DepartmentName",department.DepartmentName),
                new SqlParameter("@Description",department.DepartmentDescription),
                new SqlParameter("@IsActive",department.IsActive),
                new SqlParameter("@UserId",userid)
            };

            int result = dbHelper.ExecuteNonQuery("SP_Edit_Department", parameter);

            return result;
        }

    }
}
