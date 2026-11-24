using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Announcement
    {
        public int Id { get; set; }

        public DateTime LOG_DATE { get; set; }

        public string USER_NAME { get; set; }

        public string USER_TYPE { get; set; }

        public string TRAIN_NUMBER { get; set; }

        public string TRAIN_NAME_ENGLISH { get; set; }

        public string ARRIVAL_TIME { get; set; }

        public string DEPARTURE_TIME { get; set; }

        public string TRAIN_STATUS_ENGLISH { get; set; }

        public string PLATFORM_NUMBER { get; set; }

        public DateTime TRAIN_DATE { get; set; }
    }
}
