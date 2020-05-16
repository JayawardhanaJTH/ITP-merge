using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Data.Entity.Validation;

namespace TutionClassManagmentSystem.Controllers
{
    public class PayRollController : Controller
    {
        public ActionResult Payment()
        {
            return View();
        }

        // GET: PayRoll
        public ActionResult TeacherList(String searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Teachers.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }


        /// <summary>
        /// GET:Add Teacher
        /// </summary>
        /// <returns></returns>
        public ActionResult Add_teacher_salary(int id)
        {


            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            string month = DateTime.Now.ToString("MMMM");

            DBmodel payrollentities = new DBmodel();
            Teacher teacher = payrollentities.
                        Teachers.SingleOrDefault(x => x.UserID == id);

            TeacherSalaryView teachersalaryView = new TeacherSalaryView();
            teachersalaryView.TeacherID = teacher.UserID;
            teachersalaryView.Name = teacher.Name;
            teachersalaryView.Month = month;
            teachersalaryView.Date_Time = date;

            return View(teachersalaryView);
        }


        ////
        ///POST: Add Teacher
        ///
        [HttpPost]
        public ActionResult Add_teacher_salary(int id, TeacherSalary teacherSalary)
        {
            try
            {
                using (DBmodel payrollentities = new DBmodel())
                {
                    payrollentities.TeacherSalaries.Add(teacherSalary);
                    payrollentities.SaveChanges();
                }

                return RedirectToAction("Salary_details_teacher");
            }
            catch
            {
                return View();
            }

        }



        public ActionResult Salary_details_teacher(string searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.TeacherSalaries.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }

