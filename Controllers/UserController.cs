using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class UserController : Controller
    {
        ManageUser manageUser = new ManageUser();
        MailService services = new MailService();

        public static List<User> userList = new List<User>();
        int? userId;
        public IActionResult addUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult addUser(User user)
        {
            int result = manageUser.checkUserExitsOrNot(user);

            if (result < 1)
            {
                result = manageUser.addUser(user);

                if (result > 0)
                {
                    TempData["UserMessage"] = "Success";
                    return View();
                }
                else
                {
                    TempData["UserMessage"] = "failed";
                    return View();
                }
            }

            TempData["UserMessage"] = "Exists";
            return View();
        }
        public IActionResult selectAllUser()
        {
            userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userId=userId;
            var userList = manageUser.getUserList();
            return View(userList);
        }

        public IActionResult DeleteUser(int userId)
        {
            manageUser.deleteUser(userId);
            return RedirectToAction("selectAllUser", "User");
        }

        public IActionResult EditUser()
        {
            userId = HttpContext.Session.GetInt32("UserId");
            manageUser.getUserList();
            User? obj = userList.Find(u => u.UserId == userId);

            if (obj == null)
            {
                return RedirectToAction("selectAllUser", "User");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            User? obj = userList.Find(u => u.UserId == user.UserId);
            int result = manageUser.checkUserBeforeUpdate(user);

            if (result == 0)
            {
                result = manageUser.updateUser(user);

                if (result > 0)
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    TempData["UserEditMessage"] = "Error";
                    return View(obj);
                }
            }

            TempData["UserEditMessage"] = "Exists";
            return RedirectToAction("EditUSer","User",obj);
        }
    }
}