using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    public class Survey
    {
        private SurveyClass surveyClass;
        private ABETGoal goal;
        internal int response;

        Survey(SurveyClass surveyClass, ABETGoal goal, int response)
        {
            this.surveyClass = surveyClass;
            this.goal = goal;
            this.response = response;
        }
        
    }
}
