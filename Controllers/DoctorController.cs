using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        ManageDoctor manageDoctor = new ManageDoctor();
        public static List<Doctor> doctorsList = new List<Doctor>();
        int? userId = null;

        public IActionResult addDoctor()
        {
            userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        public IActionResult SelectAllDoctor()
        {
            userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var datareader = manageDoctor.select_All_Doctor();
            return View(datareader);
        }

        [HttpPost]
        public IActionResult addDoctor(Doctor doctor)
        {
            userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            int check = manageDoctor.check_Befor_Insert(doctor.doctorName);

            if (check > 0)
            {
                TempData["Message"] = "Exists";
            }
            else
            {
                int result = manageDoctor.addDoctor(doctor,(int)userId);

                if (result > 0)
                {
                    TempData["Message"] = "Successfull";
                }
                else
                {
                    TempData["Message"] = "fail";
                }
            }

            return View();
        }

        public IActionResult deleteDoctor(int doctorId)
        {
            int result = manageDoctor.delete_Doctor(doctorId);

            return RedirectToAction("SelectAllDoctor");
        }


        public IActionResult editDoctor(int doctorId)
        {
            HttpContext.Session.SetInt32("Id", doctorId);
            Doctor obj = doctorsList.Find(Id => Id.doctorId == doctorId);
            if (obj == null)
            {
                return RedirectToAction("selectalldoctor");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult editDoctor(Doctor doctor)
        {
            userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                
                if (HttpContext.Session.GetInt32("Id") != null)
                {

                    int result = manageDoctor.check_Doctor_Before_Update(doctor.doctorId, doctor.doctorName);
                    Doctor obj = doctorsList.Find(Id => Id.doctorId == doctor.doctorId);

                    if (result == 1)
                    {
                        TempData["UpdateAlert"] = "Exists";
                        return View(obj);
                    }
                    else
                    {
                        result = manageDoctor.update_Doctor(doctor, (int)userId);

                        if (result == 1)
                        {
                            HttpContext.Session.Remove("Id");
                            return RedirectToAction("selectalldoctor");
                        }

                        TempData["UpdateAlert"] = "Error";
                        return View(obj);
                    }
                }
                else
                {
                    return RedirectToAction("SelectAllDoctor");
                }
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
    }
}
