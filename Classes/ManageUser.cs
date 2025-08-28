using Hospital_Management_System.Controllers;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Classes
{
    public class ManageUser
    {
        DBHelper dbHelper;
        MailService mailService;

        public ManageUser()
        {
            dbHelper = new DBHelper();
            mailService = new MailService();
        }
        public int addUser(User user)
        {
            string randomPassword = mailService.generatePassword();
            user.password = randomPassword;

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@userName",user.userName),
                new SqlParameter("@password",user.password),
                new SqlParameter("@email",user.email),
                new SqlParameter("@number",user.phoneNumber),
            };

            int result=dbHelper.ExecuteNonQuery("SP_Add_User", parameter);

            if (result > 0)
            {
                string subject = "Welcome to Hospital Management System";
                string body = $"<h3>Hello {user.userName},</h3>" +
                              $"<p>Your account has been created successfully.</p>" +
                              $"<p><b>Username:</b> {user.userName}</p>" +
                              $"<p><b>Password:</b> {randomPassword}</p>" +
                              $"<p>Please login and change your password.</p>";

                mailService.SendMail(user.email!, subject, body);
            }

            return result;

        }

        public int checkUserExitsOrNot(User user)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@userName",user.userName),
                new SqlParameter("@mobileNumber",user.phoneNumber),
                new SqlParameter("@email",user.email),
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_User", parameter);

            if(reader.HasRows)
            {
                reader.Close();
                return 1;
            }

            reader.Close();
            return 0;
        }

        public List<User> getUserList()
        {
            User? user;
            UserController.userList.Clear();
            SqlDataReader reader = dbHelper.ExecuteReader("SP_Select_All_User");

            while(reader.Read())
            {
                user=new User();
                user.UserId = Convert.ToInt32(reader["UserId"]);
                user.userName = reader["UserName"].ToString()??"";
                user.password = reader["password"].ToString() ??"";
                user.email = reader["Email"].ToString() ?? "";
                user.phoneNumber = reader["MobileNo"].ToString() ?? "";
                user.isActive = Convert.ToInt32(reader["IsActive"]);
                user.createdDate = Convert.ToDateTime(reader["Created"]);
                user.modifiedDate = Convert.ToDateTime(reader["Modified"]);
                UserController.userList.Add(user);
            }

            return UserController.userList;
        }

        public int deleteUser(int userId)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };

            return dbHelper.ExecuteNonQuery("SP_Delete_User",parameter);
        }

        public int checkUserBeforeUpdate(User user)
        {

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@UserId",user.UserId),
                new SqlParameter("@userName",user.userName),
                new SqlParameter("@mobileNumber",user.phoneNumber),
                new SqlParameter("@email",user.email),
            };

            SqlDataReader reader = dbHelper.ExecuteReader("SP_Check_User", parameter);

            if(reader.HasRows)
            {
                reader.Close();
                return 1;
            }

            reader.Close();
            return 0;
        }

        public int updateUser(User user)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@userId",user.UserId),
                new SqlParameter("@userName",user.userName),
                new SqlParameter("@password",user.password),
                new SqlParameter("@email",user.email),
                new SqlParameter("@mobileno",user.phoneNumber),
                new SqlParameter("@isActive",user.isActive),
            };

            return dbHelper.ExecuteNonQuery("SP_Edit_User", parameter);
        }

    }
}
