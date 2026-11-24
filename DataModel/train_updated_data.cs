using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class train_updated_data
    {
        public int SR_NO { get; set; }
        public string TRAIN_NUMBER { get; set; }
        public string TRAIN_NAME_ENGLISH { get; set; }
        public string TRAIN_NAME_HINDI { get; set; }
        public string TRAIN_NAME_REGIONAL { get; set; } 
        public string SCHEDULE_ARRIVAL_TIME { get; set; }
        public string SCHEDULE_DEPARTURE_TIME { get; set; }
        public string ACTUAL_ARRIVAL_TIME { get; set; }
        public string ACTUAL_DEPARTURE_TIME { get; set; } 
        public string SCHEDULE_PLATFORM_NUMBER { get; set; }
        public string ACTUAL_PLATFORM_NUMBER { get; set; }
        public string ADFlag { get; set; } = "D";
        public string LATE { get; set; }
        public string TRAIN_TYPE { get; set; }
        public string TRAIN_STATUS_CODE { get; set; }
        public string TRAIN_STATUS_ENGLISH { get; set; }
        public string TRAIN_DAY { get; set; }
        public string TRAIN_DATE { get; set; }
        public string COACH_SEQUENCE { get; set; }
        public string TRAIN_CATEOGARY { get; set; }
        public string ORIGIN_CODE { get; set; }
        public string DESTINATION_CODE { get; set; }
        public string DIV_HELD_TERM_STATION_CODE { get; set; }
        public string PLATFORM_END { get; set; } 
        public int TADB_SEND_DATA { get; set; }   
        public int CGDB_SEND_DATA { get; set; }   
        public int ANNOUNCEMENT { get; set; }     
        public string VIA1_CODE { get; set; }
        public string VIA2_CODE { get; set; }
        public string VIA3_CODE { get; set; }
        public string VIA4_CODE { get; set; } 
        public string CHECK_DATA { get; set; }
        public string REGIONAL_BITMAP { get; set; }
        public string MERGED_TRAIN { get; set; }
        public string MERGED_POS { get; set; }
        public string TRAIN_SORT_TIME { get; set; }
        public DateTime STD { get; set; }
        public int stnSrNo { get; set; }
        public bool trainReversalFlag { get; set; }
    }
}
