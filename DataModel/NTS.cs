using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel_NTS
{
    public class Rootobject
    {
        public Vtrainlist[] vTrainList { get; set; }
        public Restservicemessage restServiceMessage { get; set; }
        public Cacheupdatetime cacheUpdateTime { get; set; }
        public int cacheDataFlag { get; set; }
    }

    public class Restservicemessage
    {
        public string serviceCallFlag { get; set; }
        public string serviceDataFoundFlag { get; set; }
        public string serviceDataResultFlag { get; set; }
        public string serviceInputValidFlag { get; set; }
        public string serviceMessage { get; set; }
    }

    public class Cacheupdatetime
    {
        public int DD { get; set; }
        public int MM { get; set; }
        public int YYYY { get; set; }
        public int HH { get; set; }
        public int MI { get; set; }
        public int SECONDS { get; set; }
        public string format { get; set; }
        public Calendar calendar { get; set; }
    }

    public class Calendar
    {
        public int year { get; set; }
        public int month { get; set; }
        public int dayOfMonth { get; set; }
        public int hourOfDay { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
    }

    public class Vtrainlist
    {
        public string trainNo { get; set; }
        public string trainName { get; set; }
        public string trainNameHindi { get; set; }
        public string trainType { get; set; }
        public string trainTypeName { get; set; }
        public string trainTypeNameHindi { get; set; }
        public string src { get; set; }
        public string srcName { get; set; }
        public string srcNameHindi { get; set; }
        public string dstn { get; set; }
        public string dstnName { get; set; }
        public string dstnNameHindi { get; set; }
        public string STA { get; set; }
        public string STD { get; set; }
        public string daysOfDeparture { get; set; }
        public string daysOfArrival { get; set; }
        public int stationSchSrNo { get; set; }
        public string platformNo { get; set; }
        public int trainReversalFlag { get; set; }
        public string arrivalCoachPosition { get; set; }
        public string arrivalCoachClass { get; set; }
        public string departureCoachPosition { get; set; }
        public string departureCoachClass { get; set; }
        public string arrPWDCoachPosition { get; set; }
        public string depPWDCoachPosition { get; set; }
        public string coachPosition { get; set; }
        public string coachClass { get; set; }
        public string trainClassOfTravel { get; set; }
    }

}
