using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using tk.assetformatter.handler;

namespace tk.assetformatter
{
    public sealed class AssetFormatter
    {
        private static IConfigurationRoot _config;
        private static Dictionary<string, IHandler> _handlers;
        private static ManualResetEvent _mre;

        public static Dictionary<string, XElement> Assets { get; private set; }

        private static string _name => typeof(AssetFormatter).Name;

        public static void ExportAsset(string type, IEnumerable<XElement> nodes, string file, out int amount)
        {
            amount = nodes.Count();

            var assets = new XElement(type, nodes);
            var path = Path.Combine(_config["output-path"], $"TK_{file}.xml");

            File.WriteAllText(path, assets.ToString());
        }

        public static void ExportAssets(string directory, string type, IDictionary<string, IList<XElement>> assets, ref int amount)
        {
            var enumAssets = assets.ToDictionary(entry => entry.Key, entry => entry.Value.AsEnumerable());

            ExportAssets(directory, type, enumAssets, ref amount);
        }

        public static void ExportAssets(string directory, string type, IDictionary<string, IEnumerable<XElement>> assets, ref int amount)
        {
            var path = Path.Combine(_config["output-path"], directory.ToLower());

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var asset in assets)
            {
                var nodes = asset.Value;
                var xml = new XElement(type, nodes);
                var newPath = Path.Combine(path, $"TK_{directory}_{asset.Key}.xml");

                amount += nodes.Count();

                File.WriteAllText(newPath, xml.ToString());
            }
        }

        public static async Task InitHandlersAsync()
        {
            Console.Clear();

            DisplayTitle();

            Log.Breakline();
            Log.Breakline();

            var outputPath = _config["output-path"];

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentNullException("output-path", "Empty declaration for output path.");

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            await _handlers["load"].ExecuteAsync(_config["input-path"]);
            await _handlers["display-types"].ExecuteAsync();

            Log.Breakline();
            Log.Warn($"Press ANY key to continue...");
            Log.Breakline();

            Console.ReadKey(true);

            await _handlers["unknown"].ExecuteAsync();
            await _handlers["classes"].ExecuteAsync();
            await _handlers["projectiles"].ExecuteAsync();
            await _handlers["skins"].ExecuteAsync();
            await _handlers["grounds"].ExecuteAsync();
            await _handlers["portals"].ExecuteAsync();
            await _handlers["characters"].ExecuteAsync();
            await _handlers["gameobjects"].ExecuteAsync();
            await _handlers["containers"].ExecuteAsync();
            await _handlers["miscellaneous"].ExecuteAsync();
            await _handlers["npcs"].ExecuteAsync();
            await _handlers["dyes"].ExecuteAsync();
            await _handlers["walls"].ExecuteAsync();
            await _handlers["equipments"].ExecuteAsync();

            Log.Breakline();
            Log.Warn("Success!");
            Log.Breakline();
            Log.Warn($"Press ANY key to close application...");
            Log.Breakline();

            Console.ReadKey(true);

            _mre.Set();
        }

        private static async Task BeginHandlersAsync()
        {
            var baseDir = AppContext.BaseDirectory;
            var settings = Path.Combine(baseDir, "settings.yml");

            if (!File.Exists(settings))
                throw new FileNotFoundException("No settings file detected!", "settings.yml");

            Assets = new Dictionary<string, XElement>();

            Log.Info($"{_name}: Loading handlers...");

            _handlers = new Dictionary<string, IHandler>
            {
                ["load"] = new LoadHandler(),
                ["display-types"] = new DisplayClassesHandler(),
                ["unknown"] = new UnknownHandler(),
                ["classes"] = new ClassesHandler(),
                ["projectiles"] = new ProjectilesHandler(),
                ["skins"] = new SkinsHandler(),
                ["grounds"] = new GroundsHandler(),
                ["portals"] = new PortalsHandler(),
                ["characters"] = new CharactersHandler(),
                ["gameobjects"] = new GameObjectsHandler(),
                ["containers"] = new ContainersHandler(),
                ["miscellaneous"] = new MiscellaneousHandler(),
                ["npcs"] = new NPCsHandler(),
                ["dyes"] = new DyesHandler(),
                ["walls"] = new WallsHandler(),
                ["equipments"] = new EquipmentsHandler()
            };

            Log.Info($"{_name}: Loaded {_handlers.Count} handler{(_handlers.Count > 1 ? "s" : "")}");
            Log.Info($"{_name}: Ready");
            Log.Breakline();
            Log.Warn($"Press ANY key to continue...");

            Console.ReadKey(true);

            await InitHandlersAsync();
        }

        private static void DisplayTitle()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
@"
     _                 _   _____                          _   _
    / \   ___ ___  ___| |_|  ___|__  _ __ _ __ ___   __ _| |_| |_ ___ _ __
   / _ \ / __/ __|/ _ \ __| |_ / _ \| '__| '_ ` _ \ / _` | __| __/ _ \ '__|
  / ___ \\__ \__ \  __/ |_|  _| (_) | |  | | | | | | (_| | |_| ||  __/ |
 /_/   \_\___/___/\___|\__|_|  \___/|_|  |_| |_| |_|\__,_|\__|\__\___|_|
");

            Console.ResetColor();
        }

        private static async Task Main()
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddYamlFile("settings.yml");

            _config = builder.Build();
            _mre = new ManualResetEvent(false);

            Console.CancelKeyPress += (s, e) => _mre.Set();

            Log.Warn($"{_name}: Initializing...");

            await BeginHandlersAsync();

            _mre.WaitOne();

            Log.Breakline();
            Log.Warn($"{_name}: Terminating...");
            Thread.Sleep(500);
            Environment.Exit(0);
        }
    }
}
