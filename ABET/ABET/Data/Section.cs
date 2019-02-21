using System;
using System.Collections.Generic;
using System.Text;
namespace ABET.Data
{
    public class Section
    {
        private Course Course { get; set; }
        private Semester Semester { get; set; }
        private int SectionNum { get; set; }
        private int StudentCount { get; set; }
        private int ID { get; set; }

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
