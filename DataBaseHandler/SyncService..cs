using DataBaseHandler.IPISv2.Sync;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPISv2.Sync
{
    public class SyncService : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _peerNodeId;
        private readonly string _peerUrl;

        private readonly SyncRepository _repository;

        private readonly HttpClient _httpClient;

        private CancellationTokenSource _cts;
        private Task _workerTask;

        private readonly int _intervalMilliseconds =
            5000;

        private readonly int _batchSize =
            100;

        private bool _disposed;


        public SyncService(
            string connectionString,
            string peerNodeId,
            string peerUrl)
        {
            _connectionString = connectionString;
            _peerNodeId = peerNodeId;
            _peerUrl = peerUrl.TrimEnd('/');

            _repository =
                new SyncRepository(
                    _connectionString);

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }


        // ============================================================
        // START
        // ============================================================

        public void Start()
        {
            if (_workerTask != null)
                return;

            _cts =
                new CancellationTokenSource();

            _workerTask =
                Task.Run(
                    () => WorkerLoopAsync(
                        _cts.Token));
        }


        // ============================================================
        // STOP
        // ============================================================

        public async Task StopAsync()
        {
            // Capture current instances first.
            var cts = _cts;
            var workerTask = _workerTask;

            // Already stopped / never started.
            if (cts == null)
                return;

            // Immediately mark service as stopped.
            _cts = null;
            _workerTask = null;

            try
            {
                cts.Cancel();

                if (workerTask != null)
                {
                    await workerTask;
                }
            }
            catch (OperationCanceledException)
            {
                // Normal during shutdown.
            }
            finally
            {
                cts.Dispose();
            }
        }

        // ============================================================
        // WORKER
        // ============================================================

        private async Task WorkerLoopAsync(
            CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await SynchronizeAsync(
                        token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // Replace this with your existing logger.
                    Console.WriteLine(
                        "[SYNC] " +
                        ex);
                }

                try
                {
                    await Task.Delay(
                        _intervalMilliseconds,
                        token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }


        // ============================================================
        // SYNC
        // ============================================================

        private async Task SynchronizeAsync(
            CancellationToken token)
        {
            string localNodeId =
                _repository.GetLocalNodeId();

            // --------------------------------------------------------
            // SEND LOCAL CHANGES
            // --------------------------------------------------------

            long lastChangeId =
                _repository.GetLastChangeId(
                    _peerNodeId);

            List<SyncChange> changes =
                _repository.GetChanges(
                    localNodeId,
                    lastChangeId,
                    _batchSize);

            if (changes.Count > 0)
            {
                foreach (SyncChange change in changes)
                {
                    _repository.LoadChangeData(
                        change);
                }

                SyncBatch batch =
                    new SyncBatch
                    {
                        SourceNodeId =
                            localNodeId,

                        Changes =
                            changes
                    };

                SyncResponse response = await SendChangesAsync(batch, token);

                if (response != null &&
                    response.Success)
                {
                    long newLastChangeId =
                        changes[changes.Count - 1]
                            .ChangeId;

                    _repository.SaveLastChangeId(
                        _peerNodeId,
                        newLastChangeId);
                }
            }
        }


        // ============================================================
        // SEND HTTP
        // ============================================================

        private async Task<SyncResponse> SendChangesAsync(
     SyncBatch batch,
     CancellationToken token)
        {
            string json = JsonConvert.SerializeObject(
                batch,
                Formatting.None);

            using StringContent content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            string url = _peerUrl + "/sync/apply";

            Console.WriteLine(
                $"[SYNC] POST START: {url} - {DateTime.Now:HH:mm:ss.fff}");

            try
            {
                HttpResponseMessage response =
                    await _httpClient.PostAsync(
                        url,
                        content,
                        token);

                Console.WriteLine(
                    $"[SYNC] POST END: {response.StatusCode} - {DateTime.Now:HH:mm:ss.fff}");

                if (!response.IsSuccessStatusCode)
                    return null;

                string responseJson =
                    await response.Content.ReadAsStringAsync(token);

                if (string.IsNullOrWhiteSpace(responseJson))
                    return null;

                return JsonConvert.DeserializeObject<SyncResponse>(
                    responseJson);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(
                    $"[SYNC] POST CANCELLED/TIMEOUT: {ex.Message}");

                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(
                    $"[SYNC] HTTP ERROR: {ex.Message}");

                return null;
            }
        }


        // ============================================================
        // RECEIVE REMOTE BATCH
        // Called by SyncHttpServer
        // ============================================================

        public SyncResponse ApplyRemoteBatch(SyncBatch batch)
        {
            if (batch == null)
            {
                return new SyncResponse
                {
                    Success = false,
                    Message = "Request is null."
                };
            }

            if (string.IsNullOrWhiteSpace(
                    batch.SourceNodeId))
            {
                return new SyncResponse
                {
                    Success = false,
                    Message = "SourceNodeId is missing."
                };
            }

            string localNodeId = _repository.GetLocalNodeId();

            // Never accept our own changes.
            if (string.Equals(
                    batch.SourceNodeId,
                    localNodeId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return new SyncResponse
                {
                    Success = false,
                    Message =
                        "SourceNodeId is same as local node."
                };
            }

            int received = 0;
            int applied = 0;
            int ignored = 0;

            long lastChangeId = 0;

            if (batch.Changes != null)
            {
                foreach (SyncChange change
                         in batch.Changes)
                {
                    received++;

                    try
                    {
                        bool result =
                            _repository.ApplyRemoteChange(
                                change);

                        if (result)
                            applied++;
                        else
                            ignored++;

                        if (change.ChangeId >
                            lastChangeId)
                        {
                            lastChangeId =
                                change.ChangeId;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            "Failed to apply change. " +
                            "Table=" +
                            change.TableName +
                            ", PK=" +
                            change.PrimaryKeyValue +
                            ", Operation=" +
                            change.Operation,
                            ex);
                    }
                }
            }

            return new SyncResponse
            {
                Success = true,

                Message = "Sync completed.",

                NodeId = localNodeId,

                Received = received,

                Applied = applied,

                Ignored = ignored,

                LastReceivedChangeId =
                    lastChangeId
            };
        }


        // ============================================================
        // DISPOSE
        // ============================================================

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                StopAsync()
                    .GetAwaiter()
                    .GetResult();
            }
            catch
            {
            }

            _httpClient.Dispose();
        }
    }
}