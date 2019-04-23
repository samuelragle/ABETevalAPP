using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ABET.Data
{
    public class Class
    {
        private Section Section { get; set; }
        internal List<Survey> Surveys { get; set; }

        public Class(Section section, List<Survey> surveys)
        {
            Section = section;
            Surveys = surveys;
        }
        public override string ToString()
        {
            return Section.Course.CourseTitle + " " + Section.Semester.semester + " " + Section.Semester.year;
        }
    }
}
