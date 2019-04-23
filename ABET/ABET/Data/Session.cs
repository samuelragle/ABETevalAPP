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
        public List<Class> Classes;
        private List<Class> SelectedClasses;
        public List<Survey> sClassSurveys;
        public List<Semester> Semesters;
        private static SqlConnection conn;

        public Session(string connString, string semester)
        {
            SelectedClasses = new List<Class>();
            sClassSurveys = new List<Survey>();
            Semesters = new List<Semester>();
            Classes = new List<Class>();
            conn = new SqlConnection(connString);
            conn.Open();

        }
        public void UpdateClasses(string semester)
        {
            sClassSurveys.Clear();
            SelectedClasses.Clear();
        }
        public void UpdateSurveys()
        {
            sClassSurveys.Clear();
            foreach (Class c in SelectedClasses)
            {
                sClassSurveys.AddRange(c.Surveys);
            }
        }
        public void AddSelection(Class selectedClass)
        {
            SelectedClasses.Add(selectedClass);
            UpdateSurveys();
        }
        public void RemoveSelection(Class removedClass)
        {
            SelectedClasses.Remove(removedClass);
            UpdateSurveys();
        }
        public int SurveyAverage()
        {
            int sum = 0;
            foreach (Survey s in sClassSurveys)
            {
                sum += s.response;
            }
            return sum / sClassSurveys.Count;
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
                List<Survey> surveys = PullSurveys(s);
                Classes.Add(new Class(s, surveys));

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
            return results;
        }

        internal List<Survey> PullSurveys(Section s)
        {
            List<short> ABETIDs = new List<short>();
            List<Survey> results = new List<Survey>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT id, goal, response FROM surveys sur WHERE sur.id IN ");
            sb.Append("(SELECT id FROM surveys_class sc WHERE sc.section IN ");
            sb.Append($"(SELECT id FROM section s WHERE s.id = {s.ID})) ORDER BY sur.id");
            string sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine(reader.GetValue(0) + " " + reader.GetValue(1) + " " + reader.GetValue(2));
                        SurveyClass sClass = s.SurveyClasses.Find(delegate (SurveyClass sc){ return sc.id == (long)reader.GetValue(0); });
                        results.Add(new Survey(sClass, null, (byte)reader.GetValue(2)));
                        ABETIDs.Add((short)reader.GetValue(1));
                    }
                }
            }
            for(int i = 0; i < ABETIDs.Count; ++i)
            {
                results[i].goal = PullGoal(ABETIDs[i]);
            }
            return null;
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
        internal void PullSemesters()
        {
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
