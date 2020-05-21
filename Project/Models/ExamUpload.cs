using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class ExamUpload
    {
        public int grade_id { get; set; }

        [DisplayName("Grade")]
        
        public string grade1 { get; set; }
        public int subject_id { get; set; }

        [DisplayName("Subject")]
        
        public string subject1 { get; set; }
        public int ExamID { get; set; }

        [DisplayName("Start Time")]
        
        public System.DateTime StartTime { get; set; }

        [DisplayName(" End Time")]
        
        public System.DateTime EndTime { get; set; }
        
        public string TeacherName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}