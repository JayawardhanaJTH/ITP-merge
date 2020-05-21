﻿using System;
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

                            tution.Timetables.Add(timetable);
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
                return View();
            }
            return RedirectToAction("TimetableList");
        }
        

        // GET: Timetable/Edit/5
        public ActionResult EditTimetable(int id)
        {
            using (DBmodel tution = new DBmodel())
            {
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
                    timetable.classId = id;
                    tution.Entry(timetable).State = EntityState.Modified;
                    tution.SaveChanges();
                    return RedirectToAction("TimetableList");
                }
                // TODO: Add update logic here

                
            }
            catch
            {
                return View("TimetableList");
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
            try
            {
                using (DBmodel tution = new DBmodel())
                {
                    Project.Models.Timetable timetable =tution.Timetables.Where(x => x.classId == id).FirstOrDefault();
                    tution.Timetables.Remove(timetable);
                    tution.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("TimetableList");
            }
            catch
            {
                return View();
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
            string fileNameExtenction = "jpg";
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render("Image", "", out mimeType, out encoding, out fileNameExtenction, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment:filename = timetable_report." + fileNameExtenction);
            return File(renderedByte, fileNameExtenction);

        }
    }


}

