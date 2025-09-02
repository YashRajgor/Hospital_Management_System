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
            new Patient { IsNew = true };
            return View();
        }

        [HttpPost]
        public IActionResult addPatient(Patient patient)
        {
            userId = HttpContext.Session.GetInt32("UserId");

            if (patient.ImageFile != null && patient.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(patient.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    patient.ImageFile.CopyTo(fileStream);
                }

                patient.PatientImage = uniqueFileName;
            }

            int result = managePatient.checkPatient(patient.PatientName!);
            if (result == 0)
            {
                result = managePatient.addPatient(patient, (int)userId!);
                TempData["PatientMessage"] = result > 0 ? "Success" : "Fail";
            }
            else
            {
                TempData["PatientMessage"] = "Exists";
            }
            return View();
            //int result = managePatient.checkPatient(patient.PatientName!);
            //if (result == 0)
            //{
            //    result = managePatient.addPatient(patient, (int)userId!);

            //    if (result > 0)
            //    {
            //        TempData["PatientMessage"] = "Success";
            //    }
            //    else
            //    {
            //        TempData["PatientMessage"] = "Fail";
            //    }
            //}   
            //else
            //{
            //    TempData["PatientMessage"] = "Exists";
            //}
            //return View();
        }

        public IActionResult selectAllPatient()
        {
            var readdata = managePatient.select_All_Patient();
            return View(readdata);
        }

        //[Route("/Patient/deletePatient/{patientId?}")]

        public IActionResult deletePatient(int patientId)
        {
            int result = managePatient.deletePatient(patientId);

            return RedirectToAction("selectAllPatient", "Patient");
        }



        public IActionResult editPatient(int patientId)
        {
            userId = HttpContext.Session.GetInt32("UserId");
            HttpContext.Session.SetInt32("patientId", patientId);
            Patient? patient = Patients.Find(i => i.patientId == patientId);
            if (patient == null)
            {
                return RedirectToAction("selectAllPatient", "Patient");
            }
            return View(patient);
        }

        [HttpPost]
        public IActionResult editPatient(Patient patient)
        {
            userId = HttpContext.Session.GetInt32("UserId");
            patient.IsNew = false;
            Patient? existingPatient = Patients.Find(i => i.patientId == patient.patientId);

            if (existingPatient == null)
            {
                return RedirectToAction("selectAllPatient", "Patient");
            }

            if (patient.ImageFile != null && patient.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(patient.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    patient.ImageFile.CopyTo(fileStream);
                }

                patient.PatientImage = uniqueFileName;  
            }
            else
            {
                patient.PatientImage = existingPatient?.PatientImage; 
            }

            int result = managePatient.checkBeforePatientUpdate(patient);
            if (result == 1)
            {
                TempData["patientUpdate"] = "Exists";
                return View(existingPatient);
            }

            result = managePatient.editPatient(patient, (int)userId!);

            if (result > 0)
            {
                HttpContext.Session.Remove("patientId");
                return RedirectToAction("selectAllPatient", "Patient");
            }

            TempData["patientUpdate"] = "Error";
            return View(existingPatient);
            //int result = managePatient.checkBeforePatientUpdate(patient);
            //Patient? patientsObj = Patients.Find(i => i.patientId == patient.patientId);
            //if (result == 1)
            //{
            //    TempData["patientUpdate"] = "Exists";
            //    if (patientsObj == null)
            //    {
            //        return RedirectToAction("selectAllPatient", "Patient");
            //    }
            //    return View(patientsObj);
            //}
            //else
            //{
            //    result = managePatient.editPatient(patient, (int)userId!);

            //    if (result > 0)
            //    {
            //        HttpContext.Session.Remove("patientId");
            //        return RedirectToAction("selectAllPatient", "Patient");
            //    }

            //    TempData["patientUpdate"] = "error";
            //    return RedirectToAction("editPatient", "Patient", patientsObj);
            //}
        }
    }
}
