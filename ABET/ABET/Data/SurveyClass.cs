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
        public override string ToString()
        {
            return id.ToString();
        }
        public override bool Equals(object obj)
        {
            var @class = obj as SurveyClass;
            return @class != null &&
                   id == @class.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
