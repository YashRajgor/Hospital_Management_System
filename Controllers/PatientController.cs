using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Hospital_Management_System.Controllers
{
    public class PatientController : Controller
    {
        ManagePatient managePatient = new ManagePatient();
        public static List<Patient> Patients = new List<Patient>();
        int? userId;
        public IActionResult addPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult addPatient(Patient patient)
        {
            userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                int result = managePatient.checkPatient(patient.PatientName);
                if (result == 0)
                {
                    result = managePatient.addPatient(patient, (int)userId);

                    if (result > 0)
                    {
                        TempData["PatientMessage"] = "Success";
                    }
                    else
                    {
                        TempData["PatientMessage"] = "Fail";
                    }
                }
                else
                {
                    TempData["PatientMessage"] = "Exists";
                }
                return View();
            }

            return RedirectToAction("Login", "Admin");
        }

        public IActionResult selectAllPatient()
        {
            userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var readdata = managePatient.select_All_Patient();
                return View(readdata);
            }

            return RedirectToAction("Login", "Admin");
        }

        //[Route("/Patient/deletePatient/{patientId?}")]
        
        public IActionResult deletePatient(int patientId)
        {
            int result = managePatient.deletePatient(patientId);

            return RedirectToAction("selectAllPatient");
        }

        

        public IActionResult editPatient(int patientId)
        {
            HttpContext.Session.SetInt32("patientId", patientId);
            Patient? patient = Patients.Find(i => i.patientId == patientId);
            if (patient == null)
            {
                return RedirectToAction("selectAllPatient");
            }
            return View(patient);
        }

        [HttpPost]
        public IActionResult editPatient(Patient patient)
        {
            userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                int result = managePatient.checkBeforePatientUpdate(patient);
                Patient? patientsObj = Patients.Find(i => i.patientId == patient.patientId);
                if (result == 1)
                {
                    TempData["patientUpdate"] = "Exists";
                    if (patientsObj == null)
                    {
                        return RedirectToAction("selectAllPatient");
                    }
                    return View(patientsObj);
                }
                else
                {
                    result = managePatient.editPatient(patient, (int)userId);

                    if (result > 0)
                    {
                        HttpContext.Session.Remove("patientId");
                        return RedirectToAction("selectAllPatient");
                    }

                    TempData["patientUpdate"] = "error";
                    //return View(patientsObj);
                    return RedirectToAction("editPatient","Patient", patientsObj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
    }
}
