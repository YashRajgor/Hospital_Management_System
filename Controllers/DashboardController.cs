using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
