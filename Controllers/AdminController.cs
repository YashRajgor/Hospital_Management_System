using Hospital_Management_System.Classes;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
    {
        ManageAdmin manageAdmin = new ManageAdmin();
        ManageUser manageUser = new ManageUser();
        MailService mailService = new MailService();

        [AllowAnonymous]
        public IActionResult Login()
        {
            try
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
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Admin admin)
        {
            try
            {
                SqlDataReader reader = manageAdmin.Check_Login(admin.userName!, admin.password!);
                if (reader.Read())
                {
                    var UserName = reader["UserName"].ToString() ?? "";
                    HttpContext.Session.SetString("username", UserName);
                    HttpContext.Session.SetInt32("UserId", Convert.ToInt32(reader["UserId"]));
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    TempData["Message"] = "Invalid Account";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred: " + ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Login", "Admin");
            }
        }

        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ForgetPassword(Admin admin, string actionStep)
        {
            try
            {
                // Get user by email
                var userlist = manageUser.getUserList();
                var user = userlist.Find(u => u.email == admin.email);

                if (user == null)
                {
                    TempData["AdminMessage"] = "Email does not exist.";
                    TempData["AdminMessageType"] = "error";
                    return View();
                }

                if (actionStep == "sendOTP")
                {
                    int otp = manageAdmin.GenerateOTP(admin.email!);

                    string subject = "Your OTP for Password Reset";
                    string body = $"<p>Hello {user.userName},</p><p>Your OTP for password reset is: <strong>{otp}</strong> OTP Valid For 5 Minutes</p>";

                    bool isSent = mailService.SendMail(admin.email!, subject, body);

                    if (isSent)
                    {
                        TempData["AdminMessage"] = "OTP sent successfully. Please check your email.";
                        TempData["AdminMessageType"] = "success";
                        TempData["ShowOTPSection"] = true;
                    }
                    else
                    {
                        TempData["AdminMessage"] = "Failed to send OTP. Try again later.";
                        TempData["AdminMessageType"] = "error";
                    }
                }
                else if (actionStep == "verifyOTP")
                {
                    if (manageAdmin.VerifyOTP(admin.email!, admin.OTP))
                    {
                        TempData["AdminMessage"] = "OTP verified successfully. You can now reset your password.";
                        TempData["AdminMessageType"] = "success";
                        TempData["ShowResetSection"] = true;
                    }
                    else
                    {
                        TempData["AdminMessage"] = "Invalid or expired OTP. Please try again.";
                        TempData["AdminMessageType"] = "error";
                        TempData["ShowOTPSection"] = true;
                    }
                }
                else if (actionStep == "resetPassword")
                {
                    if (admin.newPassword != admin.confirmNewPassword)
                    {
                        TempData["AdminMessage"] = "Passwords do not match.";
                        TempData["AdminMessageType"] = "warning";
                        TempData["ShowResetSection"] = true;
                    }
                    else
                    {
                        bool updated = manageAdmin.UpdatePassword(admin.email!, admin.newPassword!);
                        if (updated)
                        {
                            TempData["AdminMessage"] = "Password updated successfully. You can now login.";
                            TempData["AdminMessageType"] = "success";
                            return RedirectToAction("Login", "Admin");
                        }
                        else
                        {
                            TempData["AdminMessage"] = "Failed to update password. Try again.";
                            TempData["AdminMessageType"] = "error";
                            TempData["ShowResetSection"] = true;
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["AdminMessage"] = "An error occurred";
                TempData["AdminMessageType"] = "error";
                Console.WriteLine(ex.Message);
                return View();
            }
        }
    }
}