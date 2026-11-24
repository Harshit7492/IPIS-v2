using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Data
{
    public static class Appdata
    {
        public static int currentnodeid { get; set; }
        public static string currentnodename { get; set; }
        public static bool IsParent { get; set; }
        public static int childNodeID { get; set; }
        public static int BOARD_ID { get; set; }
        public static int parentNodeID { get; set; }
        public static string boardName { get; set; }
        public static string boardtype { get; set; }
        public static string parentpf { get; set; }
    }

}
