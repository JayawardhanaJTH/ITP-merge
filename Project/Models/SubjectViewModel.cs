using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class SubjectViewModel
    {
        public int SubjectCode { get; set; }
        [Required(ErrorMessage = "This Field is Required.*")]
        public string SubjectName { get; set; }
        public string Grade { get; set; }
        public string Teacher { get; set; }
        public string Hall { get; set; }
        public string Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}