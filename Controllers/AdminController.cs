using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
    {
        public static List<Department> departmentList = new List<Department>();
        ManageDepartment manageDepartment = new ManageDepartment();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDepartment()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            int? userid = HttpContext.Session.GetInt32("UserId");

            int reader = manageDepartment.Add_Department(department, DateTime.Now, (int)userid!);
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
            return View();
        }

        public IActionResult ManageDepartment()
        {
            //int? userid = HttpContext.Session.GetInt32("UserId")

            var dataRead = manageDepartment.getAllDepartment();

            return View(dataRead);
        }


        public IActionResult DeleteDepartment(int departmentId)
        {
            try
            {
                int result = manageDepartment.DeleteDepartment(departmentId);
            }
            catch (Exception ex)
            {
                TempData["DeleteMessage"] = "Error";
            }
            return RedirectToAction("ManageDepartment");
        }

        public IActionResult EditDepartment(int departmentId)
        {
            Department? obj = departmentList.Find(i => i.departmentId == departmentId);
            if (obj == null)
            {
                return RedirectToAction("ManageDepartment");
            }
            HttpContext.Session.SetInt32("deptId", obj.departmentId);
            HttpContext.Session.SetString("deptName", obj.DepartmentName);
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditDepartment(Department department)
        {
            Department? obj = departmentList.Find(i => i.departmentId == department.departmentId);
            int? userid = HttpContext.Session.GetInt32("UserId");
            int result = manageDepartment.check_Department_Before_update(department);

            if (result == 1)
            {
                TempData["alert"] = "Exists";
                return View(obj);
            }
            else
            {
                result = manageDepartment.update_Department(department, (int)userid!);

                if (result == 1)
                {
                    return RedirectToAction("ManageDepartment");
                }
                TempData["error"] = "Error In Department Update";
                //return View(obj);
                return RedirectToAction("EditDepartment","Admin",obj);
            }
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            LoginMethod dataBase_Method = new LoginMethod();
            SqlDataReader reader = dataBase_Method.Check_Login(email, password);
            if (reader.Read())
            {
                var UserName = reader["UserName"].ToString() ?? "";
                HttpContext.Session.SetString("username", UserName);
                HttpContext.Session.SetInt32("UserId", Convert.ToInt32(reader["UserId"]));
                return View("Dashboard");
            }
            else
            {
                TempData["Message"] = "Invalid Account";
                return View();
            }
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}
