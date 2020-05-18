using Microsoft.AspNet.Identity.Owin;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public LoginController()
        {
        }

        public LoginController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Login
        [AllowAnonymous]
        public ActionResult Loginpage()
        {
            return View();
        }
        
        public ActionResult Autheriz(User userModel)
        {
            using (DBmodel db = new DBmodel())
            {
                string username = userModel.Username;
                string password = userModel.Password;

                var teacherDetails = db.Teachers.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                var managerDetails = db.Offices.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                var cleanerDetails = db.Cleaners.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                var studentDetails = db.StudentTBs.Where(x => x.username == username && x.password == password).FirstOrDefault();

                if (teacherDetails != null)
                {
                    Session["UserID"] = teacherDetails.UserID;
                        Session["Role"] = "Teacher";
                    return RedirectToAction("Index", "Homepage");
                }
                else if (managerDetails != null)
                {
                    Session["UserID"] = managerDetails.UserID;
                        Session["Role"] = "Staff";
                        return RedirectToAction("Index", "AdminPanel");
                }
                else if (cleanerDetails != null)
                {
                    Session["UserID"] = cleanerDetails.UserID;
                        Session["Role"] = "Cleaner";
                        return RedirectToAction("Index", "Homepage");
                }
                else if (studentDetails != null)
                {
                    Session["sid"] = studentDetails.sid;
                    Session["Role"] = "Student";
                    Session["Subject"] = studentDetails.subject;
                    Session["Grade"] = studentDetails.grade;
                    return RedirectToAction("Index", "Homepage");
                }
                else if (username == "Admin" && password == "admin")
                {
                    
                        Session["Role"] = "Admin";
                        return RedirectToAction("Index", "AdminPanel");

                }
                else
                {
                    userModel.LoginErrorMessage = "wrong username or password";
                    return View("Loginpage", userModel);

                }
            }

        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["sid"] = null;
            Session["Role"] = null;
            return  RedirectToAction("Index", "Homepage");
        }
    }
}