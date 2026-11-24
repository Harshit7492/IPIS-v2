using Dapper;
using DataModel;
using Helper;
using LogManager;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
namespace DataBaseHandler
{
    public static class _dbService
    {
        public static string SaveOrUpdateBoardSetting(BoardSetting _boardSetting)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var param = new
                    {
                        _boardSetting.HubId,
                        _boardSetting.NodeID,
                        _boardSetting.BoardId,
                        _boardSetting.BoardName,
                        _boardSetting.Boardip,
                        _boardSetting.BoardLine,
                        _boardSetting.BoardLocation,
                        _boardSetting.PfNo,
                        _boardSetting.SELECTED_PF,
                        _boardSetting.Plateformdata,
                        _boardSetting.Languages,
                        _boardSetting.BoardMessage,
                        _boardSetting.DisplayEffect,
                        _boardSetting.FontSize,
                        _boardSetting.BoardSize,
                        _boardSetting.delayedTime,
                        _boardSetting.VideoType,
                        _boardSetting.ScrollingSpeed,
                        _boardSetting.letterGap,
                        _boardSetting.DisplayType,
                        _boardSetting.IsActive,
                        _boardSetting.Intensity,
                        _boardSetting.RST_Soft_Reset,
                        _boardSetting.DTO,
                        _boardSetting.MODULE_IPS
                    };
                    string result = conn.QueryFirstOrDefault<string>("USP_SAVE_UPDATE_BOARDSETTING", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return string.Empty;
        }
        public static string Add_Update_HubConfig(HUBCONFIG hubConfig)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var param = new
                    {
                        hubConfig.ID,
                        hubConfig.HUB_ID,
                        hubConfig.IsHub,
                        hubConfig.NODE_NAME,
                        hubConfig.PF_ID,
                        hubConfig.HUB_IP,
                        hubConfig.STATUS
                    };
                    string result = conn.QueryFirstOrDefault<string>("USP_ADD_UPDATE_HUBCONFIG", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }

