using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AppointmentController : Controller
    {
        ManageAppointment manageAppointment = new ManageAppointment();
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
            return View();
        }
    }
}
