using System;
using System.Collections.Generic;
using System.Text;

namespace ABET.Data
{
    class SurveyClass
    {
        private Section section;
        private int id;

        public SurveyClass(Section section, int id)
        {
            this.section = section;
            this.id = id;
        }
    }
}
