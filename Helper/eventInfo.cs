using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class eventInfo
    {
        public string eventType { get; set; }
        public string message { get; set; }
        public string fromWhere { get; set; }
        public bool status { get; set; }
        public eventInfo(string eventType, string message, string fromWhere, bool status)
        {
            this.eventType = eventType;
            this.message = message;
            this.fromWhere = fromWhere;
            this.status = status;
        }
    }
}
