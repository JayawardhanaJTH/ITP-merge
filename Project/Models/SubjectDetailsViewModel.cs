using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class SubjectDetailsViewModel
    {
        public SubjectViewModel Subject { get; set; }
        public List<TeacherList> Teachers { get; set; }
        public List<GradeList> Grades { get; set; }
    }
}