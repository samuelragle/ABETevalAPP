using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    public class SurveyClass
    {
        private Section section;
        internal long id;

        public SurveyClass(Section section, long id)
        {
            this.section = section;
            this.id = id;
        }
    }
}
