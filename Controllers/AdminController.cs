using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string email, string password)
        {
            LoginMethod dataBase_Method = new LoginMethod();
            SqlDataReader reader = dataBase_Method.Check_Login(email, password);
            if (reader.Read())
            {
                var UserName = reader["UserName"].ToString() ?? "";
                HttpContext.Session.SetString("username", UserName);
                HttpContext.Session.SetInt32("UserId", Convert.ToInt32(reader["UserId"]));
                return RedirectToAction("Dashboard","Dashboard");
            }
            else
            {
                TempData["Message"] = "Invalid Account";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Admin");
        }
    }
}
