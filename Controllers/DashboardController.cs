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

        [HttpPost]
        public IActionResult Dashboard(Dashboard dashboard)
        {
            try
            {
                // Call filter SP
                dashboard.filteredAppointments = manageDashboard.getFilteredData(dashboard);

                // Also load counts so dashboard stays complete
                Dashboard counts = manageDashboard.GetDashboardCounts();
                dashboard.totalAppointment = counts.totalAppointment;
                dashboard.pendingAppointment = counts.pendingAppointment;
                dashboard.processAppointment = counts.processAppointment;
                dashboard.completedAppointment = counts.completedAppointment;
                dashboard.totalDoctor = counts.totalDoctor;
                dashboard.totalPatient = counts.totalPatient;
                dashboard.totalDepartment = counts.totalDepartment;
                dashboard.totalUser = counts.totalUser;

                return View(dashboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(new Dashboard());
            }
        }
    }
}