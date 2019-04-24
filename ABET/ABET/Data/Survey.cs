using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    public class Survey
    {
        internal SurveyClass surveyClass;
        internal ABETGoal goal;
        internal int response;

        public Survey(SurveyClass surveyClass, ABETGoal goal, int response)
        {
            this.surveyClass = surveyClass;
            this.goal = goal;
            this.response = response;
        }
    }
}
