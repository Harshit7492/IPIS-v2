using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Message
    {
        public int MESSAGE_ID { get; set; }
        public string MESSAGE_CODE { get; set; }
        public string MESSAGE_ENGLISH { get; set; }
        public string MESSAGE_HINDI { get; set; }
        public string MESSAGE_REGIONAL { get; set; }
        public string SOUND_ENGLISH { get; set; }
        public string SOUND_HINDI { get; set; }
        public string SOUND_REGIONAL { get; set; }
        public int DISPLAY_STATUS { get; set; }
        public string REGIONAL_BITMAP { get; set; }
        public string MESSAGE_FOR_DISPLAY { get; set; }
        public int LIVE_STATUS { get; set; }
        public string BottomMessageFont { get; set; }

    }
   
        public class LineColourModel
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public string ColorCode { get; set; }
        }
    public class ColourStatus
    {
        public int Id { get; set; }
        public string TrainStatus { get; set; }
        public string ArrivalDeparture { get; set; }
        public string StatusColour { get; set; }
    }
}

