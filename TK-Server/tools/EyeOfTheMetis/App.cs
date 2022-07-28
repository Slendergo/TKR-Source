using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EyeOfTheMetis
{
    public class App
    {
        private static readonly Dictionary<string, (string alias, string description, Action<string> action)> CommandList
            = new Dictionary<string, (string alias, string description, Action<string> action)>()
            {
                { "--help", ("h", "Show a list of all commands.", (input) => HandleHelp()) },
                { "--show-item-metas", ("sim", "Show a list of all most valueable items in TK server.", (input) => HandleShowItemMetas()) }
            };

        private static Dictionary<string, string> Commands = new Dictionary<string, string>();
        private static Dictionary<string, string> Aliases = new Dictionary<string, string>();
        private static MetisDb Metis;

        private static void Main()
        {
            var name = Assembly.GetExecutingAssembly().GetName().Name;
            var version =
                $"{Assembly.GetExecutingAssembly().GetName().Version}".Substring(0,
                $"{Assembly.GetExecutingAssembly().GetName().Version}".Length - 2);

            Console.Title = $"{name} v{version} - The prudence goddess is watching us.";

            var mre = new ManualResetEvent(false);

            Console.CancelKeyPress += delegate { mre.Set(); };

            Warn("Initializing...");

            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.SetMinThreads(250, completionPortThreads);

            Info("Loading MetisDb this process can take few minutes, please wait...");
            Breakline();

            using (var tp = new TimedProfiler("MetisDb", true, (message) => Warn(message)))
            {
                Metis = new MetisDb(Info);

                Breakline();

                // awaits until all assets are properly loaded
                MetisDb.Loading.WaitOne();

                Metis.SerializeBigData((message) => Info(message));
            }

            Task.Factory.StartNew(Core, TaskCreationOptions.AttachedToParent);

            mre.WaitOne();

            Warn("Terminating...");

            Thread.Sleep(500);
            Environment.Exit(0);
        }

        #region "Command Handlers"

        private static void HandleHelp()
        {
            Info("Eye of the Metis is a developer tool to analyse TK database structure pattern using a local Redis " +
                "instance server for read-only operations and then generates a detailed report that matchs with " +
                "input conditions specified on scope.");
            Breakline();

            var entryId = 1;

            foreach (var entry in CommandList)
                Info(string.Format(
                    "[{0}]. {1}{2}\n\t{3}\n",
                    entryId++,
                    entry.Key.Remove(0, 2),
                    $" (alias: {entry.Value.alias})" ?? "",
                    entry.Value.description
                ));

            Warn("Press ANY key to continue...");

            Console.ReadKey(true);

            CoreLoop();
        }

        private static void HandleShowItemMetas()
        {
            var valuableItems = Metis.AllMostValuableItems.OrderBy(item => item.ObjectId).ToArray();

            Info($"Displaying {valuableItems.Length} valueable item{(valuableItems.Length > 1 ? "s" : "")}:");
            Breakline();

            for (var i = 0; i < valuableItems.Length; i++)
            {
                var valuableItem = valuableItems[i];

                Info(
                    $"- [Tier: {Metis.TagToTierName(valuableItem)} / " +
                    $"Type: {Utils.To4Hex(valuableItem.ObjectType)}] " +
                    $"{valuableItem.ObjectId}"
                );
            }

            Tail();
        }

        #endregion "Command Handlers"

        #region "Core"

        private static void Core()
        {
            Info("Loading commands and aliases...");

            foreach (var entry in CommandList)
            {
                var command = entry.Key;
                var alias = $"--{entry.Value.alias}";

                Commands.Add(command, alias);
                Aliases.Add(alias, command);
            }

            Info($"Loaded {CommandList.Count} command{(CommandList.Count > 1 ? "s" : "")}.");
            Info("Eye of the Metis is ready to use!");
            Breakline();
            Warn("Press ANY key to continue...");

            Console.ReadKey(true);

            CoreLoop();
        }

        private static void CoreLoop()
        {
            Console.Clear();

            DisplayTitle();
            Breakline();
            Breakline();
            Info("Type --help for more details.");
            Breakline();

            var input = Console.ReadLine();
            var args = input.Split(' ');

            if (args.Length == 0)
            {
                Breakline();

                Console.Clear();

                CoreLoop();
                return;
            }

            var command = args[0];
            var newInput = string.Join(" ", args.Skip(1));

            Console.Clear();

            DisplayTitle();
            Breakline();
            Breakline();

            if (Commands.ContainsKey(command)) CommandList[command].action(newInput);
            else if (Aliases.ContainsKey(command)) CommandList[Aliases[command]].action(newInput);
            else CoreLoop();
        }

        #endregion "Core"

        private static void DisplayTitle()
            => Warn(
@"
▓█████ ▒█████ ▄▄▄█████▓███▄ ▄███▓
▓█   ▀▒██▒  ██▓  ██▒ ▓▓██▒▀█▀ ██▒
▒███  ▒██░  ██▒ ▓██░ ▒▓██    ▓██░
▒▓█  ▄▒██   ██░ ▓██▓ ░▒██    ▒██
░▒████░ ████▓▒░ ▒██▒ ░▒██▒   ░██▒
░░ ▒░ ░ ▒░▒░▒░  ▒ ░░  ░ ▒░   ░  ░
 ░ ░  ░ ░ ▒ ▒░    ░   ░  ░      ░
   ░  ░ ░ ░ ▒   ░     ░      ░
   ░  ░   ░ ░                ░

"
            );

        #region "Log utilities"

        private static void Tail(bool goToMenu = true)
        {
            Breakline();

            if (goToMenu)
            {
                Warn("Press ANY key to continue...");

                Console.ReadKey(true);

                CoreLoop();
            }
        }

        private static void Breakline() => Console.WriteLine("\n");

        private static void Info(string message, bool isWriteLine = true) => Log(message, isWriteLine, ConsoleColor.Gray);

        private static void Warn(string message, bool isWriteLine = true) => Log(message, isWriteLine, ConsoleColor.DarkYellow);

        private static void Error(string message, bool isWriteLine = true) => Log(message, isWriteLine, ConsoleColor.DarkRed);

        private static void Log(string message, bool isWriteLine, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            if (isWriteLine) Console.WriteLine(message);
            else Console.Write(message);

            Console.ResetColor();
        }

        #endregion "Log utilities"
    }
}