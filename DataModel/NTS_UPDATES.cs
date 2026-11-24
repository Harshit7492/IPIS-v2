using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class VTrainList
    {
        public string trainNo { get; set; }
        public string trainName { get; set; }
        public string trainNameHindi { get; set; }
        public string trainType { get; set; }
        public string trainTypeName { get; set; }
        public string trainTypeNameHindi { get; set; }
        public string trainSubType { get; set; }
        public string trainSubTypeName { get; set; }
        public string daysOfArrival { get; set; }
        public string trainSubTypeNameHindi { get; set; }
        public string trainStartDate { get; set; }
        public string src { get; set; }
        public int stnSrNo { get; set; }
        public string srcName { get; set; }
        public string srcNameHindi { get; set; }
        public string dstn { get; set; }
        public string dstnName { get; set; }
        public string dstnNameHindi { get; set; }
        public string STA { get; set; }
        public string ETA { get; set; }
        public int isArrived { get; set; }
        public string delayArr { get; set; }
        public string STD { get; set; }
        public string ETD { get; set; }
        public string delayDep { get; set; }
        public string ADFlag { get; set; }
        public string expectedTime { get; set; }
        public string expectedDelay { get; set; }
        public string trainStatus { get; set; }
        public string platformNo { get; set; }
        public string coachPosition { get; set; }
        public string coachClass { get; set; }
        public string arrivalCoachClass { get; set; }
        public string departureCoachClass { get; set; }
        public string schStation { get; set; }
        public string schStationDate { get; set; }
        public string schStationEvent { get; set; }
        public string trainClassOfTravel { get; set; }
        public string trainReversalFlag { get; set; }
    }

    public class VCancelledTrainList
    {
        public string trainNo { get; set; }
        public string trainName { get; set; }
        public string trainNameHindi { get; set; }
        public string trainType { get; set; }
        public string trainTypeName { get; set; }
        public string trainTypeNameHindi { get; set; }
        public string trainSubType { get; set; }
        public string trainSubTypeName { get; set; }
        public string trainStartDate { get; set; }
        public string src { get; set; }
        public string srcName { get; set; }
        public string srcNameHindi { get; set; }
        public string dstn { get; set; }
        public string dstnName { get; set; }
        public string dstnNameHindi { get; set; }
        public string STA { get; set; }
        public string STD { get; set; }
        public string ADFlag { get; set; }
        public string scheduledTime { get; set; }
        public string trainStatus { get; set; }
    }

    public class VRescheduledTrainList
    {
        public string trainNo { get; set; }
        public string trainName { get; set; }
        public string trainNameHindi { get; set; }
        public string trainType { get; set; }
        public string trainTypeName { get; set; }
        public string trainTypeNameHindi { get; set; }
        public string trainSubType { get; set; }
        public string trainSubTypeName { get; set; }
        public string trainSubTypeNameHindi { get; set; }
        public string trainStartDate { get; set; }
        public string src { get; set; }
        public string srcName { get; set; }
        public string srcNameHindi { get; set; }
        public string dstn { get; set; }
        public string dstnName { get; set; }
        public string dstnNameHindi { get; set; }
        public string STA { get; set; }
        public string STD { get; set; }
        public string ADFlag { get; set; }
        public string scheduledTime { get; set; }
        public string trainStatus { get; set; }
    }

    public class RestServiceMessage
    {
        public string serviceCallFlag { get; set; }
        public string serviceDataFoundFlag { get; set; }
        public string serviceDataUpdateFlag { get; set; }
        public string serviceMessage { get; set; }
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

    public class CacheUpdateTime
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

    public class Root
    {
        public List<VTrainList> vTrainList { get; set; }
        public List<VCancelledTrainList> vCancelledTrainList { get; set; }
        public List<VRescheduledTrainList> vRescheduledTrainList { get; set; }
        public RestServiceMessage restServiceMessage { get; set; }
        public CacheUpdateTime cacheUpdateTime { get; set; }
    }

    public class NtesTrainUDTModel
    {
        public string TrainNo { get; set; }
        public string TrainName { get; set; }
        public string TrainNameHindi { get; set; }
        public string TrainType { get; set; }
        public string TrainTypeName { get; set; }
        public string TrainTypeNameHindi { get; set; }
        public string SrcName { get; set; }
        public string DstnName { get; set; }
        public string STA { get; set; }
        public string ETA { get; set; }
        public string STD { get; set; }
        public string ETD { get; set; }
        public string PlatformNo { get; set; }
        public string ADFlag { get; set; }
        public string TrainStatus { get; set; }
    }
}
