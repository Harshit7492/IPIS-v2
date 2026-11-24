using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class LoginInfo
    {
        public int Loginid { get; set; }
        public string user_Name { get; set; }
        public string password { get; set; }
        public int ACCESS_ID { get; set; }
        public string USER_TYPE { get; set; }   // join से आएगा
        public int IsActive { get; set; }
    }
    public class AccessRight
    {
        public int ACCESS_ID { get; set; }
        public string USER_TYPE { get; set; }
        public int TRAIN_ENTRY { get; set; }
        public int VDC { get; set; }
        public int HUB_CONFIGURATION { get; set; }
        public int GENERAL_MESSAGE { get; set; }
        public int PLAY_ANNOUNCEMENT { get; set; }
        public int REDUNDANT_PC { get; set; }
        public int ADMIN_SETTINGS { get; set; }
        public int CCTV_DISPLAY { get; set; }
        public int USER_SETTING { get; set; }
        public int TADB_CGDB_SEND { get; set; }
        public int IsActive { get; set; }
    }



    public class GeneralMessage
    {
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
        public string LIVE_STATUS { get; set; }
        public string MESSAGE_ID { get; set; }
        public string BottomMessageFont { get; set; }
        public int PK_ID { get; set; }
    }
    public class LegendList
    {
        public int SR_NO { get; set; }          
        public string LEGEND_CODE { get; set; }
        public string LEGEND { get; set; }
        public string LEGEND_HINDI { get; set; }
    }

    public class LogInHistoryTable
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        public int UserAccessId { get; set; }

        public string UserName { get; set; }

        public DateTime LogInDateTime { get; set; }

        public DateTime? LogOutDateTime { get; set; }

     }
}
