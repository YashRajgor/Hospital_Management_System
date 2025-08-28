using System.Net;
using System.Net.Mail;

namespace Hospital_Management_System.Services
{
    public class MailService
    {
        public string generatePassword()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            Random rnd = new Random();
            char[] password = new char[8]; 

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = chars[rnd.Next(chars.Length)];
            }

            return new string(password);
        }

        public bool SendMail(string toEmail, string subject, string body)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)  
                {
                    Credentials = new NetworkCredential("mahetayash8@gmail.com", "twsl vkzh xwvz wosh"),
                    EnableSsl = true
                };

                MailMessage message = new MailMessage();
                message.From = new MailAddress("mahetayash8@gmail.com", "Yash Maheta");
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true; 

                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
