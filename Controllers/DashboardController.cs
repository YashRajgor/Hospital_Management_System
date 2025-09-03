using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        ManageDashboard manageDashboard = new ManageDashboard();
        public IActionResult Dashboard()
        {
            Dashboard model = manageDashboard.GetDashboardCounts();
            return View(model);
        }
    }
}
