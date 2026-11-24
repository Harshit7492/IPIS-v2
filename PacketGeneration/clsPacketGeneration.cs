using DataBaseHandler;
using DataModel;
using Helper;
using LogManager;
using System.Collections;
using System.Net;


namespace PacketGeneration
{
    public class clsPacketGeneration
    {
        private List<train_updated_data> lstAvailabletrains = new List<train_updated_data>();
        private static readonly Lazy<clsPacketGeneration> _instance =
        new Lazy<clsPacketGeneration>(() => new clsPacketGeneration(),
                                      System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        private clsPacketGeneration()
        {
        }
        public static clsPacketGeneration Instance => _instance.Value;

        private List<string> lst_packetsdata = new List<string>();
        private List<string> lst_packetsdisplaytime = new List<string>();

        private List<string> PacketHeaders(BoardInfo _BoardInfo)
        {
            List<string> packetsHeader = new List<string>();
            try
            {
                int line = 1;
                packetsHeader.Clear();
                packetsHeader.Add(commandInfo._startcommand.ToString("X4"));
                if (_BoardInfo.BOARD_NAME == "At A Glance Display Board")
                    packetsHeader.Add("04");
                else if (_BoardInfo.BOARD_NAME == "MultiLine Display Board")
                    packetsHeader.Add("05");
                else if (_BoardInfo.BOARD_NAME == "Single Line / Platform Board")
                    packetsHeader.Add("05");
                else
                    packetsHeader.Add("00");

                var bytes = IPAddress.Parse(_BoardInfo.BOARD_IP).GetAddressBytes();
                packetsHeader.Add(bytes[2].ToString("X2"));
                packetsHeader.Add(bytes[3].ToString("X2"));
                packetsHeader.Add("00FD");
                packetsHeader.Add(line.ToString("X").PadLeft(2, '0'));
                packetsHeader.Add(commandInfo._dataTransfer.ToString());

                packetsHeader.Add("02");
                return packetsHeader;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, ex.Message);
            }
            return packetsHeader;
        }

        public void ASSEMBLE_TADB_DATA(List<BoardInfo> _lstboards)
        {
            lstAvailabletrains = _dbService.GET_UPDATED_TRAINS_DATA(0);

            foreach (var itemboard in _lstboards)
            {
                lst_packetsdata.AddRange(PacketHeaders(itemboard));
                if (itemboard.BOARD_NAME == "At A Glance Display Board")
                {
                    foreach (var itemtrain in lstAvailabletrains)
                    {
                        lst_packetsdata.AddRange(MakeDisplayData(itemtrain, itemboard));
                    }
                }
            }
        }
        private string InsertOffsetAndLength(ArrayList PacketList, List<string> packetsdetails, string RightColumn, BoardInfo _boardInfo, List<string> DisplayTimeListData)
        {
            int OffsetPos = 8;
            int PacketLengthPos;
            String Char2Convert = "";

            int first = 0;
            String Data = "";
            Int32 HexValue;
            String PacketData = "";
            first = (14 * PacketList.Count) + 2;

            for (int PacketIndex = 0; PacketIndex < PacketList.Count; PacketIndex++)
            {
                int indexvalue = 0;
                if (PacketIndex % 2 == 0)
                    indexvalue = 1;
                else indexvalue = 2;
                packetsdetails.Insert(OffsetPos++, "0001" + Convert.ToInt32(RightColumn).ToString("X").PadLeft(4, '0') + "00" + Convert.ToInt32(indexvalue).ToString("X").PadRight(2, '0') + "0001" + _boardInfo.VIDEO_TYPE + _boardInfo.DISPLAY_EFFECT + _boardInfo.LETTER_GAP + Convert.ToInt32(Convert.ToInt16(DisplayTimeListData[PacketIndex].ToString())).ToString("X").PadLeft(2, '0') + (Convert.ToInt32(first).ToString("X").PadLeft(4, '0')));
                first = first + (PacketList[PacketIndex].ToString().Length) / 2;
            }
            packetsdetails.Insert(OffsetPos, "FFFF");

            Data = "";
            char c = '\0';

            for (int Index = 4; Index < packetsdetails.Count; Index++)
                Data = Data + packetsdetails[Index];
            packetsdetails.Insert(2, (Convert.ToInt32((Data.Length + 2) / 2).ToString("X").PadLeft(4, '0')));

            Data = "";
            for (int Index = 2; Index < packetsdetails.Count; Index++)
                Data = Data + packetsdetails[Index];

            Data = Data.Replace(" ", "");
            Data = Data.Replace("\n\t", "");
            Data = Data.Trim();

            for (PacketLengthPos = 8; PacketLengthPos < Data.Length; PacketLengthPos = PacketLengthPos + 2)
            {
                Char2Convert = Data.Substring(PacketLengthPos, 2);
                HexValue = Convert.ToInt32(Char2Convert, 16);
                c = (char)HexValue;
                PacketData = PacketData + c.ToString();

            }
            return PacketData;
        }

