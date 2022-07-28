using common;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetFame : Command
        {
            public SetFame() : base("setfame", permLevel: 90)
            {
            }

            // resets leaderboards, account stars, and account fame to 0
            protected override bool Process(Player player, TickData time, string args)
            {
                var amount = args.ToInt32();

                if (string.IsNullOrEmpty(args))
                {
                    player.SendInfo("/setfame <amount>");
                    return false;
                }
                player.Client.Account.Reload("fame");
                player.Client.Account.Reload("totalFame");
                player.CurrentFame = player.Client.Account.Fame += amount;
                player.Client.Account.TotalFame += amount;
                player.CoreServerManager.Database.ReloadAccount(player.Client.Account);
                return true;
            }
        }
    }
}
