using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Microsoft.Reporting.WebForms;

namespace FeedbackAndNotices.Controllers
{
    public class UserFeedbacksController : Controller
    {
        private DBmodel db = new DBmodel();

        // GET: UserFeedbacks
        public async Task<ActionResult> Index()
        {

            return View(await db.UserFeedbacks.ToListAsync());


        }

        public async Task<ActionResult> TeacherPage()
        {


            var Teacher = db.UserFeedbacks.Include(u => u.Teacher);
            return View(await db.UserFeedbacks.ToListAsync());


        }
        [HttpPost]
        public ActionResult TeacherPage(String searching)
        {

            var Teacher = db.UserFeedbacks.ToList().Where(t => t.Teacher.StartsWith(searching));
            return View(Teacher);


        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/FeedbackReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "FeedbackDataSet";
            reportDataSource.Value = db.UserFeedbacks.ToList();
            localreport.DataSources.Add(reportDataSource);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtention;
            if (reportType == "Word")
            {
                fileNameExtention = "docx"; ;
            }
            else
            {
                fileNameExtention = "pdf";
            }

            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtention, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment:filename=feedback_report." + fileNameExtention);
            return File(renderedByte, fileNameExtention);




        }




        // GET: UserFeedbacks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFeedback userFeedback = await db.UserFeedbacks.FindAsync(id);
            if (userFeedback == null)
            {
                return HttpNotFound();
            }
            return View(userFeedback);
        }

        // GET: UserFeedbacks/Create
        public ActionResult Create()
        {

            List<TeacherList> list = db.TeacherLists.ToList();
            ViewBag.list = new SelectList(list,"teacher_name", "teacher_name");

            return View();
        }

        // POST: UserFeedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Name,Teacher,Comment,Email")] UserFeedback userFeedback)
        {

            if (ModelState.IsValid)
            {
                db.UserFeedbacks.Add(userFeedback);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userFeedback);
        }

        // GET: UserFeedbacks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFeedback userFeedback = await db.UserFeedbacks.FindAsync(id);
            if (userFeedback == null)
            {
                return HttpNotFound();
            }
            List<TeacherList> list = db.TeacherLists.ToList();

            ViewBag.list = new SelectList(list, "teacher_name", "teacher_name");
            return View(userFeedback);
        }

        // POST: UserFeedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Name,Teacher,Comment,Email")] UserFeedback userFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userFeedback).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userFeedback);
        }

        // GET: UserFeedbacks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFeedback userFeedback = await db.UserFeedbacks.FindAsync(id);
            if (userFeedback == null)
            {
                return HttpNotFound();
            }
            return View(userFeedback);
        }

        // POST: UserFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserFeedback userFeedback = await db.UserFeedbacks.FindAsync(id);
            db.UserFeedbacks.Remove(userFeedback);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
