using CA.Threading.Tasks;
using common;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using wServer.core;
using wServer.utils;

namespace wServer
{
    internal class Program
    {
        public static bool EnableGuildLootBoost = false;
        private static readonly object RestartLock = new object();
        private static bool Disposed = false;

        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static CoreServerManager CoreServerManager { get; set; }
        public static DateTime EndWhen { get; private set; }
        public static AutomatedRestarter Restarter { get; private set; }
        public static DateTime StartedWhen { get; private set; }
        private static HandlerRoutine ConsoleCtrlCheckRoutine { get; set; }

        public static void Dispose()
        {
            lock (RestartLock)
            {
                if (Disposed)
                    return;

                Disposed = true;

                Restarter?.Stop(false);
                CoreServerManager?.Shutdown();
                Thread.Sleep(5000);
                CoreServerManager?.Set();
            }
        }

        public static void RestartAnnouncement(int minutes)
        {
            var message = minutes != -1
                ? $"The server will be restarted within {minutes} minute{(minutes > 1 ? "s" : "")}, be ready to disconnect."
                : "The server will be restarted soon, be ready to disconnect.";

            /* The following code performs an asynchronous operations in parallel
             * for all exist worlds and players, but with a low demand of processing
             * cost for the server.
             * */
            var worlds = CoreServerManager.WorldManager.GetWorlds();
            worlds.AsParallel().Select(w =>
            {
                var players = w.GetPlayers();
                players.AsParallel().Select(p =>
                {
                    p.SendInfo(message);
                    return p;
                }).ToArray();
                return w;
            }).ToArray();
        }

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        public static void SetupRestarter(TimeSpan timeout, KeyValuePair<TimeSpan, Action>[] listeners)
        {
            if (Restarter != null)
                Restarter = null;

            Restarter = new AutomatedRestarter(timeout);
            Restarter.AddEventListeners(listeners);
            Restarter.OnFinished += delegate
            {
                CoreServerManager.Shutdown();
                Thread.Sleep(5000);
                Process.Start(AppDomain.CurrentDomain.FriendlyName);
                CoreServerManager.Set();
            };
            Restarter.Start();

            var utcNow = DateTime.UtcNow;
            StartedWhen = utcNow;
            EndWhen = utcNow.Add(timeout);
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:
                    Dispose();
                    break;
            }
            return true;
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            SLogger.Instance.Fatal(((Exception)args.ExceptionObject).StackTrace.ToString());
            Restarter.Stop(true);
        }

        private static void Main(string[] args)
        {
            ConsoleCtrlCheckRoutine = ConsoleCtrlCheck;
            SetConsoleCtrlHandler(ConsoleCtrlCheckRoutine, true);

            CoreServerManager = new CoreServerManager(args.Length > 0 ? args[0] : "wServer.json");
            SetupLogData();
            CoreServerManager.Initialize();

            SetupThreadData();
            SetupRestarter(TimeSpan.FromHours(CoreServerManager.ServerConfig.serverSettings.restartTime), new KeyValuePair<TimeSpan, Action>[]
            {
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(15), () => RestartAnnouncement(15)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(10), () => RestartAnnouncement(10)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(5), () => RestartAnnouncement(5)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(4), () => RestartAnnouncement(4)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(3), () => RestartAnnouncement(3)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(2), () => RestartAnnouncement(2)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(1), () => RestartAnnouncement(1)),
                new KeyValuePair<TimeSpan, Action>(TimeSpan.FromSeconds(30), () => RestartAnnouncement(-1))
            });

            CoreServerManager.WaitOne();
        }

        private static void SetupLogData()
        {
            LogManager.Configuration.Variables["logDirectory"] = $"{CoreServerManager.ServerConfig.serverSettings.logFolder}/wServer";
            LogManager.Configuration.Variables["buildConfig"] = Utils.GetBuildConfiguration();
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
        }

        private static void SetupThreadData()
        {
            ThreadPool.GetMinThreads(out _, out var completionPortThreads);
            ThreadPool.SetMinThreads(250, completionPortThreads);
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";
        }
    }
}
