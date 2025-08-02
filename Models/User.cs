namespace Hospital_Management_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? email { get; set; } 
        public string? phoneNumber { get; set; } 
        public int isActive { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
    }
}
