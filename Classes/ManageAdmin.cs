using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital_Management_System.Classes
{
    public class ManageAdmin
    {
        private DBHelper dbHelper;
        private static Dictionary<string, OTPDetails> otpStore = new Dictionary<string, OTPDetails>();

        public ManageAdmin()
        {
            dbHelper = new DBHelper();
        }

        public SqlDataReader Check_Login(string input, string password)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@input", input),
                new SqlParameter("@password", password)
            };

            return dbHelper.ExecuteReader("Admin_Login_SP", parameters);
        }

        public int GenerateOTP(string email)
        {
            Random rnd = new Random();
            int otp = rnd.Next(100000, 999999);

            otpStore[email] = new OTPDetails
            {
                OTP = otp,
                GeneratedAt = DateTime.Now
            };

            return otp;
        }

        public bool VerifyOTP(string email, int userOTP)
        {
            CleanupExpiredOTPs(); 

            if (otpStore.ContainsKey(email))
            {
                OTPDetails otpDetails = otpStore[email];

                if (otpDetails.OTP == userOTP)
                {
                    otpStore.Remove(email); 
                    return true;
                }
            }
            return false; 
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", newPassword)
            };

            int result = dbHelper.ExecuteNonQuery("SP_Update_Admin_Password", parameters);
            return result > 0;
        }

        private void CleanupExpiredOTPs()
        {
            var expiredKeys = otpStore
                .Where(kvp => (DateTime.Now - kvp.Value.GeneratedAt).TotalMinutes > 5)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
                otpStore.Remove(key);
        }

        private class OTPDetails
        {
            public int OTP { get; set; }
            public DateTime GeneratedAt { get; set; }
        }
    }
}
