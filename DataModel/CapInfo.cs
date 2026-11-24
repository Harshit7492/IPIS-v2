using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class CapModel
    {
        public string Identifier { get; set; }
        public string AlarmType { get; set; }
        public string Message { get; set; }
        public string Audio_Path { get; set; }
        public string AudioOne { get; set; }
        public string AudioTwo { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public int EvenStatus { get; set; } = 0;
        public DateTime? PauseTime { get; set; }
        public int Displaycount { get; set; } = 0;
        public int AnnounceCount { get; set; } = 0;
        public string Validatetime { get; set; }
    }
}
