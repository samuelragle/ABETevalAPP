using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ABET.Data
{
    public class ABETGoal:ISerializable
    {
        private string goal { get; set; }
        private string description { get; set; }
        private short id { get; set; }

        public ABETGoal(string goal, string description, short id)
        {
            this.goal = goal;
            this.description = description;
            this.id = id;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("goal", goal, typeof(string));
            info.AddValue("desc", description, typeof(string));
            info.AddValue("id", id, typeof(short));
        }
        public ABETGoal(SerializationInfo info, StreamingContext context)
        {
            goal = (string) info.GetValue("goal", typeof(string));
            description = (string) info.GetValue("desc", typeof(string));
            id = (short) info.GetValue("id", typeof(short));
        }
        public override string ToString()
        {
            return goal + ": " + description;
        }
    }
}
