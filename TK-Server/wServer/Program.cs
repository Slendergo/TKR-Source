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
    public sealed class Restarter
    {
        private readonly AutomatedRestarter AutomatedRestarter;
        private readonly DateTime StartedAt;
        private readonly DateTime EndsAt;
        private readonly CoreServerManager CoreServerManager;

        public Restarter(CoreServerManager manager, int hoursUntilRestart)
        {
            CoreServerManager = manager;

            var timeout = TimeSpan.FromHours(hoursUntilRestart);

            var utcNow = DateTime.UtcNow;
            StartedAt = utcNow;
            EndsAt = utcNow.Add(timeout);

            AutomatedRestarter = new AutomatedRestarter(timeout);
            AutomatedRestarter.AddEventListeners(
                new KeyValuePair<TimeSpan, Action>[]
                {
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(15), () => RestartAnnouncement(15)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(10), () => RestartAnnouncement(10)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(5), () => RestartAnnouncement(5)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(4), () => RestartAnnouncement(4)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(3), () => RestartAnnouncement(3)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(2), () => RestartAnnouncement(2)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(1), () => RestartAnnouncement(1)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(30), () => RestartAnnouncement(-1))
                });

            AutomatedRestarter.OnFinished += delegate
            {
                manager.Shutdown();
                Thread.Sleep(5000);
                _ = Process.Start(AppDomain.CurrentDomain.FriendlyName);
                manager.Set();
            };
        }

        public DateTime GetRestartTime() => EndsAt;

        public void Start()
        {
            AutomatedRestarter.Start();
        }

        public void RestartAnnouncement(int minutes)
        {
            var message = minutes != -1 ? $"The server will be restarted within {minutes} minute{(minutes > 1 ? "s" : "")}, be ready to disconnect." : "The server will be restarted soon, be ready to disconnect.";

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

        public void Stop(bool isFinished) => AutomatedRestarter.Stop(isFinished);
    }

    public sealed class Program
    {
        private static readonly object RestartLock = new object();
        private static bool Disposed = false;

        public static Restarter Restarter { get; set; }
        public static CoreServerManager CoreServerManager { get; set; }
        private static HandlerRoutine ConsoleCtrlCheckRoutine { get; set; }

        private static void Main(string[] args)
        {
            ConsoleCtrlCheckRoutine = ConsoleCtrlCheck;
            SetConsoleCtrlHandler(ConsoleCtrlCheckRoutine, true);

            CoreServerManager = new CoreServerManager(args.Length > 0 ? args[0] : "wServer.json");
            SetupLogData();
            CoreServerManager.Initialize();
            SetupThreadData();
            Restarter = new Restarter(CoreServerManager, CoreServerManager.ServerConfig.serverSettings.restartTime);
            Restarter.Start();
            CoreServerManager.WaitOne();
        }

        public static void Dispose()
        {
            lock (RestartLock)
            {
                if (Disposed)
                    return;
                Disposed = true;

                Restarter?.Stop(false);
                CoreServerManager?.Shutdown();
                CoreServerManager?.Set();
            }
        }

        private static void SetupLogData()
        {
            LogManager.Configuration.Variables["logDirectory"] = $"{CoreServerManager.ServerConfig.serverSettings.logFolder}/wServer";
            LogManager.Configuration.Variables["buildConfig"] = Utils.GetBuildConfiguration();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                SLogger.Instance.Fatal(((Exception)args.ExceptionObject).StackTrace.ToString());
                Restarter?.Stop(true);
            };
        }

        private static void SetupThreadData()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";
        }

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }


        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

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

    }
}
