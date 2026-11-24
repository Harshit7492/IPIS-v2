using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{

    // View Model for Single Line Display Board
    public class SingleLineDisplayBoardData
    {
        // Board Information
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public string BoardIp { get; set; }
        public string BoardLocation { get; set; }
        public string BoardLine { get; set; }
        public bool IsActive { get; set; }
        public int? ScrollingSpeed { get; set; }
        public int? FontSize { get; set; }
        public int? LetterGap { get; set; }
        public string DisplayEffect { get; set; }
        public string DisplayType { get; set; }
        public int? NodeId { get; set; }
        public int? DelayTime { get; set; }

        // Train Information
        public string TrainNumber { get; set; }
        public string TrainNameEnglish { get; set; }
        public string TrainNameHindi { get; set; }
        public string ScheduleArrivalTime { get; set; }
        public string ScheduleDepartureTime { get; set; }
        public string ActualArrivalTime { get; set; }
        public string ActualDepartureTime { get; set; }
        public string Late { get; set; }
        public string TrainStatusEnglish { get; set; }
        public string ADFlag { get; set; }
        public string TadbSendData { get; set; }
        public string PlatformNumber { get; set; }
        public string Coach_Sequence { get; set; }
        public string FaultData { get; set; }
        public string SelectedPf { get; set; }

    }


    public class AGDBDisplayBoardData
    {
        // Board Information
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public string BoardIp { get; set; }
        public string BoardLocation { get; set; }
        public string BoardLine { get; set; }
        public bool IsActive { get; set; }
        public int? ScrollingSpeed { get; set; }
        public int? FontSize { get; set; }
        public int? LetterGap { get; set; }
        public string DisplayEffect { get; set; }
        public string DisplayType { get; set; }
        public int? NodeId { get; set; }
        public int? DelayTime { get; set; }

        // Train Information
        public string TrainNumber { get; set; }
        public string TrainNameEnglish { get; set; }
        public string TrainNameHindi { get; set; }
        public string ScheduleArrivalTime { get; set; }
        public string ScheduleDepartureTime { get; set; }
        public string ActualArrivalTime { get; set; }
        public string ActualDepartureTime { get; set; }
        public string Late { get; set; }
        public string TrainStatusEnglish { get; set; }
        public string ADFlag { get; set; }
        public string TadbSendData { get; set; }
        public string PlatformNumber { get; set; }
        public string Coach_Sequence { get; set; }
        public string FaultData { get; set; }
        public string SelectedPf { get; set; }

    }

    public class APILiveDataLog
    {
        public int ID { get; set; }
        public string VendorName { get; set; }
        public string DeviceType { get; set; }
        public string DeviceIP { get; set; }
        public string DisplayType { get; set; }
        public string DisplayIP { get; set; }
        public string PF { get; set; }
        public string HSR { get; set; }
        public string INT { get; set; }
        public bool IsPDC { get; set; }
        public string Json { get; set; }
        public DateTime LogTime { get; set; }
    }


    /// <summary>
    /// Train display information class for use with formatter
    /// </summary>
    public class TrainDisplayInfo
    {
        public string TrainNumber { get; set; }
        public string TrainName { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureTime { get; set; }
        public string Status { get; set; }
        public string PlatformNumber { get; set; }
        public string DelayMinutes { get; set; }
        public bool IsCancelled { get; set; }

        public TrainDisplayInfo()
        {
            TrainName = string.Empty;
            ArrivalTime = string.Empty;
            DepartureTime = string.Empty;
            Status = string.Empty;
            PlatformNumber = string.Empty;
            DelayMinutes = string.Empty;
        }
    }

    public class PlaylistData
    {
        public int ID { get; set; }
        public string? AdvType { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? MessageName { get; set; }
        public string? MessageDetails { get; set; }
        public int? PlayDuration { get; set; }
        public int? PlayTimes { get; set; }
        public bool? PlayUnlimited { get; set; }
    }


    public class LiveDataRequest
    {
        public List<DeviceInfo> Device { get; set; }
        public List<DeviceInfo> DisplayList { get; set; }
    }

    public class DeviceInfo
    {
        public string Vendor { get; set; }
        public string Device { get; set; }
        public string IP { get; set; }
        public string PF { get; set; }
        public string INT { get; set; }
        public string HSR { get; set; }
    }

    public class LiveDataResponse
    {
        public List<TrainDto> TrainList { get; set; }
        public List<LineConfDto> LineConf { get; set; }
        public List<PlaylistData> Playlist { get; set; } = new();

    }

    public class TrainDto
    {
        public string TNO { get; set; }
        public string TNE { get; set; }
        public string TNH { get; set; }
        public string TNR { get; set; }
        public string ADF { get; set; }
        public string SAT { get; set; }
        public string SDT { get; set; }
        public string ETA { get; set; }
        public string EDT { get; set; }
        public string LATE { get; set; }
        public string PNO { get; set; }
        public string STA { get; set; }
        public string CCD { get; set; }
        public string COL { get; set; }
    }

    public class CGDBTrainDto
    {
        public string TNO { get; set; }
        public int NOC { get; set; }
        public string CCD { get; set; }
    }

    public class LineConfDto
    {
        public int INT { get; set; }
        public int PTO { get; set; }
        public int DTO { get; set; }
        public int CHR { get; set; }
        public int EFF { get; set; }
        public int SPD { get; set; }
        public string TOD { get; set; }
        public string RST { get; set; }
        public string VID { get; set; }
        public string IMG { get; set; }
        public string LANG { get; set; }
    }

    public sealed class TrainDisplayData
    {
        public string TrainNo { get; set; } = "";
        public Dictionary<string, string> Names { get; set; } = new();

        public string Sat { get; set; } = "";
        public string Sdt { get; set; } = "";
        public string Eat { get; set; } = "";
        public string Edt { get; set; } = "";
        public string Late { get; set; } = "";
        public string Adf { get; set; } = "";
        public string Platform { get; set; } = "";
        public string Status { get; set; } = "";

        public bool SpecialStatus { get; set; }
        public bool FlipRequired { get; set; }
    }

    public class DisplayResult
    {
        public double Pto { get; set; }
        public List<DisplayFrame> Frames { get; set; } = new List<DisplayFrame>();
    }

    public class DisplayFrame
    {
        public int Page { get; set; }
        public int Line { get; set; }
        public string Language { get; set; }
        public int Flip { get; set; }
        public TrainDisplayData Train { get; set; }
    }

}