            return string.Empty;
        }
        public static async Task<bool> Update_CapStatus()
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync("USP_UpdateCapStatus", commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
                return false;
            }
        }
        public static async Task<bool> Insert_Cap(CapModel model)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    await conn.OpenAsync();

                    await conn.ExecuteAsync(
                        "USP_InsertCAP",
                        new
                        {
                            model.Identifier,
                            model.AlarmType,
                            model.Message,
                            model.Audio_Path,
                            model.StartDateTime,
                            model.EndDateTime,
                            model.EvenStatus,
                            model.PauseTime,
                            model.Displaycount,
                            model.AnnounceCount,
                            model.Validatetime
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.CapLogger.Error(ex, ex.Message);
                return false;
            }
        }
        public static List<HUBCONFIG> Get_Hub_Config(int hubId)
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();

                using (var multi = conn.QueryMultiple("USP_GET_HUBDETAIL", new { HUB_ID = hubId }, commandType: CommandType.StoredProcedure))
                {
                    var hubs = multi.Read<HUBCONFIG>().ToList();
                    var boards = multi.Read<BoardSettingInfo>().ToList();
                    var directboard = multi.Read<BoardSettingInfo>().ToList();
                    List<HUBCONFIG> lstdummyhub = new List<HUBCONFIG>();
                    foreach (var hub in hubs)
                    {
                        hub.RelatedBoardsInfo = boards
                            .Where(b => b.HUB_ID == hub.HUB_ID)
                            .ToList();
                    }
                    if (hubs.Count > 0)
                        hubs.FirstOrDefault().DirectBoardsInfo = directboard;
                    return hubs;
                }
            }
        }
        public static List<HUBCONFIG> GET_HUBCONFIG(int hubId)
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();

                using (var multi = conn.QueryMultiple("USP_GET_HUBCONFIG", new { hubid = hubId }, commandType: CommandType.StoredProcedure))
                {
                    var hubs = multi.Read<HUBCONFIG>().ToList();
                    return hubs;
                }
            }
        }
        public static List<BoardSettingInfo> Get_Hub_DirectBoard(int hubId)
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();

                using (var multi = conn.QueryMultiple("USP_GET_HUBDETAIL", new { HUB_ID = hubId }, commandType: CommandType.StoredProcedure))
                {
                    var hubs = multi.Read<HUBCONFIG>().ToList();
                    var boards = multi.Read<BoardSettingInfo>().ToList();
                    var directboard = multi.Read<BoardSettingInfo>().ToList();
                    List<HUBCONFIG> lstdummyhub = new List<HUBCONFIG>();
                    foreach (var hub in hubs)
                        hub.RelatedBoardsInfo = boards.Where(b => b.HUB_ID == hub.HUB_ID).ToList();
                    if (hubs.Count > 0)
                        hubs.FirstOrDefault().DirectBoardsInfo = directboard;

                    return directboard;
                }
            }
        }
        public static List<BoardSettingInfo> GET_BOARD_SETTING(int _boardtype)
        {
            List<BoardSettingInfo> _lst = new List<BoardSettingInfo>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { BOARD_ID = _boardtype };
                    _lst = conn.Query<BoardSettingInfo>("USP_GET_BOARDSETTING", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static string GET_SOUND_PATTERNS(string traintype, string trainstatus, string columnName)
        {
            string pattern = string.Empty;

            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new
                    {
                        traintype = traintype,
                        trainstatus = trainstatus,
                        columnName = columnName
                    };

                    pattern = conn.QueryFirstOrDefault<string>(
                        "USP_GET_SOUND_PATTERNS",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }

            return pattern ?? string.Empty;
        }
        public static List<Plateforms> GET_PLATEFORMS(int plateformid)
        {
            List<Plateforms> _lst = new List<Plateforms>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { plateformid = plateformid };
                    _lst = conn.Query<Plateforms>("USP_GET_PLATEFORMS", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<displayEffects> GET_DisplayEffect(int _effectid)
        {
            List<displayEffects> _lst = new List<displayEffects>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { effectid = _effectid };
                    _lst = conn.Query<displayEffects>("USP_GET_DisplayEffect", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<letterGap> GET_letterGap(int _letterid)
        {
            List<letterGap> _lst = new List<letterGap>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { letterid = _letterid };
                    _lst = conn.Query<letterGap>("USP_GET_letterGap", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<ScrollingSpeed> GET_ScrollingSpeed(int _speedid)
        {
            List<ScrollingSpeed> _lst = new List<ScrollingSpeed>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { speedid = _speedid };
                    _lst = conn.Query<ScrollingSpeed>("USP_GET_ScrollingSpeed", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<FontSize> GET_FontSize(int _sizeid)
        {
            List<FontSize> _lst = new List<FontSize>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { sizeid = _sizeid };
                    _lst = conn.Query<FontSize>("USP_GET_FontSize", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<VideoType> GET_VideoType(int _videoid)
        {
            List<VideoType> _lst = new List<VideoType>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { videoid = _videoid };
                    _lst = conn.Query<VideoType>("USP_GET_VideoType", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<Message> GET_MESSAGES(int MESSAGE_ID)
        {
            List<Message> _lst = new List<Message>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { MESSAGE_ID = MESSAGE_ID };
                    _lst = conn.Query<Message>("USP_GET_MESSAGES", parameters, commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static bool UpdateTrainOnlineDgv(train_updated_data train)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@TRAIN_NUMBER", train.TRAIN_NUMBER);
                    parameters.Add("@TRAIN_STATUS_ENGLISH", train.TRAIN_STATUS_ENGLISH, DbType.String);
                    parameters.Add("@SCHEDULE_ARRIVAL_TIME", train.SCHEDULE_ARRIVAL_TIME, DbType.String);
                    parameters.Add("@SCHEDULE_DEPARTURE_TIME", train.SCHEDULE_DEPARTURE_TIME, DbType.String);
                    parameters.Add("@ACTUAL_ARRIVAL_TIME", train.ACTUAL_ARRIVAL_TIME, DbType.String);
                    parameters.Add("@ACTUAL_DEPARTURE_TIME", train.ACTUAL_DEPARTURE_TIME, DbType.String);
                    parameters.Add("@LATE", train.LATE, DbType.String);
                    parameters.Add("@ACTUAL_PLATFORM_NUMBER", train.ACTUAL_PLATFORM_NUMBER, DbType.String);
                    parameters.Add("@TADB_SEND_DATA", train.TADB_SEND_DATA, DbType.String);
                    parameters.Add("@CGDB_SEND_DATA", train.CGDB_SEND_DATA, DbType.String);
                    parameters.Add("@ANNC_DATA", train.ANNOUNCEMENT, DbType.String);

                    int rowsAffected = conn.Execute("USP_UPDATE_TRAIN_ONLINE_DGV", parameters,
                        commandType: CommandType.StoredProcedure);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return false;
            }
        }
        public static string ADD_TRAIN_TO_ONLINE(string trainNumber)
        {
            string result = string.Empty;

            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    return result = conn.QuerySingleOrDefault<string>("USP_ADD_TRAIN_TO_ONLINE",
                        new { TRAIN_NUMBER = trainNumber },
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error in ADD_TRAIN_TO_ONLINE: " + ex.Message);
            }
            return result;
        }
        public static bool AddOrUpdateMessage(Message msg)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new
                    {
                        MESSAGE_ID = msg.MESSAGE_ID,
                        MESSAGE_CODE = msg.MESSAGE_CODE,
                        MESSAGE_ENGLISH = msg.MESSAGE_ENGLISH,
                        MESSAGE_HINDI = msg.MESSAGE_HINDI,
                        MESSAGE_REGIONAL = msg.MESSAGE_REGIONAL,
                        SOUND_ENGLISH = msg.SOUND_ENGLISH,
                        SOUND_HINDI = msg.SOUND_HINDI,
                        SOUND_REGIONAL = msg.SOUND_REGIONAL,
                        DISPLAY_STATUS = msg.DISPLAY_STATUS,
                        REGIONAL_BITMAP = msg.REGIONAL_BITMAP,
                        MESSAGE_FOR_DISPLAY = msg.MESSAGE_FOR_DISPLAY,
                        LIVE_STATUS = msg.LIVE_STATUS,
                        BottomMessageFont = msg.BottomMessageFont
                    };

                    int rowsAffected = conn.Execute("USP_ADD_UPDATE_MSG", parameters, commandType: CommandType.StoredProcedure);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
            return false;
        }
        public static List<StationList> GET_ALL_STATIONS()
        {
            List<StationList> _lst = new List<StationList>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    _lst = conn.Query<StationList>("USP_GET_STATION_BY_CODE", commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            { 
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static bool ADD_UPDATE_STATION_CODE(StationList _postdata)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var param = new
                    {
                        STATION_CODE = _postdata.STATION_CODE,
                        STATION_NAME_HINDI = _postdata.STATION_NAME_HINDI,
                        STATION_NAME_ENGLISH = _postdata.STATION_NAME_ENGLISH,
                        STATION_NAME_REGIONAL = _postdata.STATION_NAME_REGIONAL
                    };

                    int result = conn.QueryFirstOrDefault<int>(
                        "USP_ADD_UPDATE_STATION_CODE",
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result > 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return false;
        }
        public static int USP_DELETE_BOARD(int _IsHub, int _ID)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var param = new
                    {
                        IsHub = _IsHub,
                        ID = _ID
                    };

                    int result = conn.QueryFirstOrDefault<int>("USP_DELETE_BOARD", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return 0;
        }
        public static bool ADD_UPDATE_COACH_LEGEND(LegendList postdata)
        {
            try
            {
                using var conn = new SqlConnection(Config._dbSelfConnection);

                var param = new
                {
                    postdata.SR_NO,
                    postdata.LEGEND_CODE,
                    postdata.LEGEND,
                    postdata.LEGEND_HINDI
                };

                return conn.QueryFirstOrDefault<int>(
                    "USP_UPDATE_COACHMASTER",
                    param,
                    commandType: CommandType.StoredProcedure) == 1;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return false;
            }
        }
        public static bool ADD_UPDATE_ACCESS_RIGHTS(AccessRight _postdata)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var param = new
                    {
                        ACCESS_ID = _postdata.ACCESS_ID,
                        USER_TYPE = _postdata.USER_TYPE,
                        TRAIN_ENTRY = _postdata.TRAIN_ENTRY,
                        VDC = _postdata.VDC,
                        HUB_CONFIGURATION = _postdata.HUB_CONFIGURATION,
                        GENERAL_MESSAGE = _postdata.GENERAL_MESSAGE,
                        PLAY_ANNOUNCEMENT = _postdata.PLAY_ANNOUNCEMENT,
                        REDUNDANT_PC = _postdata.REDUNDANT_PC,
                        ADMIN_SETTINGS = _postdata.ADMIN_SETTINGS,
                        CCTV_DISPLAY = _postdata.CCTV_DISPLAY,
                        USER_SETTING = _postdata.USER_SETTING,
                        TADB_CGDB_SEND = _postdata.TADB_CGDB_SEND,
                        IsActive = _postdata.IsActive
                    };

                    int result = conn.QueryFirstOrDefault<int>("USP_ADD_UPDATE_ACCESS_RIGHTS", param, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return false;
        }
        public static bool ADD_UPDATE_USERS(LoginInfo _postdata)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var param = new
                    {
                        Loginid = _postdata.Loginid,
                        user_Name = _postdata.user_Name,
                        password = _postdata.password,
                        ACCESS_ID = _postdata.ACCESS_ID,
                        IsActive = _postdata.IsActive,
                    };

                    int result = conn.QueryFirstOrDefault<int>("USP_ADD_UPDATE_USERS", param, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                        return true;

                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return false;
        }
        public static List<AccessRight> GET_ACCESS_RIGHTS(int ACCESS_ID = 0)
        {
            List<AccessRight> _lst = new List<AccessRight>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { Access_id = ACCESS_ID };
                    _lst = conn.Query<AccessRight>("USP_GET_ACCESS_RIGHTS", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<AccessRight> GET_ACTIVE_ACCESS_RIGHTS()
        {
            List<AccessRight> _lst = new List<AccessRight>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    _lst = conn.Query<AccessRight>(
                        @"SELECT *
          FROM ACCESS_RIGHTS
          WHERE IsActive = 1"
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<TrainMaster> GET_TRAIN_MASTER(int trainNumber = 0)
        {
            List<TrainMaster> _lst = new List<TrainMaster>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { TrainNumber = trainNumber };
                    _lst = conn.Query<TrainMaster>("USP_GET_TRAIN_MASTER", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static bool SaveOrUpdateTrain(TrainMaster train)
        {
            bool isSuccess = false;
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new
                    {
                        TRAIN_NUMBER = train.TRAIN_NUMBER,
                        TRAIN_NAME_ENGLISH = train.TRAIN_NAME_ENGLISH,
                        TRAIN_NAME_HINDI = train.TRAIN_NAME_HINDI,
                        TRAIN_NAME_REGIONAL = train.TRAIN_NAME_REGIONAL,
                        ORIGIN_CODE = train.ORIGIN_CODE,
                        DESTINATION_CODE = train.DESTINATION_CODE,
                        ARRIVAL_TIME = train.ARRIVAL_TIME,
                        DEPARTURE_TIME = train.DEPARTURE_TIME,
                        PLATFORM_NUMBER = train.PLATFORM_NUMBER,
                        RUNNING_DAYS = train.RUNNING_DAYS,
                        TRAIN_TYPE = train.TRAIN_TYPE,
                        TRAIN_CATEOGARY = train.TRAIN_CATEOGARY,
                        VIA1_CODE = train.VIA1_CODE, 
                        VIA2_CODE = train.VIA2_CODE,
                        VIA3_CODE = train.VIA3_CODE,
                        VIA4_CODE = train.VIA4_CODE,
                        STATUS = train.STATUS,
                        PF_END = train.PF_END,
                        //COACH_SEQUENCE = train.COACH_SEQUENCE,
                        REGIONAL_BITMAP = train.REGIONAL_BITMAP,
                        TRAIN_SORT_TIME = train.TRAIN_SORT_TIME
                    };

                    conn.Execute("USP_GET_ADD_UPDATE_TRAINMASTER", parameters, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
            }
            return isSuccess;
        }
        public static List<train_updated_data> GET_UPDATED_TRAINS_DATA(int _trainNumber)
        {
            List<train_updated_data> _lst = new List<train_updated_data>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { trainNumber = _trainNumber };
                    _lst = conn.Query<train_updated_data>("USP_GET_ONLINE_TRAINS_DATA", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<BoardInfo> GET_BOARD_DETAILS(string _boardtype)
        {
            List<BoardInfo> _lst = new List<BoardInfo>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { boardtype = _boardtype };
                    _lst = conn.Query<BoardInfo>("USP_GET_BOARD_DETAILS", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
                foreach (var item in _lst)
                {
                    item.selectedpf = string.IsNullOrWhiteSpace(item.SELECTED_PF)

                        ? new List<string>()
                        : item.SELECTED_PF.Split('+', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(x => x.Trim())
                                          .ToList();

                    item.selectedlanguage = string.IsNullOrWhiteSpace(item.LANGUAGES)
                        ? new List<string>()
                        : item.LANGUAGES.Split('+', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim())
                                        .ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<Legend> GET_LEGEND_LIST(int id = 0)
        {
            List<Legend> _lst = new List<Legend>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { id = id };
                    _lst = conn.Query<Legend>("USP_GET_LEGEND_LIST", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<Recordings> GET_RECORDING(int id = 0)
        {
            List<Recordings> _lst = new List<Recordings>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { id = id };
                    _lst = conn.Query<Recordings>("USP_GET_RECORDING", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static List<string> GET_ALL_DEVICE_IPS()
        {
            List<string> lst = new List<string>();

            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    lst = conn.Query<string>(
                        "USP_GET_ALL_DEVICE_IPS",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }

            return lst;
        }
        public static GeneralMessage GET_GENERAL_MESSAGE()
        {
            List<GeneralMessage> _lst = new List<GeneralMessage>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { id = 0 };
                    _lst = conn.Query<GeneralMessage>("USP_GET_GENERAL_MESSAGE", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.MessageLogger.Error(ex, ex.Message);
            }
            return _lst.FirstOrDefault();
        }
        public static GeneralMessage GET_PLATEFORMS()
        {
            List<GeneralMessage> _lst = new List<GeneralMessage>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new { id = 0 };
                    _lst = conn.Query<GeneralMessage>("USP_GET_GENERAL_MESSAGE", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return _lst.FirstOrDefault();
        }
        public static (int StatusCode, int UserId) GET_LOGIN(string userName, string password)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new
                    {
                        userName,
                        password
                    };

                    var result = conn.QueryFirstOrDefault(
                        "USP_GET_LOGIN",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    if (result == null)
                        return (0, 0);

                    return (
                        Convert.ToInt32(result.StatusCode),
                        Convert.ToInt32(result.UserId)
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.UserLoginLogger.Error(ex, ex.Message);
                throw;
            }
        }
        public static List<LoginInfo> GET_LOGIN_DETAILS(int userid)
        {
            List<LoginInfo> _lst = new List<LoginInfo>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var parameters = new
                    {
                        userid = userid
                    };
                    _lst = conn.Query<LoginInfo>("USP_GET_LOGIN_DETAILS", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.UserLoginLogger.Error(ex, ex.Message);
            }
            return _lst;
        }
        public static bool UpdateAdminSetting(AdminSetting setting)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var result = conn.Execute("USP_ADD_UPDATE_ADMINSETTING",
                        new
                        {
                            setting.AUTO_LINK_CHECK,
                            setting.RUNNING_STATION_NAME,
                            setting.ANNC_LANGUAGE,
                            setting.TOTAL_PLATFORM_DISPLAYED,
                            setting.RESET_STATE,
                            setting.CHECK_SCHEDULE,
                            setting.AUTO_INTENSITY,
                            setting.SET_Day_Time,
                            setting.SET_Night_Time,
                            setting.SET_Day_Intensity,
                            setting.SET_Night_Intensity,
                            setting.Ethernet_Port,
                            setting.Auto_Announce,
                            setting.Total_Coach,
                            setting.PASS_KEY,
                            setting.STATION_CODE,
                            setting.IsNtesAutoUpdate,
                            setting.NtesTimeMin,
                            setting.CAP_PASSKEY,
                            setting.CAP_Name,
                            setting.STATION_LANGUAGES,
                            setting.Ismaster,
                            setting.DEFAULT_MAINT_START,
                            setting.DEFAULT_MAINT_END,
                            setting.MAINT_START_TIME,
                            setting.MAINT_END_TIME,
                            setting.USE_DEFAULT_MAINT
                        },
                        commandType: CommandType.StoredProcedure
                    );
                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return false;
            }
        }
        public static AdminSetting GET_ADMIN_SETTING()
        {
            AdminSetting setting = null;
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    setting = conn.QueryFirstOrDefault<AdminSetting>(
                        "USP_GET_ADMINSETTING",
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return setting;
        }
        public static List<trainstatus> Train_Status_list(string query)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var result = conn.Query<trainstatus>(query).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return new List<trainstatus>();
            }
        }
        public static async Task SCHEDULE_MAINTENANCE(int days)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    await conn.OpenAsync();

                    var parameters = new { days };
                    await conn.ExecuteAsync(
                        "USP_SCHEDULE_MAINTENANCE",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, $"SCHEDULE_MAINTENANCE failed: {ex.Message}");
            }
        }
        public static void InsertTrainEntry(Root ntesTrain)
        {
            if (ntesTrain?.vTrainList == null || ntesTrain.vTrainList.Count == 0)
                return;
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    // Open connection synchronously to avoid race conditions with subsequent queries
                    conn.Open();
                    foreach (var itemtrain in ntesTrain.vTrainList)
                    {
                        try
                        {
                            var parameters = new { TrainNumber = itemtrain.trainNo };
                            TrainMaster _trainMaster = conn.QueryFirstOrDefault<TrainMaster>(
                                "USP_GET_TRAIN_MASTER", parameters, commandType: CommandType.StoredProcedure);

                            string trainNameEnglish = (_trainMaster?.TRAIN_NAME_ENGLISH ?? itemtrain.trainName)?.Trim();
                            string trainNameHindi = (_trainMaster?.TRAIN_NAME_HINDI ?? itemtrain.trainNameHindi)?.Trim();
                            string trainNameRegional = (_trainMaster?.TRAIN_NAME_REGIONAL ?? string.Empty).Trim();
                            string origin = _trainMaster?.ORIGIN_CODE ?? itemtrain.src;
                            string destination = _trainMaster?.DESTINATION_CODE ?? itemtrain.dstn;
                            string pfEnd = _trainMaster?.PF_END ?? "UP";

                            string arrTime = string.IsNullOrEmpty(itemtrain.ETA) ? null : itemtrain.ETA;
                            string depTime = string.IsNullOrEmpty(itemtrain.ETD) ? null : itemtrain.ETD;
                            string sArrTime = string.IsNullOrEmpty(itemtrain.STA) ? null : itemtrain.STA;
                            string sDepTime = string.IsNullOrEmpty(itemtrain.STD) ? null : itemtrain.STD;
                            string platformNo = string.IsNullOrEmpty(itemtrain.platformNo?.Trim()) ? "-" : itemtrain.platformNo.Trim();
                            string coachPosition = string.IsNullOrEmpty(itemtrain.coachPosition) ? null : itemtrain.coachPosition;

                            string trainType, status, shortTime, trainStatus;
                            if (itemtrain.ADFlag == "A")
                            {
                                trainType = "ARR";
                                status = "A";
                                shortTime = itemtrain.ETA;
                                trainStatus = itemtrain.isArrived == 1 ? "HAS ARRIVED ON" :
                                    (string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME") ?
                                    "RUNNING RIGHT TIME" : "RUNNING LATE";
                            }
                            else if (itemtrain.ADFlag == "D" && string.IsNullOrEmpty(itemtrain.ETA))
                            {
                                trainType = "DEP";
                                status = "D";
                                shortTime = itemtrain.ETD;
                                trainStatus = itemtrain.isArrived == 1 ? "IS READY TO LEAVE" :
                                    (string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME") ?
                                    "RUNNING RIGHT TIME" : "RESCHEDULED";
                            }
                            else
                            {
                                trainType = "PAS";
                                status = "A";
                                shortTime = itemtrain.ETD;
                                trainStatus = itemtrain.isArrived == 1 ? "HAS ARRIVED ON" :
                                    (string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME") ?
                                    "RUNNING RIGHT TIME" : "RUNNING LATE";
                            }
                            string lateStatus = (itemtrain.isArrived == 1 || string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME")
                                ? null : itemtrain.expectedDelay;

                            var upsertParams = new
                            {
                                TrainNumber = itemtrain.trainNo,
                                TrainNameEnglish = trainNameEnglish,
                                TrainNameHindi = trainNameHindi,
                                TrainNameRegional = trainNameRegional,
                                Origin = origin,
                                Destination = destination,
                                VIA1_CODE = _trainMaster?.VIA1_CODE,
                                VIA2_CODE = _trainMaster?.VIA2_CODE,
                                VIA3_CODE = _trainMaster?.VIA3_CODE,
                                VIA4_CODE = _trainMaster?.VIA4_CODE,
                                ArrTime = arrTime,
                                DepTime = depTime,
                                SArrTime = sArrTime,
                                SDepTime = sDepTime,
                                PfNo = platformNo,
                                Status = itemtrain.ADFlag,
                                ShortTime = shortTime,
                                TrainStatus = trainStatus,
                                TrainType = trainType,
                                LateStatus = lateStatus,
                                TadbSend = "0",
                                CoachPosition = coachPosition,
                                Platformend = pfEnd,
                                TRAIN_CATEOGARY = itemtrain.trainTypeName,
                                ADFlag = itemtrain.ADFlag,
                                stnSrNo = itemtrain.stnSrNo,
                                STD = sDepTime
                            };
                            // Synchronous Execute call prevents MultipleActiveResultSets (MARS) exceptions on database commands
                            conn.Execute("USP_ADD_UPDATE_TRAIN_FROM_NTES", upsertParams, commandType: CommandType.StoredProcedure);
                        }
                        catch (Exception exInner)
                        {
                            LoggerService.NtesLogger.Error(exInner, $"Error processing train {itemtrain.trainNo}: {exInner.Message}");
                        }
                    }
                    foreach (var itemtrain in ntesTrain.vCancelledTrainList)
                    {
                        try
                        {
                            var parameters = new { TrainNumber = itemtrain.trainNo };
                            TrainMaster _trainMaster = conn.QueryFirstOrDefault<TrainMaster>(
                                "USP_GET_TRAIN_MASTER", parameters, commandType: CommandType.StoredProcedure);

                            string trainNameEnglish = (_trainMaster?.TRAIN_NAME_ENGLISH ?? itemtrain.trainName)?.Trim();
                            string trainNameHindi = (_trainMaster?.TRAIN_NAME_HINDI ?? itemtrain.trainNameHindi)?.Trim();
                            string trainNameRegional = (_trainMaster?.TRAIN_NAME_REGIONAL ?? string.Empty).Trim();
                            string origin = _trainMaster?.ORIGIN_CODE ?? itemtrain.src;
                            string destination = _trainMaster?.DESTINATION_CODE ?? itemtrain.dstn;
                            string pfEnd = _trainMaster?.PF_END ?? "UP";

                            string arrTime = null;
                            string depTime = null;
                            string sArrTime = string.IsNullOrEmpty(itemtrain.STA) ? null : itemtrain.STA;
                            string sDepTime = string.IsNullOrEmpty(itemtrain.STD) ? null : itemtrain.STD;
                            string platformNo = "-";
                            string coachPosition = null;

                            string trainType, status, shortTime, trainStatus;
                            if (itemtrain.ADFlag == "A")
                            {
                                trainType = "ARR";
                                status = "A";
                                shortTime = itemtrain.STD;
                                trainStatus = "CANCELLED";
                            }
                            else if (itemtrain.ADFlag == "D")
                            {
                                trainType = "DEP";
                                status = "D";
                                shortTime = itemtrain.STD;
                                trainStatus = "CANCELLED";
                            }
                            else
                            {
                                trainType = "PAS";
                                status = "A";
                                shortTime = itemtrain.STD;
                                trainStatus = "CANCELLED";
                            }
                            string lateStatus = null;

                            var upsertParams = new
                            {
                                TrainNumber = itemtrain.trainNo,
                                TrainNameEnglish = trainNameEnglish,
                                TrainNameHindi = trainNameHindi,
                                TrainNameRegional = trainNameRegional,
                                Origin = origin,
                                Destination = destination,
                                VIA1_CODE = _trainMaster?.VIA1_CODE,
                                VIA2_CODE = _trainMaster?.VIA2_CODE,
                                VIA3_CODE = _trainMaster?.VIA3_CODE,
                                VIA4_CODE = _trainMaster?.VIA4_CODE,
                                ArrTime = arrTime,
                                DepTime = depTime,
                                SArrTime = sArrTime,
                                SDepTime = sDepTime,
                                PfNo = platformNo,
                                Status = itemtrain.ADFlag,
                                ShortTime = shortTime,
                                TrainStatus = trainStatus,
                                TrainType = trainType,
                                LateStatus = lateStatus,
                                TadbSend = "0",
                                CoachPosition = coachPosition,
                                Platformend = pfEnd,
                                TRAIN_CATEOGARY = itemtrain.trainTypeName,
                                ADFlag = itemtrain.ADFlag,
                                stnSrNo = 0,
                                STD = sDepTime
                            };
                            // Synchronous Execute call prevents MultipleActiveResultSets (MARS) exceptions on database commands
                            conn.Execute("USP_ADD_UPDATE_TRAIN_FROM_NTES", upsertParams, commandType: CommandType.StoredProcedure);
                        }
                        catch (Exception exInner)
                        {
                            LoggerService.NtesLogger.Error(exInner, $"Error processing train {itemtrain.trainNo}: {exInner.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, $"InsertTrainEntry failed: {ex.Message}");
            }
        }
        public static async Task InsertCancelTrainEntry(Root ntesTrain)
        {
            if (ntesTrain?.vTrainList == null || ntesTrain.vTrainList.Count == 0)
                return;

            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    await conn.OpenAsync();

                    foreach (var itemtrain in ntesTrain.vTrainList)
                    {
                        try
                        {
                            var parameters = new { TrainNumber = itemtrain.trainNo };
                            var trainMaster = await conn.QueryFirstOrDefaultAsync<TrainMaster>(
                                "USP_GET_TRAIN_MASTER", parameters, commandType: CommandType.StoredProcedure);

                            bool isTrainExists = await conn.QueryFirstOrDefaultAsync<int>(
                                "USP_GET_TRAIN", parameters, commandType: CommandType.StoredProcedure) > 0;

                            string trainNameEnglish = (trainMaster?.TRAIN_NAME_ENGLISH ?? itemtrain.trainName)?.Trim();
                            string trainNameHindi = (trainMaster?.TRAIN_NAME_HINDI ?? itemtrain.trainNameHindi)?.Trim();
                            string trainNameRegional = (trainMaster?.TRAIN_NAME_REGIONAL ?? string.Empty).Trim();
                            string origin = trainMaster?.ORIGIN_CODE ?? itemtrain.src;
                            string destination = trainMaster?.DESTINATION_CODE ?? itemtrain.dstn;

                            string arrTime = null, depTime = null, sArrTime = null, sDepTime = null, trainType, status, shortTime;

                            if (!string.IsNullOrEmpty(itemtrain.STA)) // Arrival train
                            {
                                arrTime = string.IsNullOrEmpty(itemtrain.ETA) ? null : itemtrain.ETA;
                                sArrTime = string.IsNullOrEmpty(itemtrain.STA) ? null : itemtrain.STA;
                                depTime = string.IsNullOrEmpty(itemtrain.ETD) ? null : itemtrain.ETD;
                                sDepTime = string.IsNullOrEmpty(itemtrain.STD) ? null : itemtrain.STD;
                                trainType = "ARR";
                                status = "A";
                                shortTime = sArrTime;
                            }
                            else // Departure train
                            {
                                depTime = string.IsNullOrEmpty(itemtrain.ETD) ? null : itemtrain.ETD;
                                sDepTime = string.IsNullOrEmpty(itemtrain.STD) ? null : itemtrain.STD;
                                trainType = "DEP";
                                status = "D";
                                shortTime = sDepTime;
                            }

                            var upsertParams = new
                            {
                                TrainNumber = itemtrain.trainNo,
                                TrainNameEnglish = trainNameEnglish,
                                TrainNameHindi = trainNameHindi,
                                TrainNameRegional = trainNameRegional,
                                Origin = origin,
                                Destination = destination,
                                VIA1_CODE = trainMaster?.VIA1_CODE,
                                VIA2_CODE = trainMaster?.VIA2_CODE,
                                VIA3_CODE = trainMaster?.VIA3_CODE,
                                VIA4_CODE = trainMaster?.VIA4_CODE,
                                ArrTime = arrTime,
                                DepTime = depTime,
                                SArrTime = sArrTime,
                                SDepTime = sDepTime,
                                TrainType = trainType,
                                Status = status,
                                ShortTime = shortTime,
                                TrainStatus = "CANCELLED",
                                LateStatus = (string)null,
                                PfNo = "-",
                                RunningDays = "7",
                                ChkStatus = "Y",
                                TadbSend = "0",
                                Platformend = "UP"
                            };

                            string sql = @"
                        IF EXISTS (SELECT 1 FROM TrainUpdatedData WHERE Train_Number = @TrainNumber)
                            UPDATE TrainUpdatedData SET
                                TRAIN_NAME_ENGLISH = @TrainNameEnglish,
                                TRAIN_NAME_HINDI = @TrainNameHindi,
                                TRAIN_NAME_REGIONAL = @TrainNameRegional,
                                ORIGIN_CODE = @Origin,
                                DESTINATION_CODE = @Destination,
                                VIA1_CODE = @VIA1_CODE,
                                VIA2_CODE = @VIA2_CODE,
                                VIA3_CODE = @VIA3_CODE,
                                VIA4_CODE = @VIA4_CODE,
                                SCHEDULE_ARRIVAL_TIME = @SArrTime,
                                ACTUAL_ARRIVAL_TIME = @ArrTime,
                                SCHEDULE_DEPARTURE_TIME = @SDepTime,
                                ACTUAL_DEPARTURE_TIME = @DepTime,
                                ACTUAL_PLATFORM_NUMBER = @PfNo,
                                TRAIN_STATUS_CODE = @Status,
                                TRAIN_SORT_TIME = @ShortTime,
                                TRAIN_STATUS_ENGLISH = @TrainStatus,
                                TRAIN_TYPE = @TrainType,
                                LATE = @LateStatus,
                                PLATFORM_END = @Platformend
                            WHERE Train_Number = @TrainNumber
                        ELSE
                            INSERT INTO TrainUpdatedData
                                (Train_Number, TRAIN_NAME_ENGLISH, TRAIN_NAME_HINDI, TRAIN_NAME_REGIONAL,
                                ORIGIN_CODE, DESTINATION_CODE, VIA1_CODE, VIA2_CODE, VIA3_CODE, VIA4_CODE,
                                SCHEDULE_ARRIVAL_TIME, ACTUAL_ARRIVAL_TIME, SCHEDULE_DEPARTURE_TIME, ACTUAL_DEPARTURE_TIME,
                                ACTUAL_PLATFORM_NUMBER, TRAIN_STATUS_CODE, TRAIN_SORT_TIME, TRAIN_STATUS_ENGLISH, TRAIN_TYPE,
                                LATE, TADB_SEND_DATA, PLATFORM_END)
                            VALUES
                                (@TrainNumber, @TrainNameEnglish, @TrainNameHindi, @TrainNameRegional,
                                @Origin, @Destination, @VIA1_CODE, @VIA2_CODE, @VIA3_CODE, @VIA4_CODE,
                                @SArrTime, @ArrTime, @SDepTime, @DepTime,
                                @PfNo, @Status, @ShortTime, @TrainStatus, @TrainType,
                                @LateStatus, @TadbSend, @Platformend);";

                            await conn.ExecuteAsync(sql, upsertParams);
                        }
                        catch (Exception exInner)
                        {
                            LoggerService.NtesLogger.Error(exInner, $"Error processing cancelled train {itemtrain.trainNo}: {exInner.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, $"InsertCancelTrainEntry failed: {ex.Message}");
            }
        }
        public static async Task DELETE_TRAIN_NTES(string trainNumber)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    await conn.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@TrainNumbers", trainNumber, DbType.String);

                    await conn.ExecuteAsync("USP_DELETE_TRAIN", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, $"Error executing DELETE_TRAIN_NTES: {ex.Message}");
            }
        }
        public static async Task ExecuteNonQueryCMDAsync(string query, SqlCommand command)
        {
            try
            {
                using (var connection = new SqlConnection(Config._dbSelfConnection))
                {
                    await connection.OpenAsync();

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        foreach (SqlParameter param in command.Parameters)
                        {
                            var clonedParam = new SqlParameter(param.ParameterName, param.SqlDbType)
                            {
                                Value = param.Value,
                                Direction = param.Direction,
                                Size = param.Size,
                                Precision = param.Precision,
                                Scale = param.Scale
                            };
                            cmd.Parameters.Add(clonedParam);
                        }

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
        }
        public static string QuerySingleOrDefault(string query)
        {
            string result = string.Empty;
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    return result = conn.QuerySingleOrDefault<string>(query);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return result;
        }
        public static List<string> QueryList(string query)
        {
            List<string> result = new List<string>();
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    result = conn.Query<string>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return result;
        }
        public static string ExecuteScalar(string query)
        {
            string result = string.Empty;
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    return result = conn.ExecuteScalar<string>(query);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return result;
        }

        public static DateTime? ExecuteScalarDateTime(string query)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    return conn.ExecuteScalar<DateTime?>(query);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return null;
            }
        }
        public static int ExecuteNonQuery(string query)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    return conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return -1;
            }
        }

        #region BOARDSETTING - GENERIC COLUMN UPDATE

        // Columns of BOARDSETTING that UpdateBoardSettingColumn may write. A column name cannot be a SQL
        // parameter, so only these exact names are ever put into the SQL text; the values always go as parameters.
        private static readonly string[] BoardSettingUpdatableColumns = { "Intensity", "DTO", "RST_Soft_Reset" };

        // 0/1 flag columns that ToggleBoardSettingFlag may flip.
        private static readonly string[] BoardSettingFlagColumns = { "RST_Soft_Reset" };

        /// <summary>
        /// Sets one BOARDSETTING column for one board: UPDATE BOARDSETTING SET [column] = @Value WHERE BOARD_ID = @BoardId.
        /// Returns the number of rows updated (0 = BOARD_ID not found), or -1 when the column is not allowed or the update fails.
        /// </summary>
        public static int UpdateBoardSettingColumn(int boardId, string columnName, object value)
        {
            string column = ResolveBoardSettingColumn(columnName, BoardSettingUpdatableColumns);
            if (boardId <= 0 || column == null)
            {
                LoggerService.Logger.Warning(
                    "UpdateBoardSettingColumn refused: BOARD_ID {BoardId}, column {Column}.", boardId, columnName);
                return -1;
            }

            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    return conn.Execute(
                        "UPDATE BOARDSETTING SET [" + column + "] = @Value WHERE BOARD_ID = @BoardId;",
                        new { Value = value, BoardId = boardId });
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "UpdateBoardSettingColumn failed for BOARD_ID " + boardId + ", column " + column + ".");
                return -1;
            }
        }

        /// <summary>
        /// Flips a 0/1 BOARDSETTING flag for one board (1 -> 0, 0 or NULL -> 1) in a single statement.
        /// Returns the rows updated, or -1 when the column is not an allowed flag or the update fails.
        /// </summary>
        public static int ToggleBoardSettingFlag(int boardId, string columnName)
        {
            string column = ResolveBoardSettingColumn(columnName, BoardSettingFlagColumns);
            if (boardId <= 0 || column == null)
            {
                LoggerService.Logger.Warning(
                    "ToggleBoardSettingFlag refused: BOARD_ID {BoardId}, column {Column}.", boardId, columnName);
                return -1;
            }

            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    return conn.Execute(
                        "UPDATE BOARDSETTING SET [" + column + "] = CASE WHEN ISNULL([" + column + "], 0) = 1 THEN 0 ELSE 1 END" +
                        " WHERE BOARD_ID = @BoardId;",
                        new { BoardId = boardId });
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "ToggleBoardSettingFlag failed for BOARD_ID " + boardId + ", column " + column + ".");
                return -1;
            }
        }

        /// <summary>The allowed column name exactly as listed (case-insensitive match), or null when it is not allowed.</summary>
        private static string ResolveBoardSettingColumn(string columnName, string[] allowedColumns)
        {
            if (string.IsNullOrWhiteSpace(columnName)) return null;

            return Array.Find(allowedColumns,
                c => string.Equals(c, columnName.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        public static DataTable GetTable(string commandText)
        {
            var table = new DataTable();

            try
            {
                using (var connection = new SqlConnection(Config._dbSelfConnection))
                using (var command = new SqlCommand(commandText, connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    adapter.Fill(table);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, $"Error executing GetTable: {ex.Message}");
            }
            return table;
        }
        public static List<ColourStatus> GetTrainStatusColours()
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                return conn.Query<ColourStatus>(
                    "USP_GET_TRAIN_STATUS_COLOUR",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public static List<LineColourModel> GetLineColours()
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();
                return conn.Query<LineColourModel>(
                    "USP_GET_LINE_COLOURS",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public static void UpdateTrainStatusColour(List<ColourStatus> models)
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();

                foreach (var model in models)
                {
                    conn.Execute(
                        "USP_UPDATE_TRAIN_STATUS_COLOUR",
                        new
                        {
                            STATUS_ENGLISH = model.TrainStatus,
                            StatusColour = model.StatusColour
                        },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
        }
        public static bool UpdateLineColour(List<LineColourModel> models)
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();

                int rows = 0;

                foreach (var model in models)
                {
                    rows += conn.Execute(
                        "USP_UPDATE_LINE_COLOUR",
                        new
                        {
                            Id = model.ID,
                            Colour = model.ColorCode
                        },
                        commandType: CommandType.StoredProcedure
                    );
                }

                return rows > 0;
            }
        }
        public static bool AddLogInHistory(int userId, int userAccessId, string userName)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId, DbType.Int32);
                    parameters.Add("@UserAccessId", userAccessId, DbType.Int32);
                    parameters.Add("@UserName", userName, DbType.String);

                    int loginHistoryId = conn.QuerySingle<int>(
                        "sp_UserLogin",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.UserLoginLogger.Error(ex, $"Error in AddLogInHistory: {ex.Message}");
                return false;
            }
        }
        public static bool AddLogOutHistory(int loggedInUserId)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", loggedInUserId, DbType.Int32);

                    conn.Execute(
                        "sp_UserLogout",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.UserLoginLogger.Error(ex, $"Error in AddLogOutHistory: {ex.Message}");
                return false;
            }
        }
        public static bool Maintain45DaysEventLog()
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    conn.Execute(
                        "dbo.USP_DeleteExpiredRecords",
                        commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, $"Error in Maintain45DaysEventLog: {ex.Message}");
                return false;
            }
        }
        public static bool AddMergeTrainDetails(string trainNumber, string stationCode, string stationName)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@TRAIN_NUMBER", trainNumber, DbType.String);
                    parameters.Add("@STATION_CODE", stationCode, DbType.String);
                    parameters.Add("@STATION_NAME", stationName, DbType.String);

                    conn.Execute(
                        "USP_MERGE_TRAIN_DETAILS",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, $"Error in AddMergeTrainDetails: {ex.Message}");
                return false;
            }
        }
        public static void InsertTrainEntryBulk(Root ntesTrain)
        {
            if (ntesTrain == null)
                return;

            try
            {
                var dt = new DataTable();
                dt.Columns.Add("TrainNumber", typeof(string));
                dt.Columns.Add("TrainName", typeof(string));
                dt.Columns.Add("Src", typeof(string));
                dt.Columns.Add("Dstn", typeof(string));
                dt.Columns.Add("ArrTime", typeof(string)); // Position 5 (0-indexed)
                dt.Columns.Add("DepTime", typeof(string));
                dt.Columns.Add("SArrTime", typeof(string));
                dt.Columns.Add("SDepTime", typeof(string));
                dt.Columns.Add("PfNo", typeof(string));
                dt.Columns.Add("CoachPosition", typeof(string));
                dt.Columns.Add("ADFlag", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("ShortTime", typeof(string));
                dt.Columns.Add("TrainStatus", typeof(string));
                dt.Columns.Add("TrainType", typeof(string));
                dt.Columns.Add("LateStatus", typeof(string));
                dt.Columns.Add("TadbSend", typeof(int));
                dt.Columns.Add("TrainCategory", typeof(string));
                dt.Columns.Add("StnSrNo", typeof(int));
                dt.Columns.Add("trainReversalFlag", typeof(string));
                // ---- Active / running trains (arrivals, departures, passing) ----
                if (ntesTrain.vTrainList != null)
                {
                    foreach (var itemtrain in ntesTrain.vTrainList)
                    {
                        string trainType, status, shortTime, trainStatus;

                        // Arrival row
                        if (itemtrain.ADFlag == "A")
                        {
                            trainType = "ARR";
                            status = "A";
                            shortTime = itemtrain.ETA;
                            trainStatus = itemtrain.isArrived == 1
                                ? "HAS ARRIVED ON"
                                : (string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME")
                                    ? "RUNNING RIGHT TIME"
                                    : "RUNNING LATE";
                        }
                        // Departure-only row
                        else if (itemtrain.ADFlag == "D" && string.IsNullOrEmpty(itemtrain.ETA))
                        {
                            trainType = "DEP";
                            status = "D";
                            shortTime = itemtrain.ETD;
                            trainStatus = itemtrain.isArrived == 1
                                ? "IS READY TO LEAVE"
                                : (string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME")
                                    ? "RUNNING RIGHT TIME"
                                    : "RESCHEDULED";
                        }
                        // Passing-through row
                        else
                        {
                            trainType = "PAS";
                            status = "A";
                            shortTime = itemtrain.ETD;
                            trainStatus = itemtrain.isArrived == 1
                                ? "HAS ARRIVED ON"
                                : (string.IsNullOrEmpty(itemtrain.expectedDelay) || itemtrain.expectedDelay.ToUpper() == "ON TIME")
                                    ? "RUNNING RIGHT TIME"
                                    : "RUNNING LATE";
                        }

                        // Only show a delay reason when the train hasn't arrived AND isn't on time
                        string lateStatus = (itemtrain.isArrived == 1
                                || string.IsNullOrEmpty(itemtrain.expectedDelay)
                                || itemtrain.expectedDelay.ToUpper() == "ON TIME")
                            ? null
                            : itemtrain.expectedDelay;

                        string processedArrTime = itemtrain.STA;

                        if (string.IsNullOrEmpty(itemtrain.STA))
                        {
                            processedArrTime = null; // Will insert as DBNull.Value via Dapper/TVP
                        }
                        else
                        {
                            if (DateTime.TryParseExact(itemtrain.STA, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrDateTime))
                            {
                                bool hasDelay = TimeSpan.TryParse(itemtrain.expectedDelay, out TimeSpan delay) && delay > TimeSpan.Zero;

                                processedArrTime = (hasDelay ? arrDateTime.Add(delay) : arrDateTime)
                                    .ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            // Else: Keep raw string value if fallback is needed
                        }

                        dt.Rows.Add(
                            itemtrain.trainNo ?? string.Empty,
                            itemtrain.trainName?.Trim() ?? string.Empty,
                            itemtrain.src ?? string.Empty,
                            itemtrain.dstn ?? string.Empty,
                            processedArrTime, // Modified processed column value
                            itemtrain.ETD ?? string.Empty,
                            itemtrain.STA ?? string.Empty,
                            itemtrain.STD ?? string.Empty,
                            string.IsNullOrEmpty(itemtrain.platformNo?.Trim()) ? "-" : itemtrain.platformNo.Trim(),
                            itemtrain.coachPosition ?? string.Empty,
                            itemtrain.ADFlag ?? string.Empty,
                            status,
                            shortTime ?? string.Empty,
                            trainStatus ?? string.Empty,
                            trainType,
                            lateStatus ?? string.Empty,
                            0,
                            itemtrain.trainTypeName ?? string.Empty,
                            itemtrain.stnSrNo,
                            itemtrain.trainReversalFlag
                        );
                    }
                }

                // ---- Cancelled trains ----
                if (ntesTrain.vCancelledTrainList != null)
                {
                    foreach (var itemtrain in ntesTrain.vCancelledTrainList)
                    {
                        string trainType = itemtrain.ADFlag == "A" ? "ARR" : itemtrain.ADFlag == "D" ? "DEP" : "PAS";
                        string status = itemtrain.ADFlag == "D" ? "D" : "A";

                        dt.Rows.Add(
                            itemtrain.trainNo ?? string.Empty,
                            itemtrain.trainName?.Trim() ?? string.Empty,
                            itemtrain.src ?? string.Empty,
                            itemtrain.dstn ?? string.Empty,
                            string.Empty,
                            string.Empty,
                            itemtrain.STA ?? string.Empty,
                            itemtrain.STD ?? string.Empty,
                            "-",
                            string.Empty,
                            itemtrain.ADFlag ?? string.Empty,
                            status,
                            itemtrain.STD ?? string.Empty,
                            "CANCELLED",
                            trainType,
                            string.Empty,
                            0,
                            itemtrain.trainTypeName ?? string.Empty,
                            0
                           
                        );
                    }
                }

                if (dt.Rows.Count == 0)
                    return;

                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    conn.Execute(
                        "USP_InsertTrainEntryBulk",
                        new { TrainRawList = dt.AsTableValuedParameter("dbo.TrainRawTableType") },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.NtesLogger.Error(ex, $"InsertTrainEntryBulk failed: {ex.Message}");
            }
        }
        public static bool SaveOrUpdateTrainMasterBulk(List<TrainMaster> trains)
        {
            if (trains == null || trains.Count == 0)
                return false;

            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("TRAIN_NUMBER", typeof(string));
                dt.Columns.Add("TRAIN_NAME_ENGLISH", typeof(string));
                dt.Columns.Add("TRAIN_NAME_HINDI", typeof(string));
                dt.Columns.Add("TRAIN_NAME_REGIONAL", typeof(string));
                dt.Columns.Add("ORIGIN_CODE", typeof(string));
                dt.Columns.Add("DESTINATION_CODE", typeof(string));
                dt.Columns.Add("ARRIVAL_TIME", typeof(string));
                dt.Columns.Add("DEPARTURE_TIME", typeof(string));
                dt.Columns.Add("PLATFORM_NUMBER", typeof(string));
                dt.Columns.Add("RUNNING_DAYS", typeof(string));
                dt.Columns.Add("TRAIN_TYPE", typeof(string));
                dt.Columns.Add("TRAIN_CATEOGARY", typeof(string));
                dt.Columns.Add("VIA1_CODE", typeof(string));
                dt.Columns.Add("VIA2_CODE", typeof(string));
                dt.Columns.Add("VIA3_CODE", typeof(string));
                dt.Columns.Add("VIA4_CODE", typeof(string));
                dt.Columns.Add("STATUS", typeof(string));
                dt.Columns.Add("PF_END", typeof(string));
                dt.Columns.Add("REGIONAL_BITMAP", typeof(string));
                dt.Columns.Add("TRAIN_SORT_TIME", typeof(string));
                dt.Columns.Add("COACH_SEQUENCE", typeof(string));

                foreach (var train in trains)
                {
                    dt.Rows.Add(
                        train.TRAIN_NUMBER ?? string.Empty,
                        train.TRAIN_NAME_ENGLISH ?? string.Empty,
                        train.TRAIN_NAME_HINDI ?? string.Empty,
                        train.TRAIN_NAME_REGIONAL ?? string.Empty,
                        train.ORIGIN_CODE ?? string.Empty,
                        train.DESTINATION_CODE ?? string.Empty,
                        train.ARRIVAL_TIME ?? string.Empty,
                        train.DEPARTURE_TIME ?? string.Empty,
                        train.PLATFORM_NUMBER ?? string.Empty,
                        train.RUNNING_DAYS ?? string.Empty,
                        train.TRAIN_TYPE ?? string.Empty,
                        train.TRAIN_CATEOGARY ?? string.Empty,
                        train.VIA1_CODE ?? string.Empty,
                        train.VIA2_CODE ?? string.Empty,
                        train.VIA3_CODE ?? string.Empty,
                        train.VIA4_CODE ?? string.Empty,
                        train.STATUS ?? string.Empty,
                        train.PF_END ?? string.Empty,
                        train.REGIONAL_BITMAP ?? string.Empty,
                        train.TRAIN_SORT_TIME ?? string.Empty,
                        train.COACH_SEQUENCE ?? string.Empty
                    );
                }

                using (SqlConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@TrainMasters", dt.AsTableValuedParameter("dbo.TrainMasterUDT"));

                    conn.Execute(
                        "USP_SAVE_OR_UPDATE_TRAINMASTER_BULK",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                }

                return true;
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
                return false;
            }
        }
        public static List<SingleLineDisplayBoardData> GetAllActiveBoards()
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var boards = conn.Query<SingleLineDisplayBoardData>(
                        "sp_GetAllSingleLineDisplayBoards",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return boards;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error getting all active boards");
                throw;
            }
        }

        public static List<AGDBDisplayBoardData> GetAllActiveBoardsForAGDB()
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    var boards = conn.Query<AGDBDisplayBoardData>(
                        "sp_GetAllSingleLineDisplayBoards",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return boards;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error getting all active boards");
                throw;
            }
        }


        public static DataTable GET_TRUE_COLOR_DATA(string userId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (IDbConnection conn =
                    new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new
                    {
                        UserId = userId
                    };

                    using (var reader = conn.ExecuteReader(
                        "USP_GET_TRUE_COLOR_DATA",
                        parameters,
                        commandType: CommandType.StoredProcedure))
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }

            return dt;
        }
        public static bool CHECK_TRAIN_INFO_EXISTS(int currentId = -1)
        {
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(Config._dbSelfConnection))
                {
                    int count = conn.QueryFirstOrDefault<int>(
                        "USP_CHECK_TRAIN_INFO_EXISTS",
                        new
                        {
                            ID = currentId
                        },
                        commandType: CommandType.StoredProcedure);

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return false;
            }
        }

        public static int GET_TRUE_COLOR_NEXT_ID(string userId)
        {
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(Config._dbSelfConnection))
                {
                    return conn.QueryFirstOrDefault<int>(
                        "USP_GET_TRUE_COLOR_NEXT_ID",
                        new
                        {
                            UserId = userId
                        },
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return 1;
            }
        }
        public static bool DELETE_TRUE_COLOR(int id)
        {
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(Config._dbSelfConnection))
                {
                    int result = conn.QueryFirstOrDefault<int>(
                        "USP_DELETE_TRUE_COLOR",
                        new
                        {
                            ID = id
                        },
                        commandType: CommandType.StoredProcedure);

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return false;
            }
        }
        public static bool INSERT_TRUE_COLOR(TrueColor _postdata)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var param = new
                    {
                        ID = _postdata.ID,
                        AdvType = _postdata.AdvType,
                        FilePath = _postdata.FilePath,
                        RepeatCount = _postdata.RepeatCount,
                        MessageEnglish = _postdata.MessageEnglish,
                        MessageHindi = _postdata.MessageHindi,
                        MessageRegional = _postdata.MessageRegional,
                        PlayDuration = _postdata.PlayDuration,
                        IsDeleted = _postdata.IsDeleted,
                        DisplayStatus = _postdata.DisplayStatus,
                        BottomMessage = _postdata.BottomMessage,
                        ShowTime = _postdata.ShowTime,
                        UserId = _postdata.UserId,
                        UserName = _postdata.UserName
                    };

                    int result = conn.QueryFirstOrDefault<int>(
                        "USP_INSERT_TRUE_COLOR",
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result > 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }

            return false;
        }
        public static bool UPDATE_TRUE_COLOR(TrueColor _postdata)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var param = new
                    {
                        ID = _postdata.ID,
                        AdvType = _postdata.AdvType,
                        FilePath = _postdata.FilePath,
                        RepeatCount = _postdata.RepeatCount,
                        MessageEnglish = _postdata.MessageEnglish,
                        MessageHindi = _postdata.MessageHindi,
                        MessageRegional = _postdata.MessageRegional,
                        PlayDuration = _postdata.PlayDuration,
                        IsDeleted = _postdata.IsDeleted,
                        DisplayStatus = _postdata.DisplayStatus,
                        BottomMessage = _postdata.BottomMessage,
                        ShowTime = _postdata.ShowTime,
                        UserId = _postdata.UserId,
                        UserName = _postdata.UserName
                    };

                    int result = conn.QueryFirstOrDefault<int>(
                        "USP_UPDATE_TRUE_COLOR",
                        param,
                        commandType: CommandType.StoredProcedure);

                    if (result > 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }

            return false;
        }
        public static DataTable GET_PLAYLIST_DATA(int playlistId = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    using (var reader = conn.ExecuteReader(
                        "USP_GET_PLAYLIST_DATA",
                        new { PlaylistID = playlistId },
                        commandType: CommandType.StoredProcedure))
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
            }

            return dt;
        }
      
        public static int SAVE_PLAYLIST(TrueColorPlaylist playlist)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    int res = conn.QueryFirstOrDefault<int>(
                        "USP_SAVE_PLAYLIST",
                        new
                        {
                            ID = playlist.ID,
                            PlaylistName = playlist.PlaylistName,
                            PlaylistType = playlist.PlaylistType,
                            IsActive = playlist.IsActive,
                            Repeat = playlist.Repeat,
                            StartDateTime = playlist.StartDateTime,
                            EndDateTime = playlist.EndDateTime,
                            ItemIDs = playlist.ItemIDs,
                            UserId = playlist.UserId,
                            UserName = playlist.UserName,
                            TVLineCount = playlist.TVLineCount
                        },
                        commandType: CommandType.StoredProcedure);

                    return res;
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return 0;
            }
        }
        public static bool DELETE_PLAYLIST(int playlistId)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    int affectedRows = conn.Execute(
                        @"DELETE FROM dbo.TrueColorPlaylist
                  WHERE ID = @playlistId",
                        new { playlistId },
                        commandType: CommandType.Text);

                    return affectedRows > 0;
                }

            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return false;
            }
        }
        public static bool SET_ACTIVE_PLAYLIST(int playlistId)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    int result = conn.QueryFirstOrDefault<int>(
                        "USP_SET_ACTIVE_PLAYLIST",
                        new { PlaylistID = playlistId },
                        commandType: CommandType.StoredProcedure);

                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return false;
            }
        }
        public static bool MANAGE_PLAYLIST_ITEM(string action, int playlistId, int trueColorId)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    int result = conn.QueryFirstOrDefault<int>(
                        "USP_MANAGE_PLAYLIST_ITEM",
                        new
                        {
                            Action = action,
                            PlaylistID = playlistId,
                            TrueColorID = trueColorId
                        },
                        commandType: CommandType.StoredProcedure);
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return false;
            }
        }
        public static List<TrueColorPlaylist> GetAllPlaylists(string userId)
        {
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    return conn.Query<TrueColorPlaylist>(
                        "USP_GET_ALL_PLAYLISTS",
                        new
                        {
                            UserId = userId
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return new List<TrueColorPlaylist>();
            }
        }
        public static int SaveMaskTrains(string trainNo, string TrainName, string Remarks)
        {
            using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();
                return conn.Execute(
                    "USP_SAVE_TRAIN_MASKING",
                    new { TrainNumber = trainNo, TrainName = TrainName, Remarks = Remarks },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public static List<APILiveDataLog> GetHSRList(string deviceType)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    return conn.Query<APILiveDataLog>(
                        "USP_GET_HSR_LIST",
                        new { DeviceType = deviceType },
                        commandType: CommandType.StoredProcedure)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.HsrLogger.Error(ex, ex.Message);
                return new List<APILiveDataLog>();
            }
        }

        public static string GetCoachSequence(string trainNumber)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    return conn.QueryFirstOrDefault<string>(
                        "USP_GET_COACH_SEQUENCE",
                        new { TrainNumber = trainNumber },
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return string.Empty;
            }
        }

        public static async Task<List<PlaylistData>> GetPlaylistDataAsync(int playlistId)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var playlists = await conn.QueryAsync<PlaylistData>(
                        "USP_GET_PLAYLIST_DATA",
                        new
                        {
                            PlaylistID = playlistId
                        },
                        commandType: CommandType.StoredProcedure);

                    return playlists.ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return new List<PlaylistData>();
            }
        }


        public static async Task<LiveDataResponse?> GetLiveDataDisplayAsync( LiveDataRequest request, string rawJson)
        {
            try
            {
                var device = request.Device?.FirstOrDefault();

                if (device == null)
                {
                    LoggerService.Logger.Warning(
                        "GetLiveDataDisplayAsync: Device information is missing.");

                    return null;
                }

                using (IDbConnection conn =
                       new SqlConnection(Config._dbSelfConnection))
                {
                    await ((SqlConnection)conn).OpenAsync();

                    using var multi = await conn.QueryMultipleAsync(
                        "USP_GET_LIVE_DATA",
                        new
                        {
                            PlatformNo = device.PF,
                            DeviceType = "SLDB",
                            DeviceIP = device.IP
                        },
                        commandType: CommandType.StoredProcedure);

                    var trains = (await multi.ReadAsync<TrainDto>())
                        .ToList();

                    var config = (await multi.ReadAsync<LineConfDto>())
                        .ToList();

                    return new LiveDataResponse
                    {
                        TrainList = trains,
                        LineConf = config
                    };
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error while getting live display data.");

                return null;
            }
        }

        public static async Task<LiveDataResponse?> GetLiveDataDisplayAsyncForAGDB(LiveDataRequest request, string rawJson)
        {
            try
            {
                if (request.Device == null || request.Device.Count == 0)
                {
                    LoggerService.Logger.Warning(
                        "GetLiveDataDisplayAsync: Device information is missing.");

                    return null;
                }

                using (IDbConnection conn =
                       new SqlConnection(Config._dbSelfConnection))
                {
                    await ((SqlConnection)conn).OpenAsync();

                    var distinctDevices = request.Device
                        .Where(d => d != null)
                        .GroupBy(d => new { PF = (d.PF ?? "").Trim(), IP = (d.IP ?? "").Trim() })
                        .Select(g => g.First())
                        .ToList();

                    var allTrains = new List<TrainDto>();
                    var allConfig = new List<LineConfDto>();
                    var seenTrainKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var device in distinctDevices)
                    {
                        using var multi = await conn.QueryMultipleAsync(
                            "USP_GET_LIVE_DATA",
                            new
                            {
                                PlatformNo = device.PF,
                                DeviceType = "AGDB",
                                DeviceIP = device.IP
                            },
                            commandType: CommandType.StoredProcedure);

                        var trains = (await multi.ReadAsync<TrainDto>()).ToList();
                        var config = (await multi.ReadAsync<LineConfDto>()).ToList();

                        if (trains != null)
                        {
                            foreach (var t in trains)
                            {
                                string key = $"{t.TNO}_{t.PNO}";
                                if (seenTrainKeys.Add(key))
                                {
                                    allTrains.Add(t);
                                }
                            }
                        }

                        if (allConfig.Count == 0 && config != null && config.Count > 0)
                        {
                            allConfig.AddRange(config);
                        }
                    }

                    return new LiveDataResponse
                    {
                        TrainList = allTrains,
                        LineConf = allConfig
                    };
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error while getting live display data.");

                return null;
            }
        }

        public static async Task<LiveDataResponse?> GetLiveDataDisplayAsyncForMLDB(LiveDataRequest request, string rawJson)
        {
            try
            {
                var device = request.Device?.FirstOrDefault();

                if (device == null)
                {
                    LoggerService.Logger.Warning(
                        "GetLiveDataDisplayAsync: Device information is missing.");

                    return null;
                }

                using (IDbConnection conn =
                       new SqlConnection(Config._dbSelfConnection))
                {
                    await ((SqlConnection)conn).OpenAsync();

                    using var multi = await conn.QueryMultipleAsync(
                        "USP_GET_LIVE_DATA",
                        new
                        {
                            PlatformNo = device.PF,
                            DeviceType = "SLDB",
                            DeviceIP = device.IP
                        },
                        commandType: CommandType.StoredProcedure);

                    var trains = (await multi.ReadAsync<TrainDto>())
                        .ToList();

                    var config = (await multi.ReadAsync<LineConfDto>())
                        .ToList();

                    return new LiveDataResponse
                    {
                        TrainList = trains,
                        LineConf = config
                    };
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error while getting live display data.");

                return null;
            }
        }

        public static (string TrainInfo, string Messages) GET_BOARD_DETAILS_HealthStatus(
    string _boardtype,
    string BOARD_IP)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new
                    {
                        boardtype = _boardtype,
                        BOARD_IP = BOARD_IP
                    };

                    var result = conn.QueryFirstOrDefault<(string TrainInfo, string Messages)>(
                        "USP_GET_BOARD_DETAILS_HEALTH",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return (string.Empty, string.Empty);
            }
        }
        public static int ADD_ANNOUNCEMENT(Announcement announcement)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new
                    {
                        USER_NAME = announcement.USER_NAME,
                        USER_TYPE = announcement.USER_TYPE,
                        TRAIN_NUMBER = announcement.TRAIN_NUMBER,
                        TRAIN_NAME_ENGLISH = announcement.TRAIN_NAME_ENGLISH,
                        ARRIVAL_TIME = announcement.ARRIVAL_TIME,
                        DEPARTURE_TIME = announcement.DEPARTURE_TIME,
                        TRAIN_STATUS_ENGLISH = announcement.TRAIN_STATUS_ENGLISH,
                        PLATFORM_NUMBER = announcement.PLATFORM_NUMBER,
                        TRAIN_DATE = announcement.TRAIN_DATE
                    };

                    int id = conn.QuerySingle<int>(
                        "USP_ADD_LOGANNOUNCE",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return id;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return 0;
            }
        }

        public static int ADD_LOGDISPLAYBOARD(string userName,string userType,string trainNumber,int tadbSendData,DateTime trainDate,string vendorName)
        {
            try
            {
                using (var conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new
                    {
                        USER_NAME = userName,
                        USER_TYPE = userType,
                        TRAIN_NUMBER = trainNumber,
                        TADB_SEND_DATA = tadbSendData,
                        TRAIN_DATE = trainDate,
                        VENDOR_NAME = vendorName
                    };

                    return conn.ExecuteScalar<int>(
                        "USP_ADD_LOGDISPLAYBOARD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
                return 0;
            }
        }

        public static int LOGCOACHSEQUENCE(string userName, string userType, string trainNumber, int cgdbSendData, DateTime trainDate)
        {
            using (var conn = new SqlConnection(Config._dbSelfConnection))
            {
                conn.Open();

                var parameters = new
                {
                    USER_NAME = userName,
                    USER_TYPE = userType,
                    TRAIN_NUMBER = trainNumber,
                    CGDB_SEND_DATA = cgdbSendData,  
                    TRAIN_DATE = trainDate,
                    VENDOR_NAME =  "VARDAAN"
                };

                return conn.ExecuteScalar<int>(
                    "USP_ADD_LOGCOACHSEQUENCE",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static async Task<int?> InsertMusicAsync(string musicTitle,string soundFile)
        {
            try
            {
                using IDbConnection conn =
                    new SqlConnection(Config._dbSelfConnection);

                await ((SqlConnection)conn).OpenAsync();

                var parameters = new
                {
                    MUSIC_TITLE = musicTitle,
                    SOUND_FILE = soundFile
                };

                var id = await conn.ExecuteScalarAsync<int?>(
                    "USP_INSERT_MUSIC",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return id;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error while inserting music.");

                return null;
            }
        }
        public static async Task<List<MusicList>> GetMusicListAsync()  
        {
            try
            {
                using IDbConnection conn = new SqlConnection(Config._dbSelfConnection);
                await ((SqlConnection)conn).OpenAsync();
                var result = await conn.QueryAsync<MusicList>(
                    "USP_GET_MUSIC_LIST",
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Error while getting music list.");

                return new List<MusicList>();
            }
        }

        public static async Task<int> SaveVendorDetail(Vendor vendor)
        {
            try
            {
                using IDbConnection conn = new SqlConnection(Config._dbSelfConnection);
                await ((SqlConnection)conn).OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", vendor.Id, DbType.Int32);
                parameters.Add("@VendorName", vendor.VendorName, DbType.String, size: 255);
                parameters.Add("@VendorCode", vendor.VendorCode, DbType.String, size: 100);
                parameters.Add("@IsActive", vendor.IsActive, DbType.Boolean);

                return await conn.ExecuteScalarAsync<int>(
                    "USP_SAVE_VENDOR_DETAILS",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error while saving vendor detail.");
                return -9;
            }
        }

        public static async Task<List<Vendor>> GetVendorDetails()
        {
            try
            {
                using IDbConnection conn = new SqlConnection(Config._dbSelfConnection);
                await ((SqlConnection)conn).OpenAsync();

                var result = await conn.QueryAsync<Vendor>(
                    "USP_GET_VENDOR_DETAILS",
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error while getting vendor details.");
                return new List<Vendor>();
            }
        }
        public static async Task<bool> MergeTrainCoachSequence(string mergingTrainNumber, string selectedTrainNumber,int position, string mergedTrainEnglish,string mergedTrainHindi, string mergedTrainRegional)
        {
            try
            {
                using var conn = new SqlConnection(Config._dbSelfConnection);
                await conn.OpenAsync();

                var result = await conn.QueryFirstOrDefaultAsync<int>(
                    "USP_MERGE_TRAIN_COACH_SEQUENCE",
                    new
                    {
                        MergingTrainNumber = mergingTrainNumber,
                        SelectedTrainNumber = selectedTrainNumber,
                        Position = position,
                        MergedTrainEnglish = mergedTrainEnglish,
                        MergedTrainHindi = mergedTrainHindi,
                        MergedTrainRegional = mergedTrainRegional

                    },
                    commandType: CommandType.StoredProcedure
                );

                return result == 1;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Error while merging train coach sequence.");
                return false;
            }
        }

        public static int ACTIVE_PLAYLIST_FOR_LOGIN_USER(string userId,string userName)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    int res = conn.QueryFirstOrDefault<int>(
                        "USP_ACTIVE_PLAYLIST_FOR_LOGIN_USER",
                        new
                        {
                            UserId = userId,
                            UserName = userName,
                        },
                        commandType: CommandType.StoredProcedure);

                    return res;
                }
            }
            catch (Exception ex)
            {
                LoggerService.VdbLogger.Error(ex, ex.Message);
                return 0;
            }
        }
        public static void InsertTrainMasterLog(TrainMaster train)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    conn.Execute(
                        "USP_INSERT_TRAIN_MASTER_LOG",
                        new
                        {
                            TRAIN_NUMBER = train.TRAIN_NUMBER,
                            TRAIN_NAME_ENGLISH = train.TRAIN_NAME_ENGLISH,
                            DESTINATION_CODE = train.DESTINATION_CODE,
                            ARRIVAL_TIME = train.ARRIVAL_TIME,
                            DEPARTURE_TIME = train.DEPARTURE_TIME,
                            PLATFORM_NUMBER = train.PLATFORM_NUMBER,
                            RUNNING_DAYS = train.RUNNING_DAYS,
                            TRAIN_TYPE = train.TRAIN_TYPE,
                            TRAIN_CATEOGARY = train.TRAIN_CATEOGARY,
                            STATUS = train.STATUS
                        },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                LoggerService.SettingsLogger.Error(ex, ex.Message);
            }
        }

        public static int GetMaintinanceDetails()
        {
            try
            {
                using (IDbConnection conn =
                       new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@OnlyActiveBoards", true, DbType.Boolean);

                    int res = conn.QueryFirstOrDefault<int>(
                        "dbo.USP_CREATE_SORTLED_MATRIX_DETAILS",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return res;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Failed to execute USP_CREATE_SORTLED_MATRIX_DETAILS.");

                throw;
            }
        }



        public static string ADDUPDATECGDBASSETID(
            string ip,
            string deviceAssetId,
            string firmwareId,
            int intensity,
            int portNo,
            int dto)
        {
            try
            {
                using (IDbConnection conn =
                       new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var parameters = new DynamicParameters();

                    parameters.Add("@IP", ip);
                    parameters.Add("@DEVICE_ASSET_ID", deviceAssetId);
                    parameters.Add("@FIRMWARE_ID", firmwareId);
                    parameters.Add("@INTENSITY", intensity);
                    parameters.Add("@PORT_NO", portNo);
                    parameters.Add("@DTO", dto);

                    return conn.QueryFirstOrDefault<string>(
                        "dbo.USP_ADD_UPDATE_COACHCONFIGURATION",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(
                    ex,
                    "Failed to update CGDB configuration.");

                throw;
            }
        }

        public static List<CgdbBoardInfo> GetCgdbBoardIps()
        {
            using (IDbConnection conn =
                   new SqlConnection(Config._dbSelfConnection))
            {
                const string query = @"
            SELECT DISTINCT
                LTRIM(RTRIM(IP)) AS IP,
                LTRIM(RTRIM(DEVICE_ASSET_ID)) AS DEVICE_ASSET_ID
            FROM dbo.COACHCONFIGURATION
            WHERE
                PORT_TYPE = 'CGDB'
                AND NULLIF(LTRIM(RTRIM(IP)), '') IS NOT NULL
                AND NULLIF(LTRIM(RTRIM(DEVICE_ASSET_ID)), '') IS NOT NULL
            ORDER BY IP;";

                conn.Open();

                return conn.Query<CgdbBoardInfo>(query).ToList();
            }
        }

        public static bool SyncCgdbCoachConfiguration(int pfNo, string selectedPf, int? oldPfNo = null, int? boardId = null, int? hubId = null)
        {
            return ExecuteCgdbCoachConfiguration(pfNo, selectedPf, oldPfNo, false, boardId, hubId);
        }

     
        public static bool DeleteCgdbCoachConfiguration(int pfNo, int? oldPfNo = null, int? boardId = null)
        {
            return ExecuteCgdbCoachConfiguration(pfNo, null, oldPfNo, true, boardId, null);
        }

        private static bool ExecuteCgdbCoachConfiguration(int pfNo, string selectedPf, int? oldPfNo, bool deleteAll, int? boardId = null, int? hubId = null)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();

                    var result = conn.QueryFirstOrDefault<CoachConfigurationSyncResult>(
                        "USP_SYNC_CGDB_COACH_CONFIGURATION",
                        new
                        {
                            PF_NO = pfNo,
                            SELECTED_PF = selectedPf,
                            OLD_PF_NO = oldPfNo,
                            DELETE_ALL = deleteAll,
                            BOARD_ID = boardId,
                            HUB_ID = hubId
                        },
                        commandType: CommandType.StoredProcedure);

                    LoggerService.Logger.Information(
                        "CGDB coach configuration PF {Pf} (board {BoardId}, hub {HubId}, old PF {OldPf}, delete all {DeleteAll}): inserted {Inserted}, updated {Updated}, deleted {Deleted}.",
                        pfNo, boardId, hubId, oldPfNo, deleteAll, result?.Inserted ?? 0, result?.Updated ?? 0, result?.Deleted ?? 0);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "USP_SYNC_CGDB_COACH_CONFIGURATION failed for PF " + pfNo + ".");
                return false;
            }
        }

        public class CgdbBoardInfo
        {
            public string IP { get; set; }
            public string DEVICE_ASSET_ID { get; set; }
        }

    }   
}

