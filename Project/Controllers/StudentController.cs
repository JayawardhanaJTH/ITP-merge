using Microsoft.Reporting.WebForms;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student/Home
        public ActionResult Homepage()
        {
            return View();
        }

        // GET: Student
        public ActionResult Index()
        {
            using (DBmodel dBModels = new DBmodel())
            {
                return View(dBModels.StudentTBs.ToList());
            }
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            DBmodel dBModels = new DBmodel();
            localreport.ReportPath = Server.MapPath("~/Reports/StudentReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "StudentDataSet1";
            reportDataSource.Value = dBModels.StudentTBs.ToList();
            localreport.DataSources.Add(reportDataSource);

            String reportType = ReportType;
            String mimeType;
            string encoding;
            String fileNameExtension;

            if (reportType == "PDF")
            {
                fileNameExtension = "pdf";
            }
            else if (reportType == "Word")
            {
                fileNameExtension = "docx";
            }

            string[] strems;
            Warning[] warnings;
            byte[] renderdByte;

            renderdByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out strems, out warnings);
            Response.AddHeader("content-dispostion", "attachment:filename = student_report." + fileNameExtension);
            return File(renderdByte, fileNameExtension);

        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            using (DBmodel dBModels = new DBmodel())
            {
                return View(dBModels.StudentTBs.Where(x => x.sid == id).FirstOrDefault());
            }
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            DBmodel db = new DBmodel();

            List<GradeList> grades = db.GradeLists.ToList();
            List<subject> subjects = db.subjects.ToList();

            ViewBag.Grades = new SelectList(grades, "Grade", "Grade");
            ViewBag.Subjects = new SelectList(subjects, "subject1", "subject1");


            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(StudentTB studentTB)
        {
            try
            {
                using (DBmodel dBModels = new DBmodel())
                {
                   if(dBModels.StudentTBs.Any(m=>m.username == studentTB.username) 
                        || dBModels.Teachers.Any(m=>m.Username == studentTB.username)
                        || dBModels.Cleaners.Any(m=>m.Username == studentTB.username)
                        || dBModels.Offices.Any(m=>m.Username == studentTB.username))
                    {
                        ViewBag.Error = "User name already exist! please use another one";

                        List<GradeList> grades = dBModels.GradeLists.ToList();
                        List<subject> subjects = dBModels.subjects.ToList();

                        ViewBag.Grades = new SelectList(grades, "Grade", "Grade");
                        ViewBag.Subjects = new SelectList(subjects, "subject1", "subject1");
                        return View(studentTB);
                    }
                    else
                    {
                        dBModels.StudentTBs.Add(studentTB);

                        if (ModelState.IsValid)
                        {
                            dBModels.SaveChanges();
                            return RedirectToAction("Index", "Student");
                        }
                        else
                        {
                            return new JsonResult { Data = "Student not Registered" };
                        }
                    }
                }

                //return RedirectToAction("Loginpage", "Student");
            }
            catch
            {
                return View();
            }

        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            using (DBmodel dBModels = new DBmodel())
            {
                return View(dBModels.StudentTBs.Where(x => x.sid == id).FirstOrDefault());
            }
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, StudentTB studentTB)
        {
            try
            {
                using (DBmodel dBModels = new DBmodel())
                {
                    dBModels.Entry(studentTB).State = System.Data.Entity.EntityState.Modified;
                    dBModels.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            using (DBmodel dBModels = new DBmodel())
            {
                return View(dBModels.StudentTBs.Where(x => x.sid == id).FirstOrDefault());
            }
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel dBModels = new DBmodel())
                {
                    StudentTB studentTB = dBModels.StudentTBs.Where(x => x.sid == id).FirstOrDefault();
                    dBModels.StudentTBs.Remove(studentTB);
                    dBModels.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}