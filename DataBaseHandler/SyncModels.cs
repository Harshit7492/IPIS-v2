using System;
using System.Collections.Generic;

namespace IPISv2.Sync
{
    public class SyncChange
    {
        public long ChangeId { get; set; }

        public string TableName { get; set; }

        public string PrimaryKeyValue { get; set; }

        public string Operation { get; set; }

        public DateTime ChangedAt { get; set; }

        public string ChangedBy { get; set; }

        // Data is required for INSERT / UPDATE.
        // It is null for DELETE.
        public Dictionary<string, object> Data { get; set; }
            = new Dictionary<string, object>();
    }


    public class SyncBatch
    {
        public string SourceNodeId { get; set; }

        public List<SyncChange> Changes { get; set; }
            = new List<SyncChange>();
    }


    public class SyncResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string NodeId { get; set; }

        public int Received { get; set; }

        public int Applied { get; set; }

        public int Ignored { get; set; }

        public long LastReceivedChangeId { get; set; }
    }


    public class SyncTableDefinition
    {
        public string TableName { get; set; }

        public string PrimaryKeyColumn { get; set; }

        public bool IsIdentity { get; set; }
    }


    public class SyncResult
    {
        public int Received { get; set; }

        public int Applied { get; set; }

        public int Ignored { get; set; }

        public long LastChangeId { get; set; }

        public string Message { get; set; }
    }
}