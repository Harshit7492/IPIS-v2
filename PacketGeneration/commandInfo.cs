using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGeneration
{
    public static class commandInfo
    {
        //43673 AA99 
        //43724 AACC 
        public static int _startcommand = 43724;
        public static int _sourceMSB = 43673;
        public static int _intensityMSB = 65024;
        public static int _defaultData = 78;
        public static int _defaultSet = 79;
        public static int _linkCheck = 80;
        public static int _dataTransfer = 81;
        public static int _stop = 82;
        public static int _start = 83;
        public static int _setConfiguration = 84;
        public static int _getConfiguration = 85;
        public static int _softReset = 86;
        public static int _learResetStatus = 87;
        public static int _previousCommandStatus = 88;
        public static int _protocolError = 89;
        public static int _diagnosisCommand = 138;
        public static int _optional = 139;
        public static int _ledOn = 90;
        public static int _ledOff = 91;
        public static int _altRowOn = 92;
        public static int _altColOn = 93;
        public static int _altDiagOn = 94;
        public static int _uploadTrain = 95;
        public static int _bitmapStart = 96;
        public static int _bitmapStop = 97;
        public static int _liveCheck = 99;

    }
}
