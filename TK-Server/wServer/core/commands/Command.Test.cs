using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Test : Command
        {
            public Test() : base("testThings", permLevel: 110)
            { }
            //Template for add your test/temporal commands
            protected override bool Process(Player player, TickData time, string args)
            {
                return true;
            }
        }
    }
}