        public ActionResult Salary_delete_teacher(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.TeacherSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Salary_delete_teacher(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel payrollentities = new DBmodel())
                {
                    TeacherSalary teacherSalary = payrollentities.TeacherSalaries.Where(x => x.SalaryId == id).FirstOrDefault();
                    payrollentities.TeacherSalaries.Remove(teacherSalary);
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Salary_details_teacher");
            }
            catch
            {
                return View();
            }


        }
        public ActionResult Salary_edit_teacher(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.TeacherSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Salary_edit_teacher(int id, TeacherSalary teacherSalary)
        {
            try
            {

                using (DBmodel payrollentities = new DBmodel())
                {

                    payrollentities.Entry(teacherSalary).State = EntityState.Modified;
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Salary_details_teacher");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Salary_details_single_teacher(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.TeacherSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }

        }

        /// <summary>
        /// Student Fee
        /// </summary>
        /// <param name="searching"></param>
        /// <returns></returns>
        public ActionResult StudentList(String searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.StudentTBs.Where(x => x.fullname.StartsWith(searching) || searching == null).ToList());

            }
        }

        /// <summary>
        /// GET:Add Student
        /// </summary>
        /// <returns></returns>
        public ActionResult Add_student_fee(int id)
        {


            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            string month = DateTime.Now.ToString("MMMM");

            DBmodel payrollentities = new DBmodel();
            StudentTB studentTB = payrollentities.
                        StudentTBs.SingleOrDefault(x => x.sid == id);

            StudentFeeView studentFeeView = new StudentFeeView();
            studentFeeView.StudentID = studentTB.sid;
            studentFeeView.Name = studentTB.fullname;
            studentFeeView.Subject = studentTB.subject;
            studentFeeView.Month = month;
            studentFeeView.Date_Time = date;

            return View(studentFeeView);
        }

        ////
        ///POST: Add Student
        ///
        [HttpPost]
        public ActionResult Add_student_fee(int id, StudentFee studentFee)
        {
            try
            {
                using (DBmodel payrollentities = new DBmodel())
                {
                    payrollentities.StudentFees.Add(studentFee);
                    payrollentities.SaveChanges();
                }

                return RedirectToAction("Fee_details_students");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Fee_details_students(string searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.StudentFees.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }

        public ActionResult Fee_delete_student(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.StudentFees.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Fee_delete_student(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel payrollentities = new DBmodel())
                {
                    StudentFee studentFee = payrollentities.StudentFees.Where(x => x.SalaryId == id).FirstOrDefault();
                    payrollentities.StudentFees.Remove(studentFee);
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Fee_details_students");
            }
            catch
            {
                return View();
            }


        }
        public ActionResult Fee_edit_student(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.StudentFees.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Fee_edit_student(int id, StudentFee studentFee)
        {
            try
            {

                using (DBmodel payrollentities = new DBmodel())
                {

                    payrollentities.Entry(studentFee).State = EntityState.Modified;
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Fee_details_students");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Fee_details_single_student(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.StudentFees.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }

        public ActionResult CleanerList(String searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Cleaners.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }
        /// <summary>
        /// GET:Add Cleaner
        /// </summary>
        /// <returns></returns>
        public ActionResult Add_cleaner_salary(int id)
        {


            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            string month = DateTime.Now.ToString("MMMM");

            DBmodel payrollentities = new DBmodel();
            Cleaner cleaner = payrollentities.
                        Cleaners.SingleOrDefault(x => x.UserID == id);

            CleanerSalaryView cleanerSalaryView = new CleanerSalaryView();
            cleanerSalaryView.CleanerID = cleaner.UserID;
            cleanerSalaryView.Name = cleaner.Name;
            cleanerSalaryView.Month = month;
            cleanerSalaryView.Date_Time = date;

            return View(cleanerSalaryView);
        }

        ////
        ///POST: Add Cleaner
        ///
        [HttpPost]
        public ActionResult Add_cleaner_salary(int id, CleanerSalary cleanerSalary)
        {
            try
            {
                using (DBmodel payrollentities = new DBmodel())
                {
                    payrollentities.CleanerSalaries.Add(cleanerSalary);
                    payrollentities.SaveChanges();
                }

                return RedirectToAction("Salary_details_cleaner");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Salary_details_cleaner(string searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.CleanerSalaries.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }

        public ActionResult Salary_delete_cleaner(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.CleanerSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Salary_delete_cleaner(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel payrollentities = new DBmodel())
                {
                    CleanerSalary cleanerSalary = payrollentities.CleanerSalaries.Where(x => x.SalaryId == id).FirstOrDefault();
                    payrollentities.CleanerSalaries.Remove(cleanerSalary);
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Salary_details_cleaner");
            }
            catch
            {
                return View();
            }


        }
        public ActionResult Salary_edit_cleaner(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.CleanerSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Salary_edit_cleaner(int id, CleanerSalary cleanerSalary)
        {
            try
            {

                using (DBmodel payrollentities = new DBmodel())
                {

                    payrollentities.Entry(cleanerSalary).State = EntityState.Modified;
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Salary_details_cleaner");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Salary_details_single_cleaner(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.CleanerSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }

        }


        ////
        ///Office
        ///
        public ActionResult OfficeList(String searching)
        {

            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Offices.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }
        public ActionResult Add_office_salary(int id)
        {


            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            string month = DateTime.Now.ToString("MMMM");

            DBmodel payrollentities = new DBmodel();
            Office office = payrollentities.
                         Offices.SingleOrDefault(x => x.UserID == id);

            OfficeSalaryView officeSalaryView = new OfficeSalaryView();
            officeSalaryView.OfficeID = office.UserID;
            officeSalaryView.Name = office.Name;
            officeSalaryView.Month = month;
            officeSalaryView.Date_Time = date;

            return View(officeSalaryView);
        }

        ////
        ///POST: Add office
        ///
        [HttpPost]
        public ActionResult Add_office_salary(int id, OfficeSalary officeSalary)
        {
            try
            {
                using (DBmodel payrollentities = new DBmodel())
                {
                    payrollentities.OfficeSalaries.Add(officeSalary);
                    payrollentities.SaveChanges();
                }

                return RedirectToAction("Salary_details_office");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Salary_details_office(string searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.OfficeSalaries.Where(x => x.Name.StartsWith(searching) || searching == null).ToList());
            }
        }

        public ActionResult Salary_delete_office(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.OfficeSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Salary_delete_office(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel payrollentities = new DBmodel())
                {
                    OfficeSalary officeSalary = payrollentities.OfficeSalaries.Where(x => x.SalaryId == id).FirstOrDefault();
                    payrollentities.OfficeSalaries.Remove(officeSalary);
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Salary_details_office");
            }
            catch
            {
                return View();
            }


        }
        public ActionResult Salary_edit_office(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.OfficeSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Salary_edit_office(int id, OfficeSalary officeSalary)
        {
            try
            {

                using (DBmodel payrollentities = new DBmodel())
                {

                    payrollentities.Entry(officeSalary).State = EntityState.Modified;
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("Salary_details_office");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Salary_details_single_office(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.OfficeSalaries.Where(x => x.SalaryId == id).FirstOrDefault());
            }

        }


        //Bill view
        // GET: Bills
        public ActionResult BillList(String searching)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Biils.Where(x => x.Payment_Name.StartsWith(searching) || searching == null).ToList());
            }
        }

        // GET: Bills/Details
        public ActionResult Details_bill(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Biils.Where(x => x.BillId == id).FirstOrDefault());
            }
        }

        // GET: Bills/Create
        public ActionResult CreateAccount_bill()
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            string month = DateTime.Now.ToString("MMMM");

            DBmodel payrollentities = new DBmodel();

            Biil biil = new Biil();
            biil.Date = date;
            return View(biil);
        }

        // POST: Bills/Create
        [HttpPost]
        public ActionResult CreateAccount_bill(Biil biil)
        {

            try
            {
                using (DBmodel payrollentities = new DBmodel())
                {

                    payrollentities.Biils.Add(biil);
                    payrollentities.SaveChanges();


                }
                return RedirectToAction("BillList");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bills/Edit/5
        public ActionResult EditDetails_bill(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Biils.Where(x => x.BillId == id).FirstOrDefault());
            }
        }

        // POST: Bills/Edit/5
        [HttpPost]
        public ActionResult EditDetails_bill(int id, Biil biil)
        {
            try
            {
                using (DBmodel payrollentities = new DBmodel())
                {
                    payrollentities.Entry(biil).State = EntityState.Modified;
                    payrollentities.SaveChanges();
                }

                return RedirectToAction("BillList");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bills/Delete/5
        public ActionResult DeleteDetails_bill(int id)
        {
            using (DBmodel payrollentities = new DBmodel())
            {
                return View(payrollentities.Biils.Where(x => x.BillId == id).FirstOrDefault());
            }
        }

        // POST: Bills/Delete/5
        [HttpPost]
        public ActionResult DeleteDetails_bill(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel payrollentities = new DBmodel())
                {
                    Biil biil = payrollentities.Biils.Where(x => x.BillId == id).FirstOrDefault();
                    payrollentities.Biils.Remove(biil);
                    payrollentities.SaveChanges();
                }
                return RedirectToAction("BillList");
            }
            catch
            {
                return View();
            }
        }

        //Report//
        public ActionResult IncomeOutcome(string ReportType)
        {
            DBmodel payrollentities = new DBmodel();

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/IncomeOutcome.rdlc");

            ReportDataSource reportDataSource1 = new ReportDataSource();
            reportDataSource1.Name = "Teacher";
            reportDataSource1.Value = payrollentities.TeacherSalaries.ToList();

            ReportDataSource reportDataSource2 = new ReportDataSource();
            reportDataSource2.Name = "Student";
            reportDataSource2.Value = payrollentities.StudentFees.ToList();

            ReportDataSource reportDataSource3 = new ReportDataSource();
            reportDataSource3.Name = "Office";
            reportDataSource3.Value = payrollentities.OfficeSalaries.ToList();

            ReportDataSource reportDataSource4 = new ReportDataSource();
            reportDataSource4.Name = "Cleaner";
            reportDataSource4.Value = payrollentities.CleanerSalaries.ToList();

            ReportDataSource reportDataSource5 = new ReportDataSource();
            reportDataSource5.Name = "Bill";
            reportDataSource5.Value = payrollentities.Biils.ToList();

            localReport.DataSources.Add(reportDataSource1);
            localReport.DataSources.Add(reportDataSource2);
            localReport.DataSources.Add(reportDataSource3);
            localReport.DataSources.Add(reportDataSource4);
            localReport.DataSources.Add(reportDataSource5);

            string reportType = ReportType;
            string fileNameExtension = null;
            if (reportType == "PDF")
            {
                fileNameExtension = "pdf";
            }
            byte[] renderedByte;
            renderedByte = localReport.Render(reportType);
            Response.AddHeader("content-disposition", "attachment,Filename_report." + fileNameExtension);

            return File(renderedByte, fileNameExtension);
        }


    }
}