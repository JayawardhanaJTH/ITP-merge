using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class CleanerSalaryView
    {
        public int SalaryId { get; set; }
        public int CleanerID { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Month { get; set; }
        public string Date_Time { get; set; }
    }
}