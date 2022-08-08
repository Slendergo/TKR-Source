using common;
using common.database;
using common.isc;
using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
    public sealed class ItemDustPools
    {
        public List<WeightedCollection<Item>> Pools { get; private set; } = new List<WeightedCollection<Item>>();
        
        public void AddPool(List<KeyValuePair<Item, int>> items)
        {
            var weightedCollection = new WeightedCollection<Item>();
            foreach(var item in items)
                weightedCollection.AddItem(item.Key, item.Value);
            Pools.Add(weightedCollection);
        }

        public Item GetRandom(Random random)
        {
            var pool = Pools[random.Next(Pools.Count)];
            return pool.GetRandom(random);
        }
    }

    public sealed class ItemDustWeights
    {
        public ItemDustPools ItemDusts { get; private set; } = new ItemDustPools();
        public ItemDustPools MagicDust { get; private set; } = new ItemDustPools();
        public ItemDustPools SpecialDust { get; private set; } = new ItemDustPools();
        public ItemDustPools MiscDust { get; private set; } = new ItemDustPools();
        public ItemDustPools PotionDust { get; private set; } = new ItemDustPools();

        private readonly GameServer GameServer;

        public ItemDustWeights(GameServer gameServer)
        {
            GameServer = gameServer;
        }

        public void Initialize()
        {
            var xmlData = GameServer.Resources.GameData;
            foreach (var items in xmlData.ItemDusts.ItemPools)
                ItemDusts.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.MagicPools)
                MagicDust.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.SpecialPools)
                SpecialDust.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.MiscPools)
                MiscDust.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.PotionPools)
                PotionDust.AddPool(GetItems(items, xmlData));

            var random = new Random();
            var Items = new Dictionary<string, int>();
            for(var i = 0; i < 100000; i++)
            {
                var item = MagicDust.GetRandom(random);
                if (!Items.ContainsKey(item.ObjectId))
                    Items[item.ObjectId] = 0;
                Items[item.ObjectId]++;
            }

            Items = Items.OrderByDescending(_ => _.Value).ToDictionary(_ => _.Key, _ => _.Value);
            foreach (var item in Items)
                Console.Write(item.Key + " " + item.Value + $" 1/{(100000.0f / item.Value)}" + " | ");
        }

        private List<KeyValuePair<Item, int>> GetItems(ItemPool items, XmlData xmlData)
        {
            var poolItems = new List<KeyValuePair<Item, int>>();
            foreach (var tieredItem in items.TieredItems)
            {
                var slotTypes = TierLoot.GetSlotTypes(tieredItem.ItemType);

                var tieredItems = xmlData.Items
                    .Where(item => Array.IndexOf(slotTypes, item.Value.SlotType) != -1)
                    .Where(item => item.Value.Tier == tieredItem.Tier)
                    .Select(item => item.Value);

                foreach (var item in tieredItems)
                    poolItems.Add(new KeyValuePair<Item, int>(item, tieredItem.Weight));
            }

            foreach (var namedItem in items.NamedItems)
            {
                var foundItem = xmlData.Items.Values.FirstOrDefault(item => item.ObjectId == namedItem.ItemName);
                if (foundItem == null)
                    throw new Exception("Invalid Name of item");
                poolItems.Add(new KeyValuePair<Item, int>(foundItem, namedItem.Weight));
            }
            return poolItems;
        }
    }

    public sealed class GameServer
    {
        public string InstanceId { get; private set; }
        public ServerConfig Configuration { get; private set; }
        public Resources Resources { get; private set; }
        public ItemDustWeights ItemDustWeights { get; private set; }
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

        public DateTime RestartCloseTime { get; private set; }

        public GameServer(string[] args)
        {
            if (args.Length > 1)
                throw new Exception("Too many arguments expected 1.");

            Configuration = ServerConfig.ReadFile(args.Length == 1 ? args[0] : "wServer.json");
            Resources = new Resources(Configuration.serverSettings.resourceFolder, true, null, ExportXMLS);
            ItemDustWeights = new ItemDustWeights(this);
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

        private static bool ExportXMLS = true;

        public void Run()
        {
            if (ExportXMLS)
            {
                if (!Directory.Exists("GenerateXMLS"))
                    _ = Directory.CreateDirectory("GenerateXMLS");

                var f = File.CreateText("GenerateXMLS/EmbeddedData_ObjectsCXML.xml");
                f.Write(Resources.GameData.ObjectCombinedXML.ToString());
                f.Close();

                var f3 = File.CreateText("GenerateXMLS/EmbeddedData_SkinsCXML.xml");
                f3.Write(Resources.GameData.SkinsCombinedXML.ToString());
                f3.Close();

                var f4 = File.CreateText("GenerateXMLS/EmbeddedData_PlayersCXML.xml");
                f4.Write(Resources.GameData.CombinedXMLPlayers.ToString());
                f4.Close();

                var f2 = File.CreateText("GenerateXMLS/EmbeddedData_GroundsCXML.xml");
                f2.Write(Resources.GameData.GroundCombinedXML.ToString());
                f2.Close();
            }

            InstanceId = Configuration.serverInfo.instanceId = Guid.NewGuid().ToString();
            Console.WriteLine($"[Set] InstanceId [{InstanceId}]");

            Console.WriteLine("[Initialize] ItemDustWeights");
            ItemDustWeights.Initialize();

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
            RestartCloseTime = utcNow.Add(timeout);
            var restartsIn = utcNow.Add(TimeSpan.FromMinutes(5));

            var restart = false;

            var watch = Stopwatch.StartNew();
            while (Running)
            {
                // server close event
                if(!restart && DateTime.UtcNow >= RestartCloseTime)
                {
                    // announce to the server of the restart
                    // restarting crashes for some reason :(
                    // todo future me will fix

                    foreach(var world in WorldManager.GetWorlds())
                        ChatManager.ServerAnnounce("Server **Restart** in 5 minutes, prepare to leave");

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
