using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    public class Semester
    {
        private string semester { get; set; }
        private int year { get; set; }
        private int id { get; set; }
        public string semesterYear { get; set; }

        public Semester(string semester, int year, int id)
        {
            this.semester = semester;
            this.year = year;
            this.id = id;
            semesterYear = String.Format(semester + " " + year);
        }
    }
}