        private List<string> MakeDisplayData(train_updated_data _trainInfo, BoardInfo _boardInfo)
        {
            string TermEnd = "FFFF";
            string DummyDataE = "1000";
            string DummyDataH = "0900";
            string DummyDataR = "0A00";
            List<string> _lstdata = new List<string>();
            foreach (string itemlanguage in _boardInfo.selectedlanguage)
            {
                string _pfnumber = _trainInfo.ACTUAL_PLATFORM_NUMBER;
                if (_pfnumber.Length < 2 && _pfnumber != "-")
                    _pfnumber = "0" + _pfnumber;
                else
                    _pfnumber = "--";
                if (_boardInfo.BOARD_NAME == "AGDB")
                {
                    if (itemlanguage == "English")
                    {
                        int LineNo = 1;
                        string DataEnglish = AppHelper.ConvertIntoHex(_trainInfo.TRAIN_NUMBER) + "E700" + "0A00" + GetCoachData(_trainInfo.COACH_SEQUENCE, LineNo);
                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);

                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));
                        LineNo = 2;
                        DataEnglish = ("00200020E7000A00" + GetCoachData(_trainInfo.COACH_SEQUENCE, LineNo));
                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);
                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));


                        DataEnglish = AppHelper.ConvertIntoHex(_trainInfo.TRAIN_NUMBER) + "E700" + DummyDataH;
                        DataEnglish += AppHelper.ConvertIntoHex(string.Format("{0:HH:mm}", _trainInfo.ACTUAL_ARRIVAL_TIME)) + "E700" + DummyDataE;
                        DataEnglish += AppHelper.ConvertIntoHex(_trainInfo.TRAIN_STATUS_CODE) + "E700" + DummyDataE;
                        DataEnglish += AppHelper.ConvertIntoHex(_pfnumber) + "E700" + DummyDataE;

                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);
                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));

                        DataEnglish = AppHelper.ConvertIntoHex(_trainInfo.TRAIN_NAME_ENGLISH) + "E700" + DummyDataH;
                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);
                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));
                    }
                    else if (itemlanguage == "Hindi")
                    {
                        int LineNo = 1;
                        string DataEnglish = AppHelper.ConvertIntoHex(_trainInfo.TRAIN_NUMBER) + "E700" + "0A00" + GetCoachData(_trainInfo.COACH_SEQUENCE, LineNo);
                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);

                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));
                        LineNo = 2;
                        DataEnglish = ("00200020E7000A00" + GetCoachData(_trainInfo.COACH_SEQUENCE, LineNo));
                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);
                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));


                        DataEnglish = AppHelper.ConvertIntoHex(_trainInfo.TRAIN_NUMBER) + "E700" + DummyDataH;
                        DataEnglish += AppHelper.ConvertIntoHex(string.Format("{0:HH:mm}", _trainInfo.ACTUAL_ARRIVAL_TIME)) + "E700" + DummyDataE;
                        DataEnglish += AppHelper.ConvertIntoHex(_trainInfo.TRAIN_STATUS_CODE) + "E700" + DummyDataE;
                        DataEnglish += AppHelper.ConvertIntoHex(_pfnumber) + "E700" + DummyDataE;

                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);
                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));

                        DataEnglish = AppHelper.ConvertIntoHex(_trainInfo.TRAIN_NAME_HINDI) + "E700" + DummyDataH;
                        _lstdata.Add("0000" + "0000" + DataEnglish + TermEnd);
                        lst_packetsdisplaytime.Add(Convert.ToString(_boardInfo.DELAYTIME));
                    }
                    else
                    {

                    }
                }
            }
            return _lstdata;
        }

        private string GetCoachData(string _coachSequence, int LineNo)
        {
            ArrayList CoachData = new ArrayList();

            int StartCoach = 0;
            int CoachNo = 14;
            if (LineNo == 1)
            {
                StartCoach = 0;
                CoachNo = 14;
            }
            else
            {
                StartCoach = 14;
                CoachNo = 28;
            }
            CoachData.AddRange(_coachSequence.Split('-'));
            string CoachHex = string.Empty;
            for (int IndexCoach = StartCoach; IndexCoach < CoachNo; IndexCoach++)
            {
                try
                {
                    if (CoachData.Count < IndexCoach)
                        CoachHex += "E700";
                    else if (string.IsNullOrEmpty(Convert.ToString(CoachData[IndexCoach]).Trim()))
                        CoachHex += "E700";
                    else
                        CoachHex += AppHelper.ConvertIntoHex(CoachData[IndexCoach].ToString().Trim()) + "E700";
                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex, ex.Message);
                }
            }
            return CoachHex;
        }
    }
}
