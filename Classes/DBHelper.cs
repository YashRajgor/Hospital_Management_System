using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management_System.Classes
{
    public class DBHelper
    {
        string connectionString = "Data Source=YASH\\SQLEXPRESS;Initial Catalog=\"Hospital Management System\";Integrated Security=True;Encrypt=False";
        private readonly SqlConnection? connection;

        public DBHelper()
        {
            connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            //else
            //{
            //    connection.Close();
            //}
        }

        public SqlDataReader ExecuteReader(string storeProcedure, SqlParameter[]? perameter=null)
        {
            SqlCommand cmd = new SqlCommand(storeProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (perameter != null)
            {
                cmd.Parameters.AddRange(perameter);
            }
            
            return cmd.ExecuteReader();
        }

        public int ExecuteNonQuery(string storeProcedure, SqlParameter[] perameter)
        {
            SqlCommand cmd = new SqlCommand(storeProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (perameter != null)
            {
                cmd.Parameters.AddRange(perameter);
            }

            return cmd.ExecuteNonQuery(); 
        }

        public object ExecuteScalar(string storedProcedure, SqlParameter[]? parameters=null)
        {
            SqlCommand cmd = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd.ExecuteScalar();
        }
    }
}
