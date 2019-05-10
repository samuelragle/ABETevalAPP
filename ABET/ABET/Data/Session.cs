using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using ABET;
using System.Data;

namespace ABET.Data
{
    public class Session
    {
        public List<Section> Classes;
        private List<Section> SelectedClasses;
        public List<Semester> Semesters;
        public List<Course> Courses;
        private static SqlConnection conn;

        public Session(string connString, string semester)
        {
            SelectedClasses = new List<Section>();
            Semesters = new List<Semester>();
            Classes = new List<Section>();
            Courses = new List<Course>();
            conn = new SqlConnection(connString);
            conn.Open();

        }
        internal long InsertSurveyClass(int sectionNum)
        {
            string query = "INSERT INTO surveys_class VALUES (@section); SELECT SCOPE_IDENTITY(); ";
            long id = -1;
            using (SqlCommand command = new SqlCommand(query, conn))
            {

                command.Parameters.AddWithValue("@section", sectionNum);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = (long)reader.GetValue(0);
                        System.Diagnostics.Debug.WriteLine(id);
                    }
                }
            }
            return id;
        }
        internal bool InsertCourse(string department, int courseNum, string courseTitle)
        {
            string query = "INSERT INTO course VALUES (@department,@courseNum,@courseTitle)";
            using (SqlCommand command = new SqlCommand(query, conn))
            {

                command.Parameters.AddWithValue("@department", department);
                command.Parameters.AddWithValue("@courseNum", courseNum);
                command.Parameters.AddWithValue("@courseTitle", courseTitle);

                int result = command.ExecuteNonQuery();
                if (result < 0)
                    return false;
            }

            PullCourses();
            return true;
        }
        internal bool InsertSemester(string semester, int year)
        {
            string query = "INSERT INTO semesters VALUES (@semester,@year)";
            using (SqlCommand command = new SqlCommand(query, conn))
            {
               
                command.Parameters.AddWithValue("@semester", semester);
                command.Parameters.AddWithValue("@year", year);

                int result = command.ExecuteNonQuery();
                if (result < 0)
                    return false;
            }

            PullSemesters();
            return true;
        }
        internal void PullSections(Semester semester)
        {
            Classes.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM section ");
            sb.Append($"WHERE semester = {semester.id}");
            string sql = sb.ToString();
            List<Section> tempSections = new List<Section>();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int courseID = (int)reader.GetValue(0);
                        int sectionNum = (int)reader.GetValue(2);
                        int studentCount = (int)reader.GetValue(3);
                        int id = (int)reader.GetValue(4);
                        Section s = new Section(new Course(null, -1, null, courseID), semester, sectionNum, studentCount, id);
                        tempSections.Add(s);

                    }
                }
            }
            foreach(Section s in tempSections)
            {
                Course c = PullCourse(s.Course.ID);
                s.Course = c;
                s.SurveyClasses = PullSurveyClasses(s);
                //List<Survey> surveys = PullSurveys(s);
                Classes.Add(s);

            }
        }

        private List<SurveyClass> PullSurveyClasses(Section s)
        {
            List<SurveyClass> results = new List<SurveyClass>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM surveys_class ");
            sb.Append($"WHERE section = {s.ID}");
            string sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new SurveyClass(s, (long)reader.GetValue(1)));
                    }
                }
            }
            foreach (SurveyClass sc in results)
            {
                List<Survey> surveys = new List<Survey>();
                List<short> ABETIDs = new List<short>();
                sb = new StringBuilder();
                sb.Append("SELECT * FROM surveys ");
                sb.Append($"WHERE id = {sc.id} ");
                sb.Append($"ORDER BY id");
                sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            surveys.Add(new Survey(sc, null, (byte)reader.GetValue(2)));
                            ABETIDs.Add((short)reader.GetValue(1));
                        }
                    }
                }
                for (int i = 0; i < ABETIDs.Count; ++i)
                {
                    surveys[i].goal = PullGoal(ABETIDs[i]);
                }
                sc.Surveys.AddRange(surveys);
            }
            return results;
        }

        private ABETGoal PullGoal(short ABETID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM abetGoals ");
            sb.Append($"WHERE id = {ABETID}");
            string sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string goal = (string)reader.GetValue(0);
                        string desc = (string)reader.GetValue(1);
                        return new ABETGoal(goal, desc, ABETID);
                    }
                }
            }
            return null;
        }

        internal Course PullCourse(int courseID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM course ");
            sb.Append($"WHERE id = {courseID}");
            string sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string department  = (string)reader.GetValue(0);
                        int courseNum = (int)reader.GetValue(1);
                        string courseTitle = (string)reader.GetValue(2);
                        return new Course(department, courseNum, courseTitle, courseID);

                    }
                }
            }
            return null;
        }
        internal void PullCourses()
        {
            Courses.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM course ");
            string sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Course c = new Course((string)reader.GetString(0), (int)reader.GetValue(1), (string)reader.GetValue(2), (int)reader.GetValue(3));
                        Courses.Add(c);
                    }
                }
            }
        }
        internal void PullSemesters()
        {
            Semesters.Clear();
            // TODO: pull all classes by semster
            /* 
             * To pull a Class, pull the associated Section and list of Surveys
             * For each Section pulled, we must pull a Course and a Semester
             * 
             * To pull a Survey, pull the SurveyClass and ABETGoal associated
             */
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM semesters ");
            string sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Semester s = new Semester(reader.GetString(0), (int)reader.GetValue(1), (int)reader.GetValue(2));
                        Semesters.Add(s);
                    }
                }
            }
        }
    }
}
