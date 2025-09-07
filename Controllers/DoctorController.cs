using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        ManageDepartment manageDepartment = new ManageDepartment();
        ManageDoctor manageDoctor = new ManageDoctor();
        public static List<Doctor> doctorsList = new List<Doctor>();
        public static List<Department> DepartmentIdList = new List<Department>();

        int? userId;

        public IActionResult addDoctor()
        {
            try
            {
                ViewBag.DepartmentList = manageDoctor.getDepartmentName();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error";
                Console.WriteLine(ex);
                return View();
            }
        }
        public IActionResult SelectAllDoctor()
        {
            try
            {
                var datareader = manageDoctor.select_All_Doctor();
                return View(datareader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(new List<Doctor>());
            }
        }

        [HttpPost]
        public IActionResult addDoctor(Doctor doctor, List<string> SelectedDepartments)
        {
            try
            {
                userId = HttpContext.Session.GetInt32("UserId");
                int check = manageDoctor.check_Befor_Insert(doctor.doctorName!);

                if (check > 0)
                {
                    TempData["Message"] = "Exists";
                }
                else
                {
                    int result = manageDoctor.addDoctor(doctor, (int)userId!);

                    int id = manageDoctor.getDoctorId(doctor.doctorName!);

                    if (id > 0 && SelectedDepartments.Count > 0)
                    {
                        if (SelectedDepartments.Count > 0)
                        {
                            foreach (var deptId in SelectedDepartments)
                            {
                                manageDoctor.addDoctorDepartment(id, Convert.ToInt32(deptId), (int)userId);
                            }
                        }
                    }
                    else
                    {
                        TempData["Message"] = "DoctorNotFound";
                    }

                    if (result > 0)
                    {
                        TempData["Message"] = "Successfull";
                        return RedirectToAction("addDoctor");
                    }
                    else
                    {
                        TempData["Message"] = "fail";
                    }
                }

                ViewBag.DepartmentList = manageDoctor.getDepartmentName();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error";
                Console.WriteLine(ex);
                ViewBag.DepartmentList = manageDoctor.getDepartmentName();
                return View();
            }
        }

        public IActionResult deleteDoctor(int doctorId)
        {
            try
            {
                int result = manageDoctor.delete_Doctor(doctorId);
                return RedirectToAction("SelectAllDoctor");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("SelectAllDoctor");
            }
        }

        public IActionResult editDoctor(int doctorId)
        {
            try
            {
                DepartmentIdList.Clear();

                var deptid = new List<int>();
                HttpContext.Session.SetInt32("Id", doctorId);
                Doctor obj = manageDoctor.getDoctorById(doctorId);

                if (obj == null)
                {
                    return RedirectToAction("selectalldoctor");
                }


                deptid.Clear();
                var SelecteddepartmentId = manageDoctor.getDoctorSelectDepartmentId(obj.doctorId);
                var departmentList = manageDepartment.getAllDepartment();

                foreach (var i in SelecteddepartmentId)
                {
                    deptid.Add(i.departmentId);
                }

                ViewBag.SelectedDepartmentId = deptid;
                ViewBag.DepartmentList = departmentList;

                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["UpdateAlert"] = "error";
                Console.WriteLine(ex);
                Doctor obj = manageDoctor.getDoctorById(doctorId);
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult editDoctor(Doctor doctor, List<int> SelectedDepartments)
        {
            try
            {
                userId = HttpContext.Session.GetInt32("UserId");
                doctorsList.Clear();
                DepartmentIdList.Clear();

                int? doctorIdFromSession = HttpContext.Session.GetInt32("Id");
                if (doctorIdFromSession == null)
                {
                    return RedirectToAction("SelectAllDoctor");
                }

                // Get fresh doctor details
                Doctor obj = manageDoctor.getDoctorById(doctor.doctorId);

                // Fetch old department IDs assigned to this doctor
                var previousDepartmentIds = manageDoctor
                    .getDoctorSelectDepartmentId(doctor.doctorId)
                    .Select(d => d.departmentId)
                    .ToList();

                // Prepare ViewBag for redisplay in case of error
                ViewBag.DepartmentList = manageDepartment.getAllDepartment();
                ViewBag.SelectedDepartmentId = SelectedDepartments;

                // Check for duplicate name
                int nameCheckResult = manageDoctor.check_Doctor_Before_Update(doctor.doctorId, doctor.doctorName!);
                if (nameCheckResult == 1)
                {
                    TempData["UpdateAlert"] = "Exists";
                    //return View(obj);
                    return RedirectToAction("editDoctor", "Doctor", obj);
                }

                // Perform update
                int updateResult = manageDoctor.update_Doctor(doctor, (int)userId!);
                if (updateResult == 1)
                {
                    // Get departments to ADD
                    var toAdd = SelectedDepartments.Except(previousDepartmentIds).ToList();

                    // Get departments to REMOVE
                    var toRemove = previousDepartmentIds.Except(SelectedDepartments).ToList();

                    // Add new departments
                    foreach (var deptId in toAdd)
                    {
                        manageDoctor.addDoctorDepartment(doctor.doctorId, deptId, (int)userId);
                    }

                    // Remove deselected departments
                    foreach (var deptId in toRemove)
                    {
                        manageDoctor.deleteDoctorDepartmentById(deptId, doctor.doctorId);
                    }

                    // Clear session
                    HttpContext.Session.Remove("Id");
                    return RedirectToAction("SelectAllDoctor");
                }

                TempData["UpdateAlert"] = "error";
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["UpdateAlert"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("SelectAllDoctor");
            }
        }
    }
}