using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;

namespace Timetable.Controllers
{
    public class TimetableController : Controller
    {
        public ActionResult Timetablehome()
        {
            
            return View();
        }

        // GET: Timetable
        public ActionResult TimetableList(string searching)
        {
            using(DBmodel tution = new DBmodel())
            {
               
                return View(tution.Timetables.Where(x=> x.Day.StartsWith(searching)|| searching == null).ToList());
            }
        }

        // GET: Timetable/Details/5
        public ActionResult TimetableDetails(int id)
        {
            using (DBmodel tution = new DBmodel())
            {
                return View(tution.Timetables.Where(x => x.classId == id).FirstOrDefault());
            }
        }

        // GET: Timetable/Create
        public ActionResult CreateTimetable()
        {
            DBmodel db = new DBmodel();

            List<TeacherList> list = db.TeacherLists.ToList();
            List<GradeList> grades = db.GradeLists.ToList();
            List<subject> subjects = db.subjects.ToList();

            ViewBag.teacherlist = new SelectList(list, "teacher_name", "teacher_name");
            ViewBag.Grades = new SelectList(grades, "Grade", "Grade");
            ViewBag.Subjects = new SelectList(subjects, "subject1", "subject1");

            return View();
        }

        // POST: Timetable/Create
        [HttpPost]
        public ActionResult CreateTimetable(Project.Models.Timetable timetable)
        {
            try
            {
                using (DBmodel tution = new DBmodel())
                {
                    ViewBag.IsError = false;
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            SubjectDetail subject = new SubjectDetail();

                            var subje = tution.SubjectTBs.Where(m => m.SubjectName == timetable.Subject).Select(u => new { sub = u.SubjectCode }).Single();
                            
                            tution.Timetables.Add(timetable);
                            tution.SaveChanges();

                            subject.Hall = timetable.Hall;
                            subject.StartTime = timetable.StartTime;
                            subject.EndTime = timetable.EndTime;
                            subject.Day = timetable.Day;
                            subject.SubjectID = subje.sub;

                            tution.SubjectDetails.Add(subject);
                            tution.SaveChanges();

                            return RedirectToAction("TimetableList");
                        }
                    }
                    catch
                    {

                    }
                }
            }

            catch
            {
                DBmodel db = new DBmodel();

                List<TeacherList> list = db.TeacherLists.ToList();
                List<GradeList> grades = db.GradeLists.ToList();
                List<subject> subjects = db.subjects.ToList();

                ViewBag.teacherlist = new SelectList(list, "teacher_name", "teacher_name");
                ViewBag.Grades = new SelectList(grades, "Grade", "Grade");
                ViewBag.Subjects = new SelectList(subjects, "subject1", "subject1");

                return View();
            }
            return RedirectToAction("TimetableList");
        }
        

        // GET: Timetable/Edit/5
        public ActionResult EditTimetable(int id)
        {
            using (DBmodel tution = new DBmodel())
            {
                DBmodel db = new DBmodel();

                List<TeacherList> list = db.TeacherLists.ToList();
                List<GradeList> grades = db.GradeLists.ToList();
                List<subject> subjects = db.subjects.ToList();

                ViewBag.teacherlist = new SelectList(list, "teacher_name", "teacher_name");
                ViewBag.Grades = new SelectList(grades, "Grade", "Grade");
                ViewBag.Subjects = new SelectList(subjects, "subject1", "subject1");

                return View(tution.Timetables.Where(x => x.classId == id).FirstOrDefault());
            }
        }

        // POST: Timetable/Edit/5
        [HttpPost]
        public ActionResult EditTimetable(int id, Project.Models.Timetable timetable)
        {
            try
            {
                using (DBmodel tution = new DBmodel())
                {
                    //SubjectDetail subject = new SubjectDetail();

                    //string sub = tution.Timetables.Find(id).Subject.ToString();
                    timetable.classId = id;

                    tution.Entry(timetable).State = EntityState.Modified;
                    tution.SaveChanges();

                    //var subje = tution.SubjectTBs.Where(m => m.SubjectName == timetable.Subject).Select(u => new { sub = u.SubjectCode }).Single();

                    //subject.Hall = timetable.Hall;
                    //subject.StartTime = timetable.StartTime;
                    //subject.EndTime = timetable.EndTime;
                    //subject.Day = timetable.Day;
                    //subject.SubjectID = subje.sub;

                    //tution.Entry(subject).State = EntityState.Modified;
                    //tution.SaveChanges();

                    return RedirectToAction("TimetableList");
                }
                // TODO: Add update logic here

                
            }
            catch
            {

                return RedirectToAction("TimetableList");
            }
        }

        // GET: Timetable/Delete/5
        public ActionResult DeleteTimetable(int id)
        {
            using (DBmodel tution = new DBmodel())
            {
                return View(tution.Timetables.Where(x => x.classId == id).FirstOrDefault());
            }
        }

        // POST: Timetable/Delete/5
        [HttpPost]
        public ActionResult DeleteTimetable(int id, FormCollection collection)
        {
            Project.Models.Timetable timetable = new Project.Models.Timetable();
            try
            {
               
                using (DBmodel tution = new DBmodel())
                {
                    
                    timetable = tution.Timetables.Where(x => x.classId == id).FirstOrDefault();

                    //var subje = tution.SubjectTBs.Where(m => m.SubjectName == timetable.Subject).Select(u => new { sub = u.SubjectCode }).Single();


                    //SubjectDetail subject = tution.SubjectDetails.Where(x => x.SubjectID == subje.sub).FirstOrDefault();
                   
                    tution.Timetables.Remove(timetable);
                    tution.SaveChanges();

                    //tution.SubjectDetails.Remove(subject);
                    //tution.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("TimetableList");
            }
            catch
            {
                return View(timetable);
            }
        }
        public ActionResult Reports()
        {
            DBmodel tution = new DBmodel();
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/TimetableReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "TimetableDataSet";
            reportDataSource.Value = tution.Timetables.ToList();
            localReport.DataSources.Add(reportDataSource);
            string mimeType;
            string encoding;
            string fileNameExtenction = "pdf";
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render("pdf", "", out mimeType, out encoding, out fileNameExtenction, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment:filename = timetable_report." + fileNameExtenction);
            return File(renderedByte, fileNameExtenction);

        }
    }


}

