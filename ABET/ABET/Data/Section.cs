using System;
using System.Collections.Generic;
using System.Text;
namespace ABET.Data
{
    public class Section
    {
        internal Course Course { get; set; }
        internal Semester Semester { get; set; }
        internal List<SurveyClass> SurveyClasses { get; set; }
        private int SectionNum { get; set; }
        private int StudentCount { get; set; }
        internal int ID { get; set; }

        public Section(Course course, Semester semester, int sectionNum, int studentCount, int id)
        {
            Course = course;
            Semester = semester;
            SectionNum = sectionNum;
            StudentCount = studentCount;
            ID = id;
        }
    }
}
