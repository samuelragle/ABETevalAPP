using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace ABET.Data
{
    public class Session
    {
        public static List<Class> Classes;
        private List<Class> SelectedClasses;
        public List<Survey> sClassSurveys;
        private static SqlConnection conn;

        public Session(string connString, string semester)
        {
            PullClasses(semester);
            SelectedClasses = new List<Class>();
            sClassSurveys = new List<Survey>();
            conn = new SqlConnection(connString);
            
        }
        public void UpdateClasses(string semester)
        {
            sClassSurveys.Clear();
            SelectedClasses.Clear();
            PullClasses(semester);
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
        private void PullClasses(string semester)
        {
            // TODO: pull all classes by semster
            /* 
             * To pull a Class, pull the associated Section and list of Surveys
             * For each Section pulled, we must pull a Course and a Semester
             * 
             * To pull a Survey, pull the SurveyClass and ABETGoal associated
             */
            Classes = new List<Class>();
        }
    }
}
