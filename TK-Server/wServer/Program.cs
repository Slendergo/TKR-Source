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
    public sealed class Program
    {
        private static readonly object RestartLock = new object();
        private static bool Disposed = false;

        public static Restarter Restarter { get; set; }
        public static CoreServerManager CoreServerManager { get; set; }
        private static HandlerRoutine ConsoleCtrlCheckRoutine { get; set; }

        private static void Main(string[] args)
        {
//#if LINUX
//            // todo linux support
//            var unixExitSignal = new UnixExitSignal();
//            unixExitSignal.Exit += OnExit;
//#else
            ConsoleCtrlCheckRoutine = ConsoleCtrlCheck;
            SetConsoleCtrlHandler(ConsoleCtrlCheckRoutine, true);
//#endif

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

//#if LINUX
//        private void OnExit()
//        {
//            OnDispose();
//        }
//#else
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
//#endif
    }
}
