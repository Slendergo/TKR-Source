using common;
using common.database;
using common.isc;
using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using wServer.core.commands;
using wServer.core.objects.vendors;
using wServer.logic;
using wServer.logic.loot;
using wServer.networking.connection;
using wServer.utils;

namespace wServer.core
{
    public sealed class GameServer
    {
        public string InstanceId { get; private set; }
        public ServerConfig Configuration { get; private set; }
        public Resources Resources { get; private set; }
        public Database Database { get; private set; }
        public MarketSweeper MarketSweeper { get; private set; }
        public ConnectionManager ConnectionManager { get; private set; }
        public ConnectionListener ConnectionListener { get; private set; }
        public ChatManager ChatManager { get; private set; }
        public BehaviorDb BehaviorDb { get; private set; }
        public CommandManager CommandManager { get; private set; }
        public DbEvents DbEvents { get; private set; }
        public ISManager InterServerManager { get; private set; }
        public WorldManager WorldManager { get; private set; }
        public SignalListener SignalListener { get; private set; }
        private bool Running { get; set; } = true;

        public GameServer(string[] args)
        {
            if (args.Length > 1)
                throw new Exception("Too many arguments expected 1.");

            Configuration = ServerConfig.ReadFile(args.Length == 1 ? args[0] : "wServer.json");
            Resources = new Resources(Configuration.serverSettings.resourceFolder, true);
            Database = new Database(Resources, Configuration);
            MarketSweeper = new MarketSweeper(Database);
            ConnectionManager = new ConnectionManager(this);
            ConnectionListener = new ConnectionListener(this);
            ChatManager = new ChatManager(this);
            BehaviorDb = new BehaviorDb(this);
            CommandManager = new CommandManager();
            DbEvents = new DbEvents(this);
            InterServerManager = new ISManager(Database, Configuration);
            WorldManager = new WorldManager(this);
            SignalListener = new SignalListener(this);
        }

        public bool IsWhitelisted(int accountId) => Configuration.serverSettings.whitelist.Contains(accountId);

        public void Run()
        {
            InstanceId = Configuration.serverInfo.instanceId = Guid.NewGuid().ToString();
            Console.WriteLine($"[Set] InstanceId [{InstanceId}]");

            Console.WriteLine("[Initialize] CommandManager");
            CommandManager.Initialize(this);

            Console.WriteLine("[Configure] Loot");
            Loot.ConfigureDropRates();

            Console.WriteLine("[Initialize] MobDrops");
            MobDrops.Initialize(this);

            Console.WriteLine("[Initialize] BehaviorDb");
            BehaviorDb.Initialize();

            Console.WriteLine("[Initialize] MerchantLists");
            MerchantLists.Initialize(this);

            Console.WriteLine("[Initialize] WorldManager");
            WorldManager.Initialize();

            Console.WriteLine("[Initialize] InterServerManager");
            InterServerManager.Initialize();
            
            Console.WriteLine("[Initialize] ChatManager");
            ChatManager.Initialize();
            
            Console.WriteLine("[Initialize] ConnectionListener");
            ConnectionListener.Initialize();

            Console.WriteLine("[Start] MarketSweeper");
            MarketSweeper.Start();

            Console.WriteLine("[Start] ConnectionListener");
            ConnectionListener.Start();

            Console.WriteLine("[Initialize Success]");
            
            Console.WriteLine("[Network] Internal Joined");
            InterServerManager.JoinNetwork();

            LogManager.Configuration.Variables["logDirectory"] = $"{Configuration.serverSettings.logFolder}/wServer";
            LogManager.Configuration.Variables["buildConfig"] = Utils.GetBuildConfiguration();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                SLogger.Instance.Fatal(((Exception)args.ExceptionObject).StackTrace.ToString());
                // todo auto restart
            };

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            var timeout = TimeSpan.FromHours(Configuration.serverSettings.restartTime);

            var utcNow = DateTime.UtcNow;
            var startedAt = utcNow;
            var closeTime = utcNow.Add(timeout);
            var restartsIn = utcNow.Add(TimeSpan.FromMinutes(5));

            var restart = false;

            var watch = Stopwatch.StartNew();
            while (Running)
            {
                // server close event
                if(!restart && DateTime.UtcNow >= closeTime)
                {
                    // announce to the server of the restart

                    Console.WriteLine("[Restart] Procdure Commensing");
                    ConnectionListener.Disable();
                    restart = true;
                }

                if(restart && DateTime.UtcNow >= restartsIn)
                    break;

                var current = watch.ElapsedMilliseconds;

                ConnectionManager.Tick(current);

                var logicTime = (int)(watch.ElapsedMilliseconds - current);
                var sleepTime = Math.Max(0, 200 - logicTime);

                Thread.Sleep(sleepTime);
            }

            if (restart)
                Console.WriteLine("[Restart] Triggered");
            else
                Console.WriteLine("[Shutdown] Triggered");
            
            Dispose();

            if(restart)
                _ = Process.Start(AppDomain.CurrentDomain.FriendlyName);

            Console.WriteLine("[Program] Terminated");
            Thread.Sleep(10000);
        }

        public void Stop()
        {
            if (!Running)
                return;
            Running = false;
        }

        public void Dispose()
        {
            Console.WriteLine("[Dispose] InterServerManager");
            InterServerManager.Shutdown();

            Console.WriteLine("[Dispose] Resources");
            Resources.Dispose();
            
            Console.WriteLine("[Dispose] Database");
            Database.Dispose();
            
            Console.WriteLine("[Dispose] MarketSweeper");
            MarketSweeper.Stop();
            
            Console.WriteLine("[Dispose] ConnectionManager");
            ConnectionManager.Dispose();
            
            Console.WriteLine("[Dispose] ConnectionListener");
            ConnectionListener.Shutdown();
            
            Console.WriteLine("[Dispose] ChatManager");
            ChatManager.Dispose();
            
            Console.WriteLine("[Dispose] Configuration");
            WorldManager.Dispose();
            
            Console.WriteLine("[Dispose] Configuration");
            Configuration = null;
        }
    }
}
