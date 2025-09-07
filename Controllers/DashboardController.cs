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
            try
            {
                Dashboard model = manageDashboard.GetDashboardCounts();
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(new Dashboard());
            }
        }
    }
}
