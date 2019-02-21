using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ABET.Data
{
    public class Session
    {
        public static List<Class> Classes;
        private List<Class> SelectedClasses;
        public List<Survey> sClassSurveys;

        public Session()
        {
            // TODO: pull all classes from db and set to classes
            Classes = new List<Class>();
            SelectedClasses = new List<Class>();
            sClassSurveys = new List<Survey>();
        }
        public void UpdateClasses(Semester semester)
        {
            sClassSurveys.Clear();
            SelectedClasses.Clear();
            // TODO: pull all classes by semster
            Classes = new List<Class>();
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
    }
}
