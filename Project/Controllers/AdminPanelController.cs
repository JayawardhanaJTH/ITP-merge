using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AdminPanelController : Controller
    {
        // GET: AdminPanel
        public ActionResult Index()
        {
            try
            {
                DBmodel db = new DBmodel();
                AdminPanel panel = new AdminPanel();

                panel.totalStaff = db.Offices.ToList().Count();
                panel.totalStudents = db.StudentTBs.ToList().Count();
                panel.totalTeachers = db.TeacherLists.ToList().Count();

                return View(panel);
            }
            catch (Exception ex)
            {

                return View("Error",(new HandleErrorInfo(ex, "AdminPanel", "Index")));
            }
           
                
          
           
        }
    }
}