using System;

namespace DataModel
{
    public class TrainMasterBulk
    {
        public string TRAIN_NUMBER { get; set; }
        public string TRAIN_NAME_ENGLISH { get; set; }
        public string TRAIN_NAME_HINDI { get; set; }
        public string TRAIN_NAME_REGIONAL { get; set; }
        public string ORIGIN_CODE { get; set; }
        public string DESTINATION_CODE { get; set; }
        public string ARRIVAL_TIME { get; set; }
        public string DEPARTURE_TIME { get; set; }
        public string PLATFORM_NUMBER { get; set; }
        public string RUNNING_DAYS { get; set; }
        public string TRAIN_TYPE { get; set; }
        public string TRAIN_CATEOGARY { get; set; }
        public string VIA1_CODE { get; set; }
        public string VIA2_CODE { get; set; }
        public string VIA3_CODE { get; set; }
        public string VIA4_CODE { get; set; }
        public string STATUS { get; set; }
        public string PF_END { get; set; }
        public string REGIONAL_BITMAP { get; set; }
        public string TRAIN_SORT_TIME { get; set; }
        public string COACH_SEQUENCE { get; set; }
    }
    public class TrainUpdatedBulk
    {
        public string TrainNumber { get; set; }
        public string TrainNameEnglish { get; set; }
        public string TrainNameHindi { get; set; }
        public string TrainNameRegional { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string VIA1_CODE { get; set; }
        public string VIA2_CODE { get; set; }
        public string VIA3_CODE { get; set; }
        public string VIA4_CODE { get; set; }
        public string ArrTime { get; set; }
        public string DepTime { get; set; }
        public string SArrTime { get; set; }
        public string SDepTime { get; set; }
        public string PfNo { get; set; }
        public string Status { get; set; }
        public string ShortTime { get; set; }
        public string TrainStatus { get; set; }
        public string TrainType { get; set; }
        public string LateStatus { get; set; }
        public int TadbSend { get; set; }
        public string CoachPosition { get; set; }
        public string Platformend { get; set; }
        public string TRAIN_CATEOGARY { get; set; }
        public string ADFlag { get; set; }
        public int stnSrNo { get; set; }
        public string STD { get; set; }
    }
}
