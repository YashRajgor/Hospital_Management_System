using Hospital_Management_System.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDepartment()
        {
            return View();
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
            DataBase_Method dataBase_Method = new DataBase_Method();
            SqlDataReader reader = dataBase_Method.Check_Login(email, password);
            if (reader.Read())
            {
                var UserName = reader["UserName"].ToString();
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
