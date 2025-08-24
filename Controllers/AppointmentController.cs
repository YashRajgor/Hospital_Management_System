using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AppointmentController : Controller
    {
        public static List<Appointment> appointments = new List<Appointment>();
        List<string> status = new List<string> { "pending", "process", "complete" };
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

            bool exists = appointments.Any(a =>
                a.DoctorId == appointment.DoctorId &&
                a.DepartmentId == appointment.DepartmentId &&
                a.AppointmentDate == appointment.AppointmentDate);

            if (exists)
            {
                TempData["AppointmentMessage"] = "Exists";
            }
            else
            {
                int result = manageAppointment.addAppointment(appointment, (int)userId!);

                if (result > 0)
                {
                    appointments.Add(appointment);
                    TempData["AppointmentMessage"] = "Success";
                }
                else
                {
                    TempData["AppointmentMessage"] = "Fail";
                }
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

        public IActionResult EditAppointment(int appointmentId)
        {
            Appointment? appointment = appointments.Find(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                return RedirectToAction("selectAllAppointment", "Appointment");
            }

            ViewBag.PatientList = manageAppointment.getPatientName();
            ViewBag.DepartmentList = manageAppointment.getDepartmentList();
            ViewBag.DoctorList = manageAppointment.getDoctorByDepartment(appointment.DepartmentId);

            ViewBag.StatusList = status;
            return View(appointment);
        }

        [HttpPost]
        public IActionResult EditAppointment(Appointment appointment)
        {
            userId = HttpContext.Session.GetInt32("UserId");

            bool exists = appointments.Any(a =>
               a.DoctorId == appointment.DoctorId &&
               a.DepartmentId == appointment.DepartmentId &&
               a.AppointmentDate == appointment.AppointmentDate &&
               a.AppointmentId!=appointment.AppointmentId);

            if (exists)
            {
                TempData["AppointmentMessage"] = "Exists";
            }
            else
            {
                int result = manageAppointment.updateAppointment(appointment, (int)userId!);

                if(result>0)
                {
                    return RedirectToAction("selectAllAppointment");
                }
                else
                {
                    TempData["AppointmentMessage"] = "Fail";
                }
            }

            ViewBag.PatientList = manageAppointment.getPatientName();
            ViewBag.DepartmentList = manageAppointment.getDepartmentList();
            ViewBag.DoctorList = manageAppointment.getDoctorByDepartment(appointment.DepartmentId);
            ViewBag.StatusList = status;

            return View(appointment);
        }
    }
}
