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
    }
}
