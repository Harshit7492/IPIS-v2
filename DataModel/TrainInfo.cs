using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TrainMaster
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
        public string COACH_SEQUENCE {  get; set; }

    }

    public class BoardType
    {
        public int ID { get; set; }
        public string BoardName { get; set; }

        public override string ToString()
        {
            return BoardName;
        }
    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public ComboBoxItem(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class trainstatus
    {
        public string STATUS_CODE { get; set; }
        public string STATUS_ENGLISH { get; set; }
    }
}
