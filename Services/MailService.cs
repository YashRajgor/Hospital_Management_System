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



    }
}
