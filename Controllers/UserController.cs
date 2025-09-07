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
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        [HttpPost]
        public IActionResult addUser(User user)
        {
            try
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
            catch (Exception ex)
            {
                TempData["UserMessage"] = "error";
                Console.WriteLine(ex);
                return View();
            }
        }
        public IActionResult selectAllUser()
        {
            try
            {
                userId = HttpContext.Session.GetInt32("UserId");
                ViewBag.userId = userId;
                var userList = manageUser.getUserList();
                return View(userList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(new List<User>());
            }
        }

        public IActionResult DeleteUser(int userId)
        {
            try
            {
                manageUser.deleteUser(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["UserMessage"] = "error";
            }
            return RedirectToAction("selectAllUser", "User");
        }

        public IActionResult EditUser()
        {
            try
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
            catch (Exception ex)
            {
                TempData["UserEditMessage"] = "error";
                Console.WriteLine(ex);
                return RedirectToAction("selectAllUser", "User");
            }
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            try
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
                return RedirectToAction("EditUSer", "User", obj);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                User? obj = userList.Find(u => u.UserId == user.UserId);
                return RedirectToAction("selectAllUser", "User",obj);
            }
        }
    }
}