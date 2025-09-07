using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        public static List<Department> departmentList = new List<Department>();
        ManageDepartment manageDepartment = new ManageDepartment();

        int? userId;
        public IActionResult AddDepartment()
        {
            try
            {
                if (HttpContext.Session.GetString("username") == null)
                {
                    return RedirectToAction("Login", "Admin");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("ManageDepartment");
            }
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            try
            {


                userId = HttpContext.Session.GetInt32("UserId");

                int reader = manageDepartment.Add_Department(department, DateTime.Now, (int)userId!);
                if (reader == 2)
                {
                    TempData["Message"] = "Exists";
                }
                else if (reader == 1)
                {
                    TempData["Message"] = "Sussfully";
                }
                else
                {
                    TempData["Message"] = "failed";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error";
                Console.WriteLine(ex);
            }
            return View();
        }

        public IActionResult ManageDepartment()
        {
            try
            {
                var dataRead = manageDepartment.getAllDepartment();
                return View(dataRead);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error loading departments: " + ex.Message;
                return View(new List<Department>());
            }
        }


        public IActionResult DeleteDepartment(int departmentId)
        {
            try
            {
                int result = manageDepartment.DeleteDepartment(departmentId);
            }
            catch (Exception ex)
            {
                TempData["DeleteMessage"] = "Error" + ex;
            }
            return RedirectToAction("ManageDepartment", "Department");
        }

        public IActionResult EditDepartment(int departmentId)
        {
            try
            {
                Department? obj = departmentList.Find(i => i.departmentId == departmentId);
                if (obj == null)
                {
                    TempData["Message"] = "error";
                    return RedirectToAction("ManageDepartment", "Department");
                }
                HttpContext.Session.SetInt32("deptId", obj.departmentId);
                HttpContext.Session.SetString("deptName", obj.DepartmentName!);
                return View(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("ManageDepartment", "Department");
            }
        }

        [HttpPost]
        public IActionResult EditDepartment(Department department)
        {
            try
            {
                Department? obj = departmentList.Find(i => i.departmentId == department.departmentId);
                userId = HttpContext.Session.GetInt32("UserId");

                int result = manageDepartment.check_Department_Before_update(department);

                if (result == 1)
                {
                    TempData["alert"] = "Exists";
                    return View(obj);
                }
                else
                {
                    result = manageDepartment.update_Department(department, (int)userId!);

                    if (result == 1)
                    {
                        return RedirectToAction("ManageDepartment", "Department");
                    }
                    TempData["Message"] = "error";
                    return RedirectToAction("EditDepartment", "Department", obj);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("ManageDepartment", "Department");
            }
        }
    }
}
