using Microsoft.Data.SqlClient;
using System.Data;
namespace Hospital_Management_System.Classes
{
    public class LoginMethod
    {

        DBHelper dbHelper;
        public LoginMethod()
        {
            dbHelper=new DBHelper();
        }
        public SqlDataReader Check_Login(string input, string password)
        {

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@input", input),
                new SqlParameter("@password", password)
            };

            return dbHelper.ExecuteReader("Admin_Login_SP",parameters);

            //string query = "Admin_Login_SP";
            //SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            //sqlCommand.Parameters.Add(new SqlParameter("@input", input));
            //sqlCommand.Parameters.Add(new SqlParameter("@password", password));   
            //if (sqlConnection.State!=ConnectionState.Open)
            //{
            //    sqlConnection.Open();
            //}

            //SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            //return sqlDataReader;
        }
    }
}
