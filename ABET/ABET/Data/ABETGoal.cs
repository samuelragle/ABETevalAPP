using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ABET.Data
{
    class ABETGoal:ISerializable
    {
        private string goal { get; set; }
        private string description { get; set; }
        private int id { get; set; }

        public ABETGoal(string goal, string description, int id)
        {
            this.goal = goal;
            this.description = description;
            this.id = id;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("goal", goal, typeof(string));
            info.AddValue("desc", description, typeof(string));
            info.AddValue("id", id, typeof(int));
        }
        public ABETGoal(SerializationInfo info, StreamingContext context)
        {
            goal = (string) info.GetValue("goal", typeof(string));
            description = (string) info.GetValue("desc", typeof(string));
            id = (int) info.GetValue("id", typeof(int));
        }
    }
}
