using System;
using System.Collections.Generic;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Restart : Command
        {
            public Restart() : base("restart", permLevel: 110, alias: "res")
            {
            }

            protected override bool Process(Player player, TickData time, string color)
            {
                Program.SetupRestarter(TimeSpan.FromMinutes(5.05), new KeyValuePair<TimeSpan, Action>[]
                {
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(5), () => Program.RestartAnnouncement(5)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(4), () => Program.RestartAnnouncement(4)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(3), () => Program.RestartAnnouncement(3)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(2), () => Program.RestartAnnouncement(2)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromMinutes(1), () => Program.RestartAnnouncement(1)),
                    new KeyValuePair<TimeSpan, Action>(TimeSpan.FromSeconds(30), () => Program.RestartAnnouncement(-1))
                });
                return true;
            }
        }
    }
}
