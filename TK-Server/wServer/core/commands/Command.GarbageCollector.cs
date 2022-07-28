using common;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class GarbageCollector : Command
        {
            public GarbageCollector() : base("gc", permLevel: 110)
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                player.SendInfo("GC.Collect invoked.");

                using (var profiler = new TimedProfiler("GC.Collect", (message) => player.SendHelp(message)))
                    GC.Collect();

                player.SendInfo("GC.Collect ran with success!");
                return true;
            }
        }
    }
}
