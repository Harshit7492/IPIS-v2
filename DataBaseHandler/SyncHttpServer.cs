using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPISv2.Sync
{
    public class SyncHttpServer : IDisposable
    {
        private readonly SyncService _syncService;

        private readonly HttpListener _listener;

        private CancellationTokenSource _cts;

        private Task _serverTask;

        private bool _disposed;


        public SyncHttpServer(
            SyncService syncService,
            string listenUrl)
        {
            if (syncService == null)
                throw new ArgumentNullException(
                    nameof(syncService));

            if (string.IsNullOrWhiteSpace(
                    listenUrl))
                throw new ArgumentException(
                    "Listen URL is required.");

            _syncService =
                syncService;

            _listener =
                new HttpListener();

            if (!listenUrl.EndsWith("/"))
                listenUrl += "/";

            _listener.Prefixes.Add(
                listenUrl);
        }


        // ============================================================
        // START
        // ============================================================

        public void Start()
        {
            if (_serverTask != null)
                return;

            _cts =
                new CancellationTokenSource();

            _listener.Start();

            _serverTask =
                Task.Run(
                    () => ListenLoopAsync(
                        _cts.Token));
        }


        // ============================================================
        // STOP
        // ============================================================

        public async Task StopAsync()
        {
            if (_cts == null)
                return;

            _cts.Cancel();

            try
            {
                _listener.Stop();
            }
            catch
            {
            }

            try
            {
                if (_serverTask != null)
                    await _serverTask;
            }
            catch
            {
            }

            _serverTask = null;

            _cts.Dispose();
            _cts = null;
        }


        // ============================================================
        // LISTEN
        // ============================================================

        private async Task ListenLoopAsync(
            CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                HttpListenerContext context = null;

                try
                {
                    context =
                        await _listener
                            .GetContextAsync();

                    _ = Task.Run(
                        () => ProcessRequestAsync(
                            context,
                            token),
                        token);
                }
                catch (
                    HttpListenerException)
                {
                    if (token.IsCancellationRequested)
                        break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "[SYNC HTTP] " +
                        ex);
                }
            }
        }


        // ============================================================
        // REQUEST
        // ============================================================

        private async Task ProcessRequestAsync(
            HttpListenerContext context,
            CancellationToken token)
        {
            try
            {
                string path =
                    context.Request.Url.AbsolutePath
                        .TrimEnd('/');

                if (context.Request.HttpMethod
                        .Equals(
                            "POST",
                            StringComparison.OrdinalIgnoreCase)
                    &&
                    path.Equals(
                        "/sync/apply",
                        StringComparison.OrdinalIgnoreCase))
                {
                    await HandleApplyAsync(
                        context,
                        token);

                    return;
                }

                if (context.Request.HttpMethod
                        .Equals(
                            "GET",
                            StringComparison.OrdinalIgnoreCase)
                    &&
                    path.Equals(
                        "/sync/health",
                        StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonAsync(
                        context,
                        200,
                        new
                        {
                            Success = true,
                            Message = "Sync server is running."
                        });

                    return;
                }

                await WriteJsonAsync(
                    context,
                    404,
                    new
                    {
                        Success = false,
                        Message = "Endpoint not found."
                    });
            }
            catch (Exception ex)
            {
                try
                {
                    await WriteJsonAsync(
                        context,
                        500,
                        new
                        {
                            Success = false,
                            Message = ex.Message
                        });
                }
                catch
                {
                }
            }
        }


        // ============================================================
        // APPLY
        // ============================================================

        private async Task HandleApplyAsync(
            HttpListenerContext context,
            CancellationToken token)
        {
            string json;

            using (StreamReader reader =
                   new StreamReader(
                       context.Request.InputStream,
                       context.Request.ContentEncoding))
            {
                json =
                    await reader.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                await WriteJsonAsync(
                    context,
                    400,
                    new SyncResponse
                    {
                        Success = false,
                        Message = "Request body is empty."
                    });

                return;
            }

            SyncBatch batch;

            try
            {
                batch =
                    JsonConvert.DeserializeObject
                        <SyncBatch>(
                            json);
            }
            catch (Exception ex)
            {
                await WriteJsonAsync(
                    context,
                    400,
                    new SyncResponse
                    {
                        Success = false,
                        Message =
                            "Invalid JSON: " +
                            ex.Message
                    });

                return;
            }

            SyncResponse result =
                _syncService.ApplyRemoteBatch(
                    batch);

            await WriteJsonAsync(
                context,
                result.Success ? 200 : 500,
                result);
        }


        // ============================================================
        // WRITE JSON
        // ============================================================

        private async Task WriteJsonAsync(
            HttpListenerContext context,
            int statusCode,
            object data)
        {
            string json =
                JsonConvert.SerializeObject(
                    data,
                    Formatting.None);

            byte[] buffer =
                Encoding.UTF8.GetBytes(json);

            context.Response.StatusCode =
                statusCode;

            context.Response.ContentType =
                "application/json";

            context.Response.ContentEncoding =
                Encoding.UTF8;

            context.Response.ContentLength64 =
                buffer.Length;

            try
            {
                using (Stream output =
                       context.Response.OutputStream)
                {
                    await output.WriteAsync(
                        buffer,
                        0,
                        buffer.Length);
                }
            }
            catch
            {
            }
            finally
            {
                try
                {
                    context.Response.Close();
                }
                catch
                {
                }
            }
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

            try
            {
                _listener.Close();
            }
            catch
            {
            }
        }
    }
}