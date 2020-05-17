using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;

namespace Staff_Management1.Controllers
{
    public class CleanerController : Controller
    {

        public ActionResult CleanerList()
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Cleaners.ToList());
            }
        }

        public ActionResult Reports(string ReportType)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                LocalReport localreport = new LocalReport();
                localreport.ReportPath = Server.MapPath("~/Reports/CleanersReport.rdlc");

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "CleanersDataSet";
                reportDataSource.Value = dbModel.Cleaners.ToList();
                localreport.DataSources.Add(reportDataSource);
                string reportType = ReportType;
                string mimeType;
                string encoding;
                string fileNameExtension;

                if (reportType == "Excel")
                {
                    fileNameExtension = "xlsx";
                }
                else if (reportType == "Word")
                {
                    fileNameExtension = "docx";
                }
                else if (reportType == "PDF")
                {
                    fileNameExtension = "pdf";
                }
                else
                {
                    fileNameExtension = "jpg";
                }

                string[] streams;
                Warning[] warnings;
                byte[] renderByte;
                renderByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                Response.AddHeader("content-disposition", "attachment-filename = cleaners_report." + fileNameExtension);
                return File(renderByte, fileNameExtension);

            }

        }
        // GET: Cleaner
        public ActionResult Index()
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Cleaners.ToList());
            }
        }

        // GET: Cleaner/Details/5
        public ActionResult Details(int id)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Cleaners.Where(x => x.UserID == id).FirstOrDefault());
            }
        }

        // GET: Cleaner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cleaner/Create
        [HttpPost]
        public ActionResult Create(Cleaner cleaner)
        {
            try
            {
                // TODO: Add insert logic here
                using (DBmodel dbModel = new DBmodel())
                {
                    if (dbModel.StudentTBs.Any(m => m.username == cleaner.Username)
                        && dbModel.Teachers.Any(m => m.Username == cleaner.Username)
                        && dbModel.Cleaners.Any(m => m.Username == cleaner.Username)
                        && dbModel.Offices.Any(m => m.Username == cleaner.Username))
                    {
                        ViewBag.Error = "User name already exist! please use another one";
                        return View(cleaner);
                    }
                    else
                    {
                        dbModel.Cleaners.Add(cleaner);
                        dbModel.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cleaner/Edit/5
        public ActionResult Edit(int id)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Cleaners.Where(x => x.UserID == id).FirstOrDefault());
            }
        }

        // POST: Cleaner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Cleaner cleaner)
        {
            try
            {
                // TODO: Add update logic here
                using (DBmodel dbModel = new DBmodel())
                {
                    dbModel.Entry(cleaner).State = EntityState.Modified;
                    dbModel.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cleaner/Delete/5
        public ActionResult Delete(int id)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Cleaners.Where(x => x.UserID == id).FirstOrDefault());
            }
        }

        // POST: Cleaner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel dbModel = new DBmodel())
                {
                    Cleaner cleaner = dbModel.Cleaners.Where(x => x.UserID == id).FirstOrDefault();
                    dbModel.Cleaners.Remove(cleaner);
                    dbModel.SaveChanges();
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
