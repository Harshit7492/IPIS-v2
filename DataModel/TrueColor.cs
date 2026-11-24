using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TrueColor
    {
        public int ID { get; set; }

        public string AdvType { get; set; }
        public string FilePath { get; set; }

        public string MessageEffect { get; set; }

        public int PlayDuration { get; set; }
        public int RepeatCount { get; set; }

        public int DisplayOrder { get; set; }

        public int IsDeleted { get; set; }

        public string MessageFont { get; set; }

        public string MessageColor { get; set; }

        public int PlaylistID { get; set; }

        public string MessageEnglish { get; set; }

        public string MessageHindi { get; set; }

        public string MessageRegional { get; set; }

        public bool DisplayStatus { get; set; } = true;

        public bool BottomMessage { get; set; } = false;

        public bool ShowTime { get; set; } = false;
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    public class TrueColorPlaylist
    {
        public int ID { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ItemIDs { get; set; }
        public int Repeat { get; set; } = 1;
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TVLineCount { get; set; }
    }

    public class TrueColorPlaylistItem
    {
        public int ID { get; set; }
        public int PlaylistID { get; set; }
        public int TrueColorID { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
