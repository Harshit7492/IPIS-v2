using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHandler
{
    using global::IPISv2.Sync;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    namespace IPISv2.Sync
    {
        public class SyncRepository
        {
            private readonly string _connectionString;

            private readonly Dictionary<string, SyncTableDefinition>
                _tableDefinitions;

            public SyncRepository(string connectionString)
            {
                _connectionString = connectionString;

                _tableDefinitions =
    new Dictionary<string, SyncTableDefinition>(
        StringComparer.OrdinalIgnoreCase)
    {
        {
            "ACCESS_RIGHTS",
            new SyncTableDefinition
            {
                TableName = "ACCESS_RIGHTS",
                PrimaryKeyColumn = "ACCESS_ID",
                IsIdentity = true
            }
        },
        {
            "ADMINSETTING",
            new SyncTableDefinition
            {
                TableName = "ADMINSETTING",
                PrimaryKeyColumn = "Sr_No",
                IsIdentity = true
            }
        },
        {
            "API_LIVE_DATA_LOG",
            new SyncTableDefinition
            {
                TableName = "API_LIVE_DATA_LOG",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "BOARDSETTING",
            new SyncTableDefinition
            {
                TableName = "BOARDSETTING",
                PrimaryKeyColumn = "BOARD_ID",
                IsIdentity = true
            }
        },
      
        {
            "COACHCONFIGURATION",
            new SyncTableDefinition
            {
                TableName = "COACHCONFIGURATION",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "DisplayEffect",
            new SyncTableDefinition
            {
                TableName = "DisplayEffect",
                PrimaryKeyColumn = "EFFECT_ID",
                IsIdentity = true
            }
        },
        {
            "FontSize",
            new SyncTableDefinition
            {
                TableName = "FontSize",
                PrimaryKeyColumn = "SIZE_ID",
                IsIdentity = false
            }
        },
        {
            "GENERALMESSAGE",
            new SyncTableDefinition
            {
                TableName = "GENERALMESSAGE",
                PrimaryKeyColumn = "MESSAGE_ID",
                IsIdentity = true
            }
        },
  
        {
            "HUBCONFIG",
            new SyncTableDefinition
            {
                TableName = "HUBCONFIG",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "IntensitySetting",
            new SyncTableDefinition
            {
                TableName = "IntensitySetting",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
       
        {
            "LEGENDLIST",
            new SyncTableDefinition
            {
                TableName = "LEGENDLIST",
                PrimaryKeyColumn = "SR_NO",
                IsIdentity = true
            }
        },
        {
            "LetterGap",
            new SyncTableDefinition
            {
                TableName = "LetterGap",
                PrimaryKeyColumn = "GAP_ID",
                IsIdentity = false
            }
        },
        {
            "LOGANNOUNCE",
            new SyncTableDefinition
            {
                TableName = "LOGANNOUNCE",
                PrimaryKeyColumn = "Id",
                IsIdentity = true
            }
        },
       
        {
            "LOGDISPLAYBOARD",
            new SyncTableDefinition
            {
                TableName = "LOGDISPLAYBOARD",
                PrimaryKeyColumn = "Id",
                IsIdentity = true
            }
        },
        {
            "LOGINUSER",
            new SyncTableDefinition
            {
                TableName = "LOGINUSER",
                PrimaryKeyColumn = "Loginid",
                IsIdentity = true
            }
        },
        {
            "LOGVDC",
            new SyncTableDefinition
            {
                TableName = "LOGVDC",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "MUSICLIST",
            new SyncTableDefinition
            {
                TableName = "MUSICLIST",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
      
        {
            "OVDLINECOLORS",
            new SyncTableDefinition
            {
                TableName = "OVDLINECOLORS",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },


        {
            "RECORDING",
            new SyncTableDefinition
            {
                TableName = "RECORDING",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },

        {
            "SCROLLINGSPEED",
            new SyncTableDefinition
            {
                TableName = "SCROLLINGSPEED",
                PrimaryKeyColumn = "ID",
                IsIdentity = false
            }
        },
        {
            "STATION",
            new SyncTableDefinition
            {
                TableName = "STATION", 
                PrimaryKeyColumn = "STATION_CODE",
                IsIdentity = false
            }
        },
        {
            "TRAIN_CATEGORY",
            new SyncTableDefinition
            {
                TableName = "TRAIN_CATEGORY",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "TRAINANNOUNCEPATTERN",
            new SyncTableDefinition
            {
                TableName = "TRAINANNOUNCEPATTERN",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "TRAINMASTER",
            new SyncTableDefinition
            {
                TableName = "TRAINMASTER",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "TRAINSTATUS",
            new SyncTableDefinition
            {
                TableName = "TRAINSTATUS",
                PrimaryKeyColumn = "Id",
                IsIdentity = true
            }
        },
        {
            "TrainType",
            new SyncTableDefinition
            {
                TableName = "TrainType",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "TRAINUPDATEDDATA",
            new SyncTableDefinition
            {
                TableName = "TRAINUPDATEDDATA",
                PrimaryKeyColumn = "TRAIN_NUMBER",
                IsIdentity = false
            }
        },
        {
            "TrueColor",
            new SyncTableDefinition
            {
                TableName = "TrueColor",
                PrimaryKeyColumn = "ID",
                IsIdentity = false
            }
        },
        {
            "TrueColorPlaylist",
            new SyncTableDefinition
            {
                TableName = "TrueColorPlaylist",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "UserLoginHistory",
            new SyncTableDefinition
            {
                TableName = "UserLoginHistory",
                PrimaryKeyColumn = "ID",
                IsIdentity = true
            }
        },
        {
            "Vendors",
            new SyncTableDefinition
            {
                TableName = "Vendors",
                PrimaryKeyColumn = "Id",
                IsIdentity = true
            }
        },
        {
            "VideoType",
            new SyncTableDefinition
            {
                TableName = "VideoType",
                PrimaryKeyColumn = "ID",
                IsIdentity = false
            }
        }
    };
            }


            // ============================================================
            // CONFIGURATION
            // ============================================================

            public string GetLocalNodeId()
            {
                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string sql = @"
                    SELECT LocalNodeId
                    FROM dbo.SyncConfiguration
                    WHERE ConfigId = 1";

                    object value;

                    using (SqlCommand command =
                           new SqlCommand(sql, connection))
                    {
                        value = command.ExecuteScalar();
                    }

                    if (value == null || value == DBNull.Value)
                        throw new Exception(
                            "SyncConfiguration is not configured.");

                    return Convert.ToString(value);
                }
            }


            // ============================================================
            // SYNC STATE
            // ============================================================

            public long GetLastChangeId(string peerNodeId)
            {
                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string sql = @"
                    SELECT LastChangeId
                    FROM dbo.SyncState
                    WHERE PeerNodeId = @PeerNodeId";

                    using (SqlCommand command =
                           new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@PeerNodeId",
                            peerNodeId);

                        object value = command.ExecuteScalar();

                        if (value == null || value == DBNull.Value)
                            return 0;

                        return Convert.ToInt64(value);
                    }
                }
            }


            public void SaveLastChangeId(
                string peerNodeId,
                long changeId)
            {
                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string sql = @"
                    UPDATE dbo.SyncState
                    SET
                        LastChangeId = @LastChangeId,
                        UpdatedAt = SYSUTCDATETIME()
                    WHERE PeerNodeId = @PeerNodeId";

                    using (SqlCommand command =
                           new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@LastChangeId",
                            changeId);

                        command.Parameters.AddWithValue(
                            "@PeerNodeId",
                            peerNodeId);

                        int rows = command.ExecuteNonQuery();

                        if (rows == 0)
                        {
                            const string insertSql = @"
                            INSERT INTO dbo.SyncState
                            (
                                PeerNodeId,
                                LastChangeId
                            )
                            VALUES
                            (
                                @PeerNodeId,
                                @LastChangeId
                            )";

                            using (SqlCommand insertCommand =
                                   new SqlCommand(
                                       insertSql,
                                       connection))
                            {
                                insertCommand.Parameters.AddWithValue(
                                    "@PeerNodeId",
                                    peerNodeId);

                                insertCommand.Parameters.AddWithValue(
                                    "@LastChangeId",
                                    changeId);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            public List<SyncChange> GetChanges(
                string localNodeId,
                long lastChangeId,
                int maxRows)
            {
                List<SyncChange> changes =
                    new List<SyncChange>();

                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string sql = @"
                    SELECT TOP (@MaxRows)
                        ChangeId,
                        TableName,
                        PrimaryKeyValue,
                        Operation,
                        ChangedAt,
                        ChangedBy
                    FROM dbo.SyncChange
                    WHERE
                        ChangeId > @LastChangeId
                        AND ChangedBy = @LocalNodeId
                    ORDER BY ChangeId";

                    using (SqlCommand command =
                           new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@MaxRows",
                            maxRows);

                        command.Parameters.AddWithValue(
                            "@LastChangeId",
                            lastChangeId);

                        command.Parameters.AddWithValue(
                            "@LocalNodeId",
                            localNodeId);

                        using (SqlDataReader reader =
                               command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SyncChange change =
                                    new SyncChange
                                    {
                                        ChangeId =
                                            reader.GetInt64(0),

                                        TableName =
                                            reader.GetString(1),

                                        PrimaryKeyValue =
                                            reader.GetString(2),

                                        Operation =
                                            reader.GetString(3),

                                        ChangedAt =
                                            reader.GetDateTime(4),

                                        ChangedBy =
                                            reader.GetString(5)
                                    };

                                changes.Add(change);
                            }
                        }
                    }
                }

                return changes;
            }

            public void LoadChangeData(SyncChange change)
            {
                if (change.Operation == "D")
                    return;

                SyncTableDefinition definition =
                    GetDefinition(change.TableName);

                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sql = $@"
                    SELECT *
                    FROM {QuoteIdentifier(definition.TableName)}
                    WHERE {QuoteIdentifier(definition.PrimaryKeyColumn)}
                          = @PrimaryKeyValue";

                    using (SqlCommand command =
                           new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@PrimaryKeyValue",
                            change.PrimaryKeyValue);

                        using (SqlDataReader reader =
                               command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                // Row may have been deleted after
                                // the journal entry was created.
                                change.Data = null;
                                return;
                            }

                            Dictionary<string, object> data =
                                new Dictionary<string, object>(
                                    StringComparer.OrdinalIgnoreCase);

                            for (int i = 0;
                                 i < reader.FieldCount;
                                 i++)
                            {
                                object value = reader.GetValue(i);

                                if (value == DBNull.Value)
                                    value = null;

                                data[reader.GetName(i)] = value;
                            }

                            change.Data = data;
                        }
                    }
                }
            }

            public SyncChange GetLatestLocalVersion(
                string tableName,
                string primaryKeyValue)
            {
                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string sql = @"
                    SELECT TOP 1
                        ChangeId,
                        TableName,
                        PrimaryKeyValue,
                        Operation,
                        ChangedAt,
                        ChangedBy
                    FROM dbo.SyncChange
                    WHERE
                        TableName = @TableName
                        AND PrimaryKeyValue = @PrimaryKeyValue
                    ORDER BY
                        ChangedAt DESC,
                        ChangeId DESC";

                    using (SqlCommand command =
                           new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@TableName",
                            tableName);

                        command.Parameters.AddWithValue(
                            "@PrimaryKeyValue",
                            primaryKeyValue);

                        using (SqlDataReader reader =
                               command.ExecuteReader())
                        {
                            if (!reader.Read())
                                return null;

                            return new SyncChange
                            {
                                ChangeId =
                                    reader.GetInt64(0),

                                TableName =
                                    reader.GetString(1),

                                PrimaryKeyValue =
                                    reader.GetString(2),

                                Operation =
                                    reader.GetString(3),

                                ChangedAt =
                                    reader.GetDateTime(4),

                                ChangedBy =
                                    reader.GetString(5)
                            };
                        }
                    }
                }
            }

            public bool ApplyRemoteChange(
                SyncChange remoteChange)
            {
                SyncTableDefinition definition =
                    GetDefinition(remoteChange.TableName);

                SyncChange localVersion =
                    GetLatestLocalVersion(
                        remoteChange.TableName,
                        remoteChange.PrimaryKeyValue);

                // --------------------------------------------------------
                // Latest wins
                // --------------------------------------------------------

                if (localVersion != null)
                {
                    int comparison =
                        CompareVersions(
                            remoteChange,
                            localVersion);

                    if (comparison <= 0)
                    {
                        return false;
                    }
                }

                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction =
                           connection.BeginTransaction())
                    {
                        try
                        {
                            SetSyncApplyContext(
                                connection,
                                transaction);

                            if (remoteChange.Operation == "D")
                            {
                                ApplyDelete(
                                    connection,
                                    transaction,
                                    definition,
                                    remoteChange);
                            }
                            else
                            {
                                ApplyInsertOrUpdate(
                                    connection,
                                    transaction,
                                    definition,
                                    remoteChange);
                            }

                            // Store remote version locally.
                            // ChangedBy remains the ORIGINAL node,
                            // therefore it will NOT be sent back.
                            InsertRemoteMetadata(
                                connection,
                                transaction,
                                remoteChange);

                            transaction.Commit();

                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }


            // ============================================================
            // DELETE
            // ============================================================

            private void ApplyDelete(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncTableDefinition definition,
                SyncChange change)
            {
                string sql = $@"
                DELETE FROM
                    {QuoteIdentifier(definition.TableName)}
                WHERE
                    {QuoteIdentifier(definition.PrimaryKeyColumn)}
                    = @PrimaryKeyValue";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@PrimaryKeyValue",
                        change.PrimaryKeyValue);

                    command.ExecuteNonQuery();
                }

                UpsertTombstone(
                    connection,
                    transaction,
                    change);
            }


            // ============================================================
            // INSERT / UPDATE
            // ============================================================

            private void ApplyInsertOrUpdate(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncTableDefinition definition,
                SyncChange change)
            {
                if (change.Data == null ||
                    change.Data.Count == 0)
                {
                    return;
                }

                bool exists =
                    RowExists(
                        connection,
                        transaction,
                        definition,
                        change.PrimaryKeyValue);

                if (exists)
                {
                    UpdateRow(
                        connection,
                        transaction,
                        definition,
                        change);
                }
                else
                {
                    InsertRow(
                        connection,
                        transaction,
                        definition,
                        change);
                }

                RemoveTombstone(
                    connection,
                    transaction,
                    change.TableName,
                    change.PrimaryKeyValue);
            }


            // ============================================================
            // CHECK ROW
            // ============================================================

            private bool RowExists(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncTableDefinition definition,
                string primaryKeyValue)
            {
                string sql = $@"
                SELECT COUNT(1)
                FROM {QuoteIdentifier(definition.TableName)}
                WHERE {QuoteIdentifier(definition.PrimaryKeyColumn)}
                      = @PrimaryKeyValue";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@PrimaryKeyValue",
                        primaryKeyValue);

                    return Convert.ToInt32(
                               command.ExecuteScalar()) > 0;
                }
            }


            // ============================================================
            // UPDATE ROW
            // ============================================================

            private void UpdateRow(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncTableDefinition definition,
                SyncChange change)
            {
                List<string> assignments =
                    new List<string>();

                using (SqlCommand command =
                       new SqlCommand("",
                           connection,
                           transaction))
                {
                    int index = 0;

                    foreach (KeyValuePair<string, object> item
                             in change.Data)
                    {
                        if (item.Key.Equals(
                                definition.PrimaryKeyColumn,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if (IsIdentityColumn(
                                connection,
                                transaction,
                                definition.TableName,
                                item.Key))
                        {
                            continue;
                        }

                        string parameterName =
                            "@p" + index++;

                        assignments.Add(
                            $"{QuoteIdentifier(item.Key)} = {parameterName}");

                        command.Parameters.AddWithValue(
                            parameterName,
                            item.Value ?? DBNull.Value);
                    }

                    if (assignments.Count == 0)
                        return;

                    command.Parameters.AddWithValue(
                        "@PrimaryKeyValue",
                        change.PrimaryKeyValue);

                    command.CommandText = $@"
                    UPDATE {QuoteIdentifier(
                            definition.TableName)}
                    SET
                        {string.Join(",", assignments)}
                    WHERE
                        {QuoteIdentifier(
                                definition.PrimaryKeyColumn)}
                        = @PrimaryKeyValue";

                    command.ExecuteNonQuery();
                }
            }


            // ============================================================
            // INSERT ROW
            // ============================================================

            private void InsertRow(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncTableDefinition definition,
                SyncChange change)
            {
                List<string> columns =
                    new List<string>();

                List<string> parameters =
                    new List<string>();

                using (SqlCommand command =
                       new SqlCommand("",
                           connection,
                           transaction))
                {
                    int index = 0;

                    foreach (KeyValuePair<string, object> item
                             in change.Data)
                    {
                        string parameterName =
                            "@p" + index++;

                        columns.Add(
                            QuoteIdentifier(item.Key));

                        parameters.Add(parameterName);

                        command.Parameters.AddWithValue(
                            parameterName,
                            item.Value ?? DBNull.Value);
                    }

                    string tableName =
                        QuoteIdentifier(definition.TableName);

                    bool hasIdentity =
                        HasIdentityColumn(
                            connection,
                            transaction,
                            definition.TableName);

                    if (hasIdentity)
                    {
                        command.CommandText = $@"
                        SET IDENTITY_INSERT {tableName} ON;

                        INSERT INTO {tableName}
                        (
                            {string.Join(",", columns)}
                        )
                        VALUES
                        (
                            {string.Join(",", parameters)}
                        );

                        SET IDENTITY_INSERT {tableName} OFF;";
                    }
                    else
                    {
                        command.CommandText = $@"
                        INSERT INTO {tableName}
                        (
                            {string.Join(",", columns)}
                        )
                        VALUES
                        (
                            {string.Join(",", parameters)}
                        );";
                    }

                    command.ExecuteNonQuery();
                }
            }


            // ============================================================
            // REMOTE METADATA
            // ============================================================

            private void InsertRemoteMetadata(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncChange change)
            {
                const string sql = @"
                INSERT INTO dbo.SyncChange
                (
                    TableName,
                    PrimaryKeyValue,
                    Operation,
                    ChangedAt,
                    ChangedBy
                )
                VALUES
                (
                    @TableName,
                    @PrimaryKeyValue,
                    @Operation,
                    @ChangedAt,
                    @ChangedBy
                )";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@TableName",
                        change.TableName);

                    command.Parameters.AddWithValue(
                        "@PrimaryKeyValue",
                        change.PrimaryKeyValue);

                    command.Parameters.AddWithValue(
                        "@Operation",
                        change.Operation);

                    command.Parameters.AddWithValue(
                        "@ChangedAt",
                        change.ChangedAt);

                    // IMPORTANT:
                    // This is the ORIGINAL source node.
                    command.Parameters.AddWithValue(
                        "@ChangedBy",
                        change.ChangedBy);

                    command.ExecuteNonQuery();
                }
            }


            // ============================================================
            // TOMBSTONE
            // ============================================================

            private void UpsertTombstone(
                SqlConnection connection,
                SqlTransaction transaction,
                SyncChange change)
            {
                const string sql = @"
                IF EXISTS
                (
                    SELECT 1
                    FROM dbo.SyncTombstone
                    WHERE
                        TableName = @TableName
                        AND PrimaryKeyValue = @PrimaryKeyValue
                )
                BEGIN
                    UPDATE dbo.SyncTombstone
                    SET
                        ChangedAt = @ChangedAt,
                        ChangedBy = @ChangedBy,
                        ChangeId = @ChangeId
                    WHERE
                        TableName = @TableName
                        AND PrimaryKeyValue = @PrimaryKeyValue;
                END
                ELSE
                BEGIN
                    INSERT INTO dbo.SyncTombstone
                    (
                        TableName,
                        PrimaryKeyValue,
                        ChangedAt,
                        ChangedBy,
                        ChangeId
                    )
                    VALUES
                    (
                        @TableName,
                        @PrimaryKeyValue,
                        @ChangedAt,
                        @ChangedBy,
                        @ChangeId
                    );
                END";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@TableName",
                        change.TableName);

                    command.Parameters.AddWithValue(
                        "@PrimaryKeyValue",
                        change.PrimaryKeyValue);

                    command.Parameters.AddWithValue(
                        "@ChangedAt",
                        change.ChangedAt);

                    command.Parameters.AddWithValue(
                        "@ChangedBy",
                        change.ChangedBy);

                    command.Parameters.AddWithValue(
                        "@ChangeId",
                        change.ChangeId);

                    command.ExecuteNonQuery();
                }
            }


            private void RemoveTombstone(
                SqlConnection connection,
                SqlTransaction transaction,
                string tableName,
                string primaryKeyValue)
            {
                const string sql = @"
                DELETE FROM dbo.SyncTombstone
                WHERE
                    TableName = @TableName
                    AND PrimaryKeyValue = @PrimaryKeyValue";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@TableName",
                        tableName);

                    command.Parameters.AddWithValue(
                        "@PrimaryKeyValue",
                        primaryKeyValue);

                    command.ExecuteNonQuery();
                }
            }


            // ============================================================
            // SESSION CONTEXT
            // ============================================================

            private void SetSyncApplyContext(
                SqlConnection connection,
                SqlTransaction transaction)
            {
                const string sql = @"
                EXEC sys.sp_set_session_context
                    @key = N'SyncApply',
                    @value = 1;";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.ExecuteNonQuery();
                }
            }


            // ============================================================
            // VERSION COMPARISON
            // ============================================================

            private int CompareVersions(
                SyncChange a,
                SyncChange b)
            {
                int result =
                    DateTime.Compare(
                        a.ChangedAt,
                        b.ChangedAt);

                if (result != 0)
                    return result;

                // Deterministic tie breaker.
                return string.Compare(
                    a.ChangedBy,
                    b.ChangedBy,
                    StringComparison.OrdinalIgnoreCase);
            }


            // ============================================================
            // TABLE DEFINITION
            // ============================================================

            private SyncTableDefinition GetDefinition(
                string tableName)
            {
                SyncTableDefinition definition;

                if (!_tableDefinitions.TryGetValue(
                        tableName,
                        out definition))
                {
                    throw new Exception(
                        "Table is not configured for synchronization: "
                        + tableName);
                }

                return definition;
            }


            // ============================================================
            // IDENTITY
            // ============================================================

            private bool HasIdentityColumn(
                SqlConnection connection,
                SqlTransaction transaction,
                string tableName)
            {
                const string sql = @"
                SELECT COUNT(1)
                FROM sys.columns c
                INNER JOIN sys.tables t
                    ON c.object_id = t.object_id
                WHERE
                    t.name = @TableName
                    AND c.is_identity = 1";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@TableName",
                        tableName);

                    return Convert.ToInt32(
                               command.ExecuteScalar()) > 0;
                }
            }


            private bool IsIdentityColumn(
                SqlConnection connection,
                SqlTransaction transaction,
                string tableName,
                string columnName)
            {
                const string sql = @"
                SELECT COUNT(1)
                FROM sys.columns c
                INNER JOIN sys.tables t
                    ON c.object_id = t.object_id
                WHERE
                    t.name = @TableName
                    AND c.name = @ColumnName
                    AND c.is_identity = 1";

                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.AddWithValue(
                        "@TableName",
                        tableName);

                    command.Parameters.AddWithValue(
                        "@ColumnName",
                        columnName);

                    return Convert.ToInt32(
                               command.ExecuteScalar()) > 0;
                }
            }


            // ============================================================
            // SQL IDENTIFIER
            // ============================================================

            private string QuoteIdentifier(
                string identifier)
            {
                if (string.IsNullOrWhiteSpace(identifier))
                    throw new ArgumentException(
                        "Invalid SQL identifier.");

                return "[" +
                       identifier.Replace("]", "]]") +
                       "]";
            }
        }
    }
}
