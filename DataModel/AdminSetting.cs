using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Legend
    {
        public int SR_NO { get; set; }
        public string LEGEND_CODE { get; set; }
        public string LEGEND { get; set; }
        public string LEGEND_HINDI { get; set; }

    }

    public class AdminSetting
    {
        public int Sr_No { get; set; }
        public int AUTO_LINK_CHECK { get; set; }
        public string RUNNING_STATION_NAME { get; set; }
        public string ANNC_LANGUAGE { get; set; }
        public int TOTAL_PLATFORM_DISPLAYED { get; set; }
        public int RESET_STATE { get; set; }
        public string CHECK_SCHEDULE { get; set; }
        public int AUTO_INTENSITY { get; set; }
        public string SET_Day_Time { get; set; }
        public string SET_Night_Time { get; set; }
        public string SET_Day_Intensity { get; set; }
        public string SET_Night_Intensity { get; set; }
        public int Ethernet_Port { get; set; }
        public int Auto_SEND { get; set; }
        public int Auto_Announce { get; set; }
        public int Total_Coach { get; set; }
        public string PASS_KEY { get; set; }
        public string STATION_CODE { get; set; }
        public int NtesTimeMin { get; set; }
        public bool IsNtesAutoUpdate { get; set; }
        public int Ismaster { get; set; }
        public string CAP_PASSKEY { get; set; }
        public string CAP_Name { get; set; }
        public string STATION_LANGUAGES { get; set; }
        public TimeSpan? DEFAULT_MAINT_START { get; set; }
        public TimeSpan? DEFAULT_MAINT_END { get; set; }
        public TimeSpan? MAINT_START_TIME { get; set; }
        public TimeSpan? MAINT_END_TIME { get; set; }
        public bool USE_DEFAULT_MAINT { get; set; }
    }
    public class BoardInfo
    {
        public int BOARD_ID { get; set; }
        public int HUB_ID { get; set; }
        public int NODE_ID { get; set; }
        public string BOARD_NAME { get; set; }
        public string HUB_IP { get; set; }
        public string HUB_PF { get; set; }
        public string BOARD_IP { get; set; }
        public int BOARD_LINE { get; set; }
        public string BOARD_LOCATION { get; set; }
        public string PF_NO { get; set; }
        public string SELECTED_PF { get; set; }
        public string PLATEFORM_DATA { get; set; }
        public string LANGUAGES { get; set; }
        public string BOTTOM_MESSAGE { get; set; }
        public string DISPLAY_EFFECT { get; set; }
        public int FONT_SIZE { get; set; }
        public string BOARD_SIZE { get; set; }
        public int DELAYTIME { get; set; }
        public int LETTER_GAP { get; set; }
        public string VIDEO_TYPE { get; set; }
        public string SCROLLING_SPEED { get; set; }
        public string DISPLAY_TYPE { get; set; }
        public int IsActive { get; set; }
        public List<string> selectedpf { get; set; }
        public List<string> selectedlanguage { get; set; }
        public bool Health_Status { get; set; }
        public string API_IP { get; set; }


    }

    public class Plateforms
    {
        public int PF_ID { get; set; }
        public string PF_NUMBER { get; set; }
    }
    public class displayEffects
    {
        public int EFFECT_ID { get; set; }
        public string EFFECT_VALUE { get; set; }
        public string EFFECT_NAME { get; set; }
    }
    public class letterGap
    {
        public int GAP_ID { get; set; }
        public string GAP_VALUE { get; set; }
        public string GAP_NAME { get; set; }
    }
    public class ScrollingSpeed
    {
        public int ID { get; set; }
        public string SCROLLING_TEXT { get; set; }
        public string SCROLLING_VALUE { get; set; }
    }
    public class FontSize
    {
        public int SIZE_ID { get; set; }
        public string SIZE_VALUE { get; set; }
        public string SIZE { get; set; }
    }
    public class VideoType
    {
        public int ID { get; set; }
        public string VIDEO_TYPE { get; set; }
        public int VIDEO_VALUE { get; set; }
    }
    public class Recordings
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; } 
    }
}
