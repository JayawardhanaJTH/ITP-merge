using Microsoft.Reporting.WebForms;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class SubjectController : Controller
    {
        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        // GET: Subject/Create
        public ActionResult Create()
        {
            ViewBag.IsError = false;
            SubjectDetailsViewModel model = new SubjectDetailsViewModel();
            model.Subject = new SubjectViewModel() { SubjectName = "" };
            using (DBmodel DBmodel = new DBmodel())
            {
                model.Teachers = DBmodel.TeacherLists.ToList<TeacherList>();
                model.Grades = DBmodel.GradeLists.ToList<GradeList>();
            }
            return View(model);
        }

        // POST: Subject/Create
        [HttpPost]
        public ActionResult Create(SubjectDetailsViewModel viewModel)
        {
            using (DBmodel DBmodel = new DBmodel())
            {
                ViewBag.IsError = false;
                try
                {
                    if (ModelState.IsValid)
                    {
                        SubjectTB model = new SubjectTB();
                        subject model_subject = new subject();
                        teacher_grade model_grade = new teacher_grade();
                        teacher_subject model_teacher = new teacher_subject();

                        model.Grade = viewModel.Subject.Grade;
                        model.SubjectName = viewModel.Subject.SubjectName;
                        model.Teacher = viewModel.Subject.Teacher;
                       

                        if (!DBmodel.SubjectTBs.Any(x => x.Teacher == viewModel.Subject.Teacher && x.Grade == viewModel.Subject.Grade && x.SubjectName == viewModel.Subject.SubjectName))
                        {
                            DBmodel.SubjectTBs.Add(model);
                            DBmodel.SaveChanges();

                            List<SubjectTB> list = DBmodel.SubjectTBs.ToList();
                            SubjectTB index = list.Last();

                            model_subject.subject_id = index.SubjectCode;
                            model_subject.subject1 = index.SubjectName;

                            model_grade.grade = index.Grade;
                            model_grade.teacher_id = index.Teacher;
                            model_grade.grade_id = DBmodel.GradeLists.Find(index.Grade).ID;
                            model_grade.subjectCode = index.SubjectCode;

                            model_teacher.subject_id = index.SubjectCode;
                            model_teacher.teacher_id = index.Teacher;

                            DBmodel.subjects.Add(model_subject);
                            DBmodel.teacher_grade.Add(model_grade);
                            DBmodel.teacher_subject.Add(model_teacher);

                            DBmodel.SaveChanges();

                            return RedirectToAction("SubjectList");
                        }
                        else
                        {
                            ViewBag.DuplicateMessage = "Subject Details already exist.";
                            ViewBag.IsError = true;
                        }
                    }
                }
                catch
                {
                   
                }
                viewModel.Teachers = DBmodel.TeacherLists.ToList<TeacherList>();
                viewModel.Grades = DBmodel.GradeLists.ToList<GradeList>();
            }
            return View(viewModel);
        }

        public ActionResult SubjectList()
        {
            using (DBmodel DBmodel = new DBmodel())
            {
                return View(DBmodel.SubjectTBs.ToList());
            }
        }
        // GET: Subject
        public ActionResult GetData()
        {
            using (DBmodel DBmodel = new DBmodel())
            {
                List<SubjectViewModel> subjectTB = DBmodel.SubjectTBs.Select(s =>
                    new SubjectViewModel
                    {
                        Grade = s.GradeList.Grade,
                        SubjectCode = s.SubjectCode,
                        SubjectName = s.SubjectName,
                        TeacherName = s.TeacherList.teacher_name
                    }
                ).ToList<SubjectViewModel>();
                return Json(new { data = subjectTB }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Subject/Edit/
        public ActionResult Edit(int id)
        {
            SubjectDetailsViewModel model = new SubjectDetailsViewModel();
            using (DBmodel DBmodel = new DBmodel())
            {
                model.Subject = DBmodel.SubjectTBs.Where(x => x.SubjectCode == id).Select(s => new SubjectViewModel
                {
                    Grade = s.Grade,
                    SubjectCode = s.SubjectCode,
                    SubjectName = s.SubjectName,
                    TeacherName = s.TeacherList.teacher_name
                }).FirstOrDefault();
                model.Teachers = DBmodel.TeacherLists.ToList<TeacherList>();
                model.Grades = DBmodel.GradeLists.ToList<GradeList>();

                return View(model);
            }

        }

        // POST: Subject/Edit/
        [HttpPost]
        public ActionResult Edit(int id, SubjectDetailsViewModel model)
        {
            using (DBmodel DBmodel = new DBmodel())
            {
                ViewBag.IsError = false;

                try
                {
                    if (ModelState.IsValid)
                    {
                        subject model_subject = DBmodel.subjects.Find(id);
                        teacher_grade model_grade = DBmodel.teacher_grade.Find(id);
                        teacher_subject model_teacher = DBmodel.teacher_subject.Find(id);

                        model.Subject.SubjectCode = id;
                        var subject = DBmodel.SubjectTBs.Where(x => x.SubjectCode == id).FirstOrDefault();
                        if (subject != null)
                        {
                            subject.Grade = model.Subject.Grade;
                            subject.Teacher = model.Subject.Teacher;
                            subject.SubjectName = model.Subject.SubjectName;
                        }

                        if (!DBmodel.SubjectTBs.Any(x => x.Teacher == model.Subject.Teacher && x.Grade == model.Subject.Grade && x.SubjectName == model.Subject.SubjectName))
                        {
                            DBmodel.Entry(subject).State = EntityState.Modified;
                            DBmodel.SaveChanges();

                            model_subject.subject1 = model.Subject.SubjectName;

                            model_grade.grade = model.Subject.Grade;
                            model_grade.teacher_id = model.Subject.Teacher;
                            model_grade.grade_id = DBmodel.GradeLists.Find(model.Subject.Grade).ID;

                            model_teacher.teacher_id = model.Subject.Teacher;

                            DBmodel.Entry(model_subject).State = EntityState.Modified;
                            DBmodel.Entry(model_grade).State = EntityState.Modified;
                            DBmodel.Entry(model_teacher).State = EntityState.Modified;
                            DBmodel.SaveChanges();

                            return RedirectToAction("SubjectList");
                        }
                        else
                        {
                            ViewBag.DuplicateMessage = "Subject Details already exist.";
                            ViewBag.IsError = true;
                        }

                    }

                }

                catch (Exception ex)
                {

                }

                model.Teachers = DBmodel.TeacherLists.ToList<TeacherList>();
                model.Grades = DBmodel.GradeLists.ToList<GradeList>();
            }

            return View(model);
        }

        // GET: Subject/Details/
        public ActionResult Details(int id)
        {
            using (DBmodel DBmodel = new DBmodel())
            {
                SubjectDetailsViewModel model = new SubjectDetailsViewModel();
                try
                {
                    var subject = DBmodel.SubjectTBs.Where(x => x.SubjectCode == id).FirstOrDefault();
                    var subDetails = DBmodel.SubjectDetails.Where(x => x.SubjectID == id).FirstOrDefault();
                    model.Subject = new SubjectViewModel
                    {
                        Grade = subject.GradeList.Grade,
                        TeacherName = subject.TeacherList.teacher_name,
                        SubjectCode = subject.SubjectCode,
                        SubjectName = subject.SubjectName
                    };
                    if (subDetails != null)
                    {
                        model.Subject.Hall = subDetails.Hall;
                        model.Subject.Day = subDetails.Day;
                        model.Subject.StartTime = subDetails.StartTime;
                        model.Subject.EndTime = subDetails.EndTime;
                    }
                }
                catch (Exception ex)
                {

                }
                return View(model);
            }
        }


        // GET: Subject/Delete/
        public ActionResult Delete(int id)
        {
            SubjectDetailsViewModel model = new SubjectDetailsViewModel();
            using (DBmodel DBmodel = new DBmodel())
            {
                model.Subject = DBmodel.SubjectTBs.Where(x => x.SubjectCode == id).Select(s => new SubjectViewModel
                {
                    Grade = s.Grade,
                    SubjectCode = s.SubjectCode,
                    SubjectName = s.SubjectName,
                    TeacherName = s.TeacherList.teacher_name
                }).FirstOrDefault();

                model.Teachers = DBmodel.TeacherLists.ToList<TeacherList>();
                model.Grades = DBmodel.GradeLists.ToList<GradeList>();

                return View(model);
            }
        }

        // POST: Subject/Delete/
        [HttpPost]
        public ActionResult Delete(int id, SubjectTB subject)
        {
            try
            {
                using (DBmodel DBmodel = new DBmodel())
                {
                    subject model_subject = DBmodel.subjects.Find(id);
                    teacher_grade model_grade = DBmodel.teacher_grade.Find(id);
                    teacher_subject model_teacher = DBmodel.teacher_subject.Find(id);

                    SubjectDetail subDetail = new SubjectDetail();
                    subject = DBmodel.SubjectTBs.Where(x => x.SubjectCode == id).FirstOrDefault();
                    if (DBmodel.SubjectDetails.Any(x => x.SubjectID == id))
                    {
                        subDetail = DBmodel.SubjectDetails.Where(x => x.SubjectID == id).FirstOrDefault();
                        DBmodel.SubjectDetails.Remove(subDetail);

                    }
                    DBmodel.SubjectTBs.Remove(subject);
                    DBmodel.SaveChanges();

                    string grade = model_grade.grade;
                    int grade2 = model_grade.grade_id;
                    int grade3 = model_grade.subjectCode;
                    int grade4 = model_grade.teacher_id;

                    DBmodel.teacher_grade.Remove(model_grade);
                   // DBmodel.subjects.Remove(model_subject);
                   // DBmodel.teacher_subject.Remove(model_teacher);
                    DBmodel.SaveChanges();

                }
                return RedirectToAction("SubjectList");
            }
            catch
            {
                return View();
            }
        }

        // GET : Student View
        public ActionResult StudentSubjectList()
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.SubjectTBs.ToList());
            }
        }
        public ActionResult Reports(string ReportType)
        {
            DBmodel db = new DBmodel();
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/SubjectReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SubjectTBDataSet";
            reportDataSource.Value = db.SubjectTBs.ToList();
            localReport.DataSources.Add(reportDataSource);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtenction;
            if (reportType == "Excel")
            {
                fileNameExtenction = "xlsx";
            }
            else if (reportType == "Word")
            {
                fileNameExtenction = "docx";
            }
            else if (reportType == "PDF")
            {
                fileNameExtenction = "pdf";
            }
            else
            {
                fileNameExtenction = "jpg";
            }
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtenction, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment:filename = subject_report." + fileNameExtenction);
            return File(renderedByte, fileNameExtenction);

        }
    }
}