using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class PingResult
    {
        public string IP { get; set; }
        public bool IsSuccess { get; set; }
        public long LatencyMs { get; set; }
        public int Attempts { get; set; }
    }
}
