using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        public static List<Dashboard> dashboard=new List<Dashboard>();
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
