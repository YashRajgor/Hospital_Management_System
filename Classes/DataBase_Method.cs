using Microsoft.Data.SqlClient;
using System.Data;
namespace Hospital_Management_System.Classes
{
    public class DataBase_Method
    {
        string con = "Data Source=YASH\\SQLEXPRESS;Initial Catalog=\"Hospital Management System\";Integrated Security=True;Encrypt=False";

        SqlConnection? sqlConnection = null;
        public DataBase_Method()
        {
            sqlConnection = new SqlConnection(con);
        }
        public SqlDataReader Check_Login(string input, string password)
        {
            string query = "Admin_Login_SP";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(new SqlParameter("@input", input));
            sqlCommand.Parameters.Add(new SqlParameter("@password", password));   
            if (sqlConnection.State!=ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            return sqlDataReader;
        }
    }
}
