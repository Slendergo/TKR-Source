using Anna;
using Anna.Request;
using CA.Threading.Tasks;
using common;
using common.database;
using common.isc;
using common.resources;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using static server.ContextRequest;

namespace server
{
    public class App
    {
        public static ServerConfig Config;
        public static RedisDb Db;
        public static ISManager ISManager;
        public static Resources Resources;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly ManualResetEvent Shutdown = new ManualResetEvent(false);

        private static ConcurrentQueue<RequestContext> EarlyRequests { get; set; }

        private static bool IsReadyToAccept { get; set; }

        public static void OnError(Exception e, RequestContext rContext)
        {
            Log.Error($"{e.Message}\r\n{e.StackTrace}");

            try { rContext?.Respond("<Error>Internal server error</Error>", 500); }
            catch { }
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs args) => Log.Fatal((Exception)args.ExceptionObject);

        private static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.SetMinThreads(250, completionPortThreads);

            Config = ServerConfig.ReadFile(args.Length > 0 ? args[0] : "server.json");

            LogManager.Configuration.Variables["logDirectory"] = Config.serverSettings.logFolder + "/server";
            LogManager.Configuration.Variables["buildConfig"] = GenericExtensions.GetBuildConfiguration();

            EarlyRequests = new ConcurrentQueue<RequestContext>();

            using (Resources = new Resources(Config.serverSettings.resourceFolder, false))
            {
                Db = new RedisDb(Config.dbInfo, Resources);

                await Db.StartAsync();

                RequestHandlers.Initialize(Resources);

                Config.serverInfo.instanceId = Guid.NewGuid().ToString();

                ISManager = new ISManager(Db, Config, true);
                ISManager.Initialize();

                Console.CancelKeyPress += delegate { Shutdown.Set(); };

                var port = Config.serverInfo.port;
                var address = Config.serverInfo.bindAddress;
                var url = $"http://{address}:{port}/";
                var source = new CancellationTokenSource();

                using (var server = new HttpServer(url))
                {
                    foreach (var uri in RequestHandlers.Get.Keys.ToList()) server.GET(uri).Subscribe(Response);
                    foreach (var uri in RequestHandlers.Post.Keys.ToList()) server.POST(uri).Subscribe(Response);

                    Log.Info("Listening at address {0}:{1}...", address, port);

                    var routine = new InternalRoutine(200, UpdateServersUsage);
                    routine.AttachToParent(source.Token);
                    routine.Start();

                    IsReadyToAccept = true;

                    ProcessEarlyConnections();

                    Shutdown.WaitOne();
                }

                Log.Info("Terminating...");

                source.Cancel();
                ISManager.Dispose();
                Db.Stop();
            }
        }

        private static void ProcessEarlyConnections()
        {
            while (EarlyRequests.Count > 0)
            {
                if (!EarlyRequests.TryDequeue(out var requestContext))
                    break;

                Response(requestContext);
            }
        }

        private static void Response(RequestContext rContext)
        {
            try
            {
                if (!IsReadyToAccept)
                {
                    EarlyRequests.Enqueue(rContext);
                    return;
                }

                if (rContext.Request.HttpMethod.Equals("GET"))
                {
                    var request = rContext.Request.Url.LocalPath;

                    if (!RequestHandlers.Get.ContainsKey(request))
                        return;

                    var query = HttpUtility.ParseQueryString(rContext.Request.Url.Query);

                    RequestHandlers.Get[rContext.Request.Url.LocalPath].HandleRequest(rContext, query);
                    return;
                }

                var cr = new ContextRequest();
                var acr = new AsyncContextRequest(cr.HandleContext);

                acr.BeginInvoke(rContext, 4096, new AsyncCallback(cr.HandleContextCallback), null);
            }
            catch (Exception e) { OnError(e, rContext); }
        }

        private static void UpdateServersUsage()
        {
            var servers = ISManager.GetServerList().Where(server => server.instanceId != Config.serverInfo.instanceId).ToArray();

            if (servers.Length == 0)
            {
                Config.serverInfo.players = 0;
                Config.serverInfo.maxPlayers = 0;
                return;
            }

            var players = 0;
            var maxPlayers = 0;

            for (var i = 0; i < servers.Length; i++)
            {
                var server = servers[i];

                players += server.players;
                maxPlayers += server.maxPlayers;
            }

            Config.serverInfo.players = players;
            Config.serverInfo.maxPlayers = maxPlayers;
        }
    }
}
