using DataBaseHandler;
using DataModel;
using Helper;
using IPISv2.Sync;
using LogManager;

namespace Global_Data
{
    public static class GlobalData
    {
        private static bool _isInitialized = false;
        private static SyncService _syncService;
        private static SyncHttpServer _syncHttpServer;
        public static AdminSetting _lstAdminSetting = new AdminSetting();
        public static List<LoginInfo> _lstLoginInfo = new List<LoginInfo>();
        public static LoginInfo CurrentUser => _lstLoginInfo?.FirstOrDefault();
        public static string CurrentUserId => CurrentUser?.Loginid.ToString() ?? "0";
        public static string CurrentUserName => CurrentUser?.user_Name ?? string.Empty;
        public static AccessRight CurrentUserAccessRight = new AccessRight();
        public static AccessRight accessType;

        public static void SetConfig(int UserId)
        {
            _lstAdminSetting = _dbService.GET_ADMIN_SETTING();
            _lstLoginInfo = _dbService.GET_LOGIN_DETAILS(UserId);
            var user = _lstLoginInfo.FirstOrDefault();
            if (user != null)
            {
                var accessRights = _dbService.GET_ACCESS_RIGHTS(user.ACCESS_ID);
                CurrentUserAccessRight = accessRights.FirstOrDefault() ?? new AccessRight();
            }
        }
        public static async Task SetConfigAsync(int UserId)
        {
            if (!_isInitialized)  
            {
                LoggerService.InitializeLogger();
                _isInitialized = true;
                StartSyncServiceDelayed();
            }

            var loginTask = Task.Run(() => _dbService.GET_LOGIN_DETAILS(UserId));
            var adminTask = Task.Run(() => _dbService.GET_ADMIN_SETTING());

            await Task.WhenAll(loginTask, adminTask);
            _lstLoginInfo = loginTask.Result;
            _lstAdminSetting = adminTask.Result;

            var user = _lstLoginInfo.FirstOrDefault();
            if (user != null)
            {
                var accessRights = await Task.Run(() => _dbService.GET_ACCESS_RIGHTS(user.ACCESS_ID));
                CurrentUserAccessRight = accessRights.FirstOrDefault() ?? new AccessRight();
            }
        }
        private static void StartSyncServiceDelayed()
        {
            _ = Task.Run(async () =>
            {
                try
                {

                    await Task.Delay(10000);

                    string connectionString = Config._dbSelfConnection;

                    string localNodeId;
                    string peerNodeId;
                    string peerIp;

                    // Example:
                    //
                    // MASTER:
                    // localNodeId = "MASTER";
                    // peerNodeId  = "SLAVE";
                    // peerIp      = "10.0.0.254";
                    //
                    // SLAVE:
                    // localNodeId = "SLAVE";
                    // peerNodeId  = "MASTER";
                    // peerIp      = "10.0.0.253";

                   
                    bool isMaster = _lstAdminSetting != null && _lstAdminSetting.Ismaster == 1;

                    if (isMaster)
                    {
                        localNodeId = "MASTER";
                        peerNodeId = "SLAVE";
                        peerIp = "10.0.0.254";
                    }
                    else
                    {
                        localNodeId = "SLAVE";
                        peerNodeId = "MASTER";
                        peerIp = "10.0.0.253";
                    }

                    _syncService =
                        new SyncService(
                            connectionString,
                            peerNodeId,
                            $"http://{peerIp}:21000");


                    _syncHttpServer =
                        new SyncHttpServer(
                            _syncService,
                            "http://+:21000/");


                    // Start receiver first.
                    _syncHttpServer.Start();

                    // Start sender.
                    _syncService.Start();


                }
                catch (Exception ex)
                {
                    LoggerService.Logger.Error(ex,"SyncService start failed");
                }
            });
        }


        public static async Task StopSyncAsync()
        {
            // Only the first caller gets the objects; later or parallel calls get null and do nothing.
            var syncService = System.Threading.Interlocked.Exchange(ref _syncService, null);
            var syncHttpServer = System.Threading.Interlocked.Exchange(ref _syncHttpServer, null);

            try
            {
                if (syncService != null)
                {
                    await syncService.StopAsync().ConfigureAwait(false);
                    syncService.Dispose();
                }

                if (syncHttpServer != null)
                {
                    await syncHttpServer.StopAsync().ConfigureAwait(false);
                    syncHttpServer.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "SyncService stop failed");
            }
        }
    }
}
