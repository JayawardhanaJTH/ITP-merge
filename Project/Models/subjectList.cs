

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class subjectList
    {
        public int Id
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public bool IsSelected
        {
            get;
            set;
        }
        public List<subjectList> subjectListList
        {
            get;
            set;
        }

        public Boolean Maths
        {
            get;
            set;
        }
        public Boolean English
        {
            get;
            set;
        }
        public Boolean Science
        {
            get;
            set;
        }
    }
}