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
    public class StaffNoticesController : Controller
    {
        private DBmodel db = new DBmodel();

        // GET: StaffNotices
        public async Task<ActionResult> Index()
        {
            return View(await db.StaffNotices.ToListAsync());
        }
        public async Task<ActionResult> Home()
        {
            return View(await db.StaffNotices.ToListAsync());
        }

        // GET: StaffNotices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffNotice staffNotice = await db.StaffNotices.FindAsync(id);
            if (staffNotice == null)
            {
                return HttpNotFound();
            }
            return View(staffNotice);
        }
        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/NoticeReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "NoticeDataSet";
            reportDataSource.Value = db.StaffNotices.ToList();
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
            Response.AddHeader("content-disposition", "attachment:filename=notice_report." + fileNameExtention);
            return File(renderedByte, fileNameExtention);




        }

        // GET: StaffNotices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffNotices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Notice,Type")] StaffNotice staffNotice)
        {
            if (ModelState.IsValid)
            {
                db.StaffNotices.Add(staffNotice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(staffNotice);
        }

        // GET: StaffNotices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffNotice staffNotice = await db.StaffNotices.FindAsync(id);
            if (staffNotice == null)
            {
                return HttpNotFound();
            }
            return View(staffNotice);
        }

        // POST: StaffNotices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Notice,Type")] StaffNotice staffNotice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffNotice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(staffNotice);
        }

        // GET: StaffNotices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffNotice staffNotice = await db.StaffNotices.FindAsync(id);
            if (staffNotice == null)
            {
                return HttpNotFound();
            }
            return View(staffNotice);
        }

        // POST: StaffNotices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StaffNotice staffNotice = await db.StaffNotices.FindAsync(id);
            db.StaffNotices.Remove(staffNotice);
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
