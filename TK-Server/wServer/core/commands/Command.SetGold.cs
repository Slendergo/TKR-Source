using common;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetGold : Command
        {
            public SetGold() : base("setgold", permLevel: 90, alias: "gold")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var amount = args.ToInt32();

                if (string.IsNullOrEmpty(args))
                {
                    player.SendInfo("/setgold <amount>");
                    return false;
                }
                player.Credits = player.Client.Account.Credits += amount;
                player.ForceUpdate(player.Credits);
                return true;
            }
        }
    }
}
