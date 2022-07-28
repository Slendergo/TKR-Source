using common;
using common.isc;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using wServer.core.commands;
using wServer.core.objects.vendors;
using wServer.logic;
using wServer.logic.loot;
using common.database;
using wServer.utils;

namespace wServer.core
{
    public sealed class CoreServerManager
    {
        public CoreServerManager(string fileName)
        {
            ServerConfig = ServerConfig.ReadFile(fileName);
            Resources = new Resources(ServerConfig.serverSettings.resourceFolder, true);
            Database = new Database(Resources, ServerConfig);
            ShutdownManualResetEvent = new ManualResetEvent(false);
            MarketSweeper = new MarketSweeper(Database);
            MarketSweeper.Start();
            SLogger.Instance.Info("Start MarketSweeper");
        }

        public static bool Initialized { get; private set; }
        public static string InstanceId { get; set; }

        public BehaviorDb BehaviorDb { get; private set; }
        public ChatManager ChatManager { get; private set; }
        public CommandManager CommandManager { get; private set; }
        public ConnectionManager ConnectionManager { get; private set; }
        public CoreServerManagerTicker CoreServerManagerTicker { get; private set; }
        public Thread CoreServerManagerTickerThread { get; private set; }
        public Database Database { get; private set; }
        public DbEvents DbEvents { get; private set; }
        public ISManager InterServerManager { get; private set; }
        public MarketSweeper MarketSweeper { get; private set; }
        public Resources Resources { get; private set; }
        public ServerConfig ServerConfig { get; private set; }
        public ManualResetEvent ShutdownManualResetEvent { get; set; }
        public WorldManager WorldManager { get; private set; }

        public double GetEnemyDamageRate()
        {
            var settings = ServerConfig.serverSettings;
            var utc = DateTime.UtcNow;

            if (utc >= settings.GetEnemyDamageStartAt && utc <= settings.GetEnemyDamageEndAt)
                return settings.enemyDamageRate;

            return 1.0;
        }

        public string[] GetEventMessages()
        {
            var settings = ServerConfig.serverSettings;
            var utc = DateTime.UtcNow;
            var events = new List<string>();

            if (utc >= settings.GetEnemyHealthBoostStartAt && utc <= settings.GetEnemyHealthBoostEndAt)
                events.Add($"- Enemy Health Boost [rate : {settings.enemyDamageRate}x]: " +
                    $"{GetTimeRemains(settings.GetEnemyHealthBoostEndAt)}"
                    );

            if (utc >= settings.GetEnemyDamageStartAt && utc <= settings.GetEnemyDamageEndAt)
                events.Add($"- Enemy Damage [rate : {settings.enemyDamageRate}x]: " +
                    $"{GetTimeRemains(settings.GetEnemyDamageEndAt)}"
                    );

            if (utc >= settings.GetExpEventStartAt && utc <= settings.GetExpEventEndAt)
                events.Add(
                    $"- EXP Event [rate: {settings.expEventRate}x]: " +
                    $"{GetTimeRemains(settings.GetExpEventEndAt)}"
                );
            if (utc >= settings.GetLootEventStartAt && utc <= settings.GetLootEventEndAt)
                events.Add(
                    $"- Loot Event [rate: {settings.lootEventRate}x]: " +
                    $"{GetTimeRemains(settings.GetLootEventEndAt)}"
                );

            return events.ToArray();
        }

        public double GetExperienceRate()
        {
            var settings = ServerConfig.serverSettings;
            var utc = DateTime.UtcNow;

            if (utc >= settings.GetExpEventStartAt && utc <= settings.GetExpEventEndAt)
                return settings.expEventRate;

            return 1.0;
        }

        public double GetHealthBoostRate()
        {
            var settings = ServerConfig.serverSettings;
            var utc = DateTime.UtcNow;

            if (utc >= settings.GetEnemyHealthBoostStartAt && utc <= settings.GetEnemyHealthBoostEndAt)
                return settings.enemyHealthBoostRate;

            return 1.0;
        }

