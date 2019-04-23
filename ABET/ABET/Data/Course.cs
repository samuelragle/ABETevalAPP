using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    public class Course
    {
        private string Department { get; set; }
        private int CourseNum { get; set; }
        internal string CourseTitle { get; set; }
        internal int ID { get; set; }
        
        public Course(string department, int courseNum, string courseTitle, int id)
        {
            Department = department;
            CourseNum = courseNum;
            CourseTitle = courseTitle;
            ID = id;
        }
    }
}
