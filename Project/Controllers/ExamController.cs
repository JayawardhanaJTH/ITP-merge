using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Project.Models;
using System.Net;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;

namespace Exam__.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        public ActionResult Index()
        {
            DBmodel DB = new DBmodel();

            List<subject> subjectList = DB.subjects.ToList();
            ViewBag.subList = new SelectList(subjectList, "subject_id", "subject1");

            List<GradeList> gradeList = DB.GradeLists.ToList();
            ViewBag.grList = new SelectList(gradeList, "ID", "Grade");

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, ExamUpload ex)
        {

            if (!ModelState.IsValid)
            {


                return new JsonResult { Data = "File not upload" };


            }
            else
            {
                DBmodel DB = new DBmodel();
                Exam2 log = new Exam2();

                var subject = DB.subjects.Where(m => m.subject_id == ex.subject_id).Select(u => new { subject = u.subject1 }).Single();
                var grade = DB.GradeLists.Where(m => m.ID == ex.grade_id).Select(u => new { grade = u.Grade}).Single();

                string path = Server.MapPath("~/App_Data/File/");
                string fileName = Path.GetFileName(file.FileName);

                string fullPath = Path.Combine(path, fileName);

                log.FileName = fileName;
                log.FilePath = fullPath;
                log.Grade = grade.grade;
                log.Subject = subject.subject;
                log.TeacherName = "ASDF";


                DB.Exam2.Add(log);
                DB.SaveChanges();


                file.SaveAs(fullPath);
                return RedirectToAction("ViewList");
            }
        }

        public ActionResult ViewList()
        {
            DBmodel db = new DBmodel();

            List<Exam2> files = db.Exam2.ToList();

            return View(files);

        }
        public FileResult DownloadFile(string FileName)
        {
            var directory = new DirectoryInfo(Server.MapPath("~/App_Data/File"));

            string path = directory.FullName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(@path + "\\" + FileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBmodel db = new DBmodel();
            Exam2 file = db.Exam2.Find(id);

            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);

        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSucces(int id)
        {
            DBmodel db = new DBmodel();
            Exam2 file = db.Exam2.Find(id);


            Exam2 teacher = db.Exam2.Find(id);
            db.Exam2.Remove(teacher);
            db.SaveChanges();

            var directory = new DirectoryInfo(Server.MapPath("~/App_Data/File"));
            string getFile = directory.FullName + "\\" + file.FileName;
            System.IO.File.Delete(getFile);

            //db.Exam2.Remove(file);
            db.SaveChanges();

            return RedirectToAction("ViewList");
        }
        public ActionResult Edit(int? id)
        {
            DBmodel db = new DBmodel();
            Exam2 file = db.Exam2.Find(id);
            ExamUpload model = new ExamUpload();

            model.ExamID = file.ExamID;
            model.subject1 = file.Subject;
            model.grade1 = file.Grade;
            model.TeacherName = file.TeacherName;
            model.FileName = file.FileName;
            model.FilePath = file.FilePath;


            List<subject> subjectList = db.subjects.ToList();
            ViewBag.subList = new SelectList(subjectList, "subject_id", "subject1");

            List<GradeList> gradeList = db.GradeLists.ToList();
            ViewBag.grList = new SelectList(gradeList, "ID", "Grade");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, ExamUpload model)
        {
            DBmodel db = new DBmodel();
            Exam2 exam = db.Exam2.Find(id);

            if (ModelState.IsValid)
            {

                string oldName = exam.FileName;
                exam.FileName = model.FileName;

                int grade2 = model.grade_id;
                int subject2 = model.subject_id;

                var subject = db.subjects.Where(m => m.subject_id == subject2).Select(u => new { subject = u.subject1 }).Single();
                var grade = db.GradeLists.Where(m => m.ID == grade2).Select(u => new { grade = u.Grade }).Single();

                //file.FilePath = fullPath;
                exam.Grade = grade.grade;
                exam.Subject = subject.subject;

                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();

                int ExamID = model.ExamID;

                Exam2 teacher = db.Exam2.Find(id);

                teacher.ExamID = model.ExamID;

                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();

                ChangeFileName(ExamID, model.FileName, oldName);

                return RedirectToAction("ViewList");
            }
            else
            {

                List<GradeList> gradeList = db.GradeLists.ToList();
                ViewBag.grList = new SelectList(gradeList, "ID", "Grade");

                List<subject> subjectList = db.subjects.ToList();
                ViewBag.subList = new SelectList(subjectList, "subject_id", "subject1");




                return View(model);
            }

        }
        public void ChangeFileName(int Exam_ID, string FileName, string oldName)
        {
            DBmodel db = new DBmodel();

            Exam2 file = db.Exam2.Find(Exam_ID);

            var directory = new DirectoryInfo(Server.MapPath("~/App_Data/File"));
            string getFile = directory.FullName + "\\" + oldName;


            System.IO.File.Move(getFile, directory.FullName + "\\" + FileName);

            string path = file.FilePath;

            path = "C:\\Users\\Halo\\source\\repos\\Exam__\\Exam__\\App_Data\\File" + FileName;

            file.FilePath = path;
            db.Entry(file).State = EntityState.Modified;
            db.SaveChanges();
        }
        public ActionResult Reports()
        {
            DBmodel db = new DBmodel();
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/ExamReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "Exam2DataSet";
            reportDataSource.Value = db.Exam2.ToList();
            localReport.DataSources.Add(reportDataSource);
            string mimeType;
            string encoding;
            string fileNameExtenction = "pdf";
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render("PDF", "", out mimeType, out encoding, out fileNameExtenction, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment:filename = Exam2_report." + fileNameExtenction);
            return File(renderedByte, fileNameExtenction);

        }


        ///--- Student ---///

        public ActionResult StudentViewList()
        {
            DBmodel db = new DBmodel();

            List<Exam2> files = db.Exam2.ToList();

            return View(files);
        }


    }
}