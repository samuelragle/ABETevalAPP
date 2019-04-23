using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    public class Semester
    {
        internal string semester { get; set; }
        internal int year { get; set; }
        internal int id { get; set; }

        public Semester(string semester, int year, int id)
        {
            this.semester = semester;
            this.year = year;
            this.id = id;
        }
        public override string ToString()
        {
            return semester + " " + year;
        }
    }
}
