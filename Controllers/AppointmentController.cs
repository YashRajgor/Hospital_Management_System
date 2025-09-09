using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AppointmentController : Controller
    {
        public static List<Appointment> appointments = new List<Appointment>();
        List<string> status = new List<string> { "pending", "process", "complete" };
        ManageAppointment manageAppointment = new ManageAppointment();
        MailService mailService = new MailService();
        int? userId;

        public IActionResult AddAppointment()
        {
            try
            {
                ViewBag.PatientList = manageAppointment.getPatientName();
                ViewBag.DepartmentList = manageAppointment.getDepartmentList();
                return View();
            }
            catch (Exception ex)
            {
                TempData["AppointmentMessage"] = "error";
                Console.WriteLine(ex);
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetDoctorsByDepartment(int departmentId)
        {
            try
            {
                var doctors = manageAppointment.getDoctorByDepartment(departmentId)
                    .Select(d => new
                    {
                        doctorId = d.doctorId,
                        doctorName = d.doctorName
                    }).ToList();

                return Json(doctors);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddAppointment(Appointment appointment)
        {
            try
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

                        var patient = manageAppointment
                            .getPatientName()
                            .FirstOrDefault(p => p.patientId == appointment.PatientId);

                        var doctor = manageAppointment
                            .getDoctorByDepartment(appointment.DepartmentId)
                            .FirstOrDefault(d => d.doctorId == appointment.DoctorId);

                        var department = manageAppointment
                            .getDepartmentList()
                            .FirstOrDefault(dep => dep.departmentId == appointment.DepartmentId);

                        string patientEmail = patient?.email ?? "xyz@gmail.com";
                        string patientName = patient?.PatientName ?? "Unknown Patient";
                        string doctorName = doctor?.doctorName ?? "Unknown Doctor";
                        string deptName = department?.DepartmentName ?? "Unknown Department";

                        string subject = "Appointment Confirmation";
                        string body = $@"
                        <h3>Dear {patientName},</h3>
                        <p>Your appointment has been successfully booked.</p>
                        <p><b>Doctor:</b> {doctorName}</p>
                        <p><b>Department:</b> {deptName}</p>
                        <p><b>Date & Time:</b> {appointment.AppointmentDate:dd-MMM-yyyy hh:mm tt}</p>
                        <br/>
                        <p>Thank you,<br/>Hospital Management System</p>";

                        mailService.SendMail(patientEmail, subject, body);
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
            catch (Exception ex)
            {
                TempData["AppointmentMessage"] = "error";
                Console.WriteLine(ex);
                return View();
            }
        }

        public IActionResult selectAllAppointment()
        {
            try
            {
                var appointmentData = manageAppointment.selectAllAppointment();
                return View(appointmentData);
            }
            catch (Exception ex)
            {
                TempData["AppointmentMessage"] = "An error occurred: " + ex.Message;
                return View(new List<Appointment>());
            }
        }

        public IActionResult EditAppointment(int appointmentId)
        {
            try
            {
                Appointment? appointment = appointments.Find(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                {
                    return RedirectToAction("selectAllAppointment", "Appointment");
                }

                if (appointment.AppointmentStatus == "complete")
                {
                    return RedirectToAction("selectAllAppointment");
                }

                ViewBag.PatientList = manageAppointment.getPatientName();
                ViewBag.DepartmentList = manageAppointment.getDepartmentList();
                ViewBag.DoctorList = manageAppointment.getDoctorByDepartment(appointment.DepartmentId);

                ViewBag.StatusList = status;
                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["AppointmentMessage"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("selectAllAppointment");
            }
        }

        [HttpPost]
        public IActionResult EditAppointment(Appointment appointment)
        {
            try
            {
                userId = HttpContext.Session.GetInt32("UserId");

                bool exists = appointments.Any(a =>
                   a.DoctorId == appointment.DoctorId &&
                   a.DepartmentId == appointment.DepartmentId &&
                   a.AppointmentDate == appointment.AppointmentDate &&
                   a.AppointmentId != appointment.AppointmentId);

                if (exists)
                {
                    TempData["AppointmentMessage"] = "Exists";
                }
                else
                {
                    int result = manageAppointment.updateAppointment(appointment, (int)userId!);

                    if (result > 0)
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
            catch (Exception ex)
            {
                TempData["AppointmentMessage"] = "error";
                Console.WriteLine(ex);
                return View(appointment);
            }
        }

        public IActionResult DeleteAppointment(int appointmentId)
        {
            try
            {
                int result = manageAppointment.deleteAppointment(appointmentId);
                return RedirectToAction("selectAllAppointment");
            }
            catch (Exception ex)
            {
                TempData["AppointmentMessage"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("selectAllAppointment");
            }
        }
    }
}