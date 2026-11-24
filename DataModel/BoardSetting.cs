

namespace DataModel
{
    public class HUBCONFIG
    {
        public int IsHub { get; set; }
        public int ID { get; set; }
        public string PF_NUMBER { get; set; }
        public string HUB_IP { get; set; }
        public int STATUS { get; set; }
        public int HUB_ID { get; set; }
        public string NODE_NAME { get; set; }
        public int PF_ID { get; set; }
        public bool Health_Status { get; set; }
        public string STATUS_TEXT { get; set; }
        public string HubIPDownLink { get; set; }
        public List<BoardSettingInfo> RelatedBoardsInfo { get; set; }
        public List<BoardSettingInfo> DirectBoardsInfo { get; set; }
        
    }
    public class BoardSettingInfo
    {
        public int BOARD_ID { get; set; }
        public int HUB_ID { get; set; }
        public int NODE_ID { get; set; }
        public string BOARD_NAME { get; set; }
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
        public string Intensity { get; set; }
        public bool RST_Soft_Reset { get; set; }
        public string DTO { get; set; } 
        public string MODULE_IPS { get; set; }
    }

    public class BoardSetting
    {
        public int BoardId { get; set; }
        public int HubId { get; set; }
        public string BoardName { get; set; }
        public string Boardip { get; set; }
        public string BoardLocation { get; set; }
        public int IsActive { get; set; }
        public int BoardPortNo { get; set; }
        public string HubName { get; set; }
        public int HubPortNo { get; set; }
        public int BoardLine { get; set; }
        public int NodeID { get; set; }
        public int PfNo { get; set; }
        public int PfAll { get; set; }
        public int PfSelect { get; set; }
        public string DisplayEffect { get; set; }
        public int FontSize { get; set; }
        public string BoardSize { get; set; }
        public string DisplayInformation { get; set; }
        public int delayedTime { get; set; }
        public string BoardMessage { get; set; }
        public string DisplayType { get; set; }
        public string VideoType { get; set; }
        public string ScrollingSpeed { get; set; }
        public int letterGap { get; set; }
        public string SELECTED_PF { get; set; }
        public string Plateformdata { get; set; }
        public string Languages { get; set; }
        public int Intensity { get; set; }
        public string IpConfig { get; set; }
        public string TrainText { get; set; }
        public bool RST_Soft_Reset { get; set; }
        public string DTO { get; set; }
        public string MODULE_IPS { get; set; }
    }
}
