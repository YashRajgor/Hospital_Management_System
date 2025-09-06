using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Admin
    {
        [Required]
        public string? userName { get; set; }

        [Required]
        public string? password { get; set; }

        [Required]
        public string? email { get; set; }

        [Required]
        public int OTP { get; set; }
        
        [Required]
        public string? newPassword { get; set; }

        [Required]
        [Compare(nameof(newPassword), ErrorMessage = "Passwords do not match")]
        public string? confirmNewPassword { get; set; }
    }
}