        public double GetLootRate()
        {
            var settings = ServerConfig.serverSettings;
            var utc = DateTime.UtcNow;

            if (utc >= settings.GetLootEventStartAt && utc <= settings.GetLootEventEndAt)
                return settings.lootEventRate;

            return 1.0;
        }

        public bool HasEvents()
        {
            var settings = ServerConfig.serverSettings;
            var utc = DateTime.UtcNow;
            return (utc >= settings.GetLootEventStartAt && utc <= settings.GetLootEventEndAt) || (utc >= settings.GetExpEventStartAt && utc <= settings.GetExpEventEndAt);
        }

        public void Initialize()
        {
            InstanceId = ServerConfig.serverInfo.instanceId = Guid.NewGuid().ToString();
            SLogger.Instance.Info("Initialize CoreServerManagerTicker");

            ConnectionManager = new ConnectionManager(this);
            ConnectionManager.Initialize();
            SLogger.Instance.Info("Initialize ConnectionManager");

            MerchantLists.Initialize(this);
            SLogger.Instance.Info("Initialize MerchantLists");

            DbEvents = new DbEvents(this);
            SLogger.Instance.Info("DbEvents BehaviorDb");

            Loot.ConfigureDropRates();
            SLogger.Instance.Info("Loot DropRates");

            BehaviorDb = new BehaviorDb(this);
            SLogger.Instance.Info("Created BehaviorDb");
            CommandManager = new CommandManager(this);
            SLogger.Instance.Info("Created CommandManager");

            WorldManager = new WorldManager(this);
            WorldManager.Initialize();
            SLogger.Instance.Info("Initialize WorldManager");

            InterServerManager = new ISManager(Database, ServerConfig);
            InterServerManager.Initialize();
            SLogger.Instance.Info("Initialize InterServerManager");

            ChatManager = new ChatManager(this);
            ChatManager.Initialize();
            SLogger.Instance.Info("Initialize ChatManager");

            CoreServerManagerTicker = new CoreServerManagerTicker(this, 5);
            SLogger.Instance.Info("Created CoreServerManagerTicker");
            CoreServerManagerTickerThread = new Thread(CoreServerManagerTicker.Update);
            CoreServerManagerTickerThread.Start();
            SLogger.Instance.Info("Start CoreServerManagerTickerThread");

            ConnectionManager.StartListening();
            SLogger.Instance.Info("StartListening ConnectionManager");

            Initialized = true;
        }

        public bool IsWhitelisted(int accountId) => ServerConfig.serverSettings.whitelist.Contains(accountId);

        public void Set() => ShutdownManualResetEvent.Set();

        public void Shutdown()
        {
            SLogger.Instance.Info("Shutdown CoreServerManager");
            CoreServerManagerTicker?.Shutdown();
            SLogger.Instance.Info("Shutdown CoreServerManagerTicker");
            CoreServerManagerTickerThread?.Join();
            SLogger.Instance.Info("Join CoreServerManagerTickerThread");
            InterServerManager?.Shutdown();
            SLogger.Instance.Info("Shutdown InterServerManager");
            ChatManager?.Shutdown();
            SLogger.Instance.Info("Shutdown ChatManager");
            ConnectionManager?.Shutdown();
            SLogger.Instance.Info("Shutdown ConnectionManager");
            WorldManager?.Shutdown();
            SLogger.Instance.Info("Shutdown WorldManager");
            Database?.Dispose();
            SLogger.Instance.Info("Shutdown Database");
            Resources?.Dispose();
            SLogger.Instance.Info("Shutdown Resources");
        }

        public void WaitOne() => ShutdownManualResetEvent.WaitOne();

        private string GetTimeRemains(DateTime end)
        {
            var timeLeft = end.Subtract(DateTime.UtcNow);

            return string.Format(
                "ends at {0} (on {5}) UTC (countdown: {1}d {2}h {3}m {4}s).",
                end.ToString("dd MMM yyyy"),
                timeLeft.Days.ToString("D2"),
                timeLeft.Hours.ToString("D2"),
                timeLeft.Minutes.ToString("D2"),
                timeLeft.Seconds.ToString("D2"),
                end.ToString("dddd")
            );
        }
    }
}
