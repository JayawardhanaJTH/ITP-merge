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
    public class OfficeController : Controller
    {

        public ActionResult OfficesList()
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Offices.ToList());
            }
        }

        public ActionResult Reports(string ReportType)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                LocalReport localreport = new LocalReport();
                localreport.ReportPath = Server.MapPath("~/Reports/OfficesReport.rdlc");

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "OfficesDataSet";
                reportDataSource.Value = dbModel.Offices.ToList();
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
                Response.AddHeader("content-disposition", "attachment-filename = offices_report." + fileNameExtension);
                return File(renderByte, fileNameExtension);

            }

        }


        // GET: Office/Index
        public ActionResult Index()
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Offices.ToList());
            }
        }

        // GET: Office/Details/5
        public ActionResult Details(int id)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Offices.Where(x => x.UserID == id).FirstOrDefault());
            }

        }

        // GET: Office/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Office/Create
        [HttpPost]
        public ActionResult Create(Office office)
        {
            try
            {
                using (DBmodel dbModel = new DBmodel())
                {
                    if (dbModel.StudentTBs.Any(m => m.username == office.Username)
                        && dbModel.Teachers.Any(m => m.Username == office.Username)
                        && dbModel.Cleaners.Any(m => m.Username == office.Username)
                        && dbModel.Offices.Any(m => m.Username == office.Username))
                    {
                        ViewBag.Error = "User name already exist! please use another one";
                        return View(office);
                    }
                    else
                    {
                        dbModel.Offices.Add(office);
                        dbModel.SaveChanges();
                    }
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Office/Edit/5
        public ActionResult Edit(int id)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Offices.Where(x => x.UserID == id).FirstOrDefault());
            }
        }

        // POST: Office/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Office office)
        {
            try
            {
                using (DBmodel dbModel = new DBmodel())
                {
                    office.UserID = id;
                    dbModel.Entry(office).State = EntityState.Modified;
                    dbModel.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Office/Delete/5
        public ActionResult Delete(int id)
        {
            using (DBmodel dbModel = new DBmodel())
            {
                return View(dbModel.Offices.Where(x => x.UserID == id).FirstOrDefault());
            }

        }

        // POST: Office/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DBmodel dbModel = new DBmodel())
                {
                    Office office = dbModel.Offices.Where(x => x.UserID == id).FirstOrDefault();
                    dbModel.Offices.Remove(office);
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
