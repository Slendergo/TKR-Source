using CA.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using wServer.core;

namespace wServer.utils
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
}
