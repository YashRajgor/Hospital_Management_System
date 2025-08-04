using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AppointmentController : Controller
    {
        public static List<Appointment> appointments = new List<Appointment>();
        ManageAppointment manageAppointment = new ManageAppointment();
        int? userId;
        public IActionResult AddAppointment()
        {
            ViewBag.PatientList = manageAppointment.getPatientName();
            ViewBag.DepartmentList = manageAppointment.getDepartmentList();
            return View();
        }

        [HttpGet]
        public JsonResult GetDoctorsByDepartment(int departmentId)
        {
            var doctors = manageAppointment.getDoctorByDepartment(departmentId)
                .Select(d => new
                {
                    doctorId = d.doctorId,
                    doctorName = d.doctorName
                }).ToList();

            return Json(doctors);
        }


        [HttpPost]
        public IActionResult AddAppointment(Appointment appointment)
        {
            userId = HttpContext.Session.GetInt32("UserId");
            int result = manageAppointment.addAppointment(appointment, (int)userId!);

            if (result > 0)
            {
                TempData["AppointmentMessage"] = "Success";
            }
            else
            {
                TempData["AppointmentMessage"] = "Fail";
            }

            ViewBag.PatientList = manageAppointment.getPatientName();
            ViewBag.DepartmentList = manageAppointment.getDepartmentList();
            return View();
        }

        public IActionResult selectAllAppointment()
        {
            var appointmentData = manageAppointment.selectAllAppointment();
            return View(appointmentData);
        }
    }
}
