using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetDonorLoot : Command
        {
            public SetDonorLoot() : base("setDonorLoot", permLevel: 10)
            {
            } //Donor-1

            protected override bool Process(Player player, TickData time, string color)
            {
                if (!player.Client.Account.SetDonorLoot)
                {
                    player.SendInfo("Your donor loot has been enabled, restart your character and enjoy!");
                    player.Client.Account.SetDonorLoot = true;
                }
                else
                {
                    player.SendInfo("Your donor loot has been disabled!");
                    player.Client.Account.SetDonorLoot = false;
                }

                var acc = player.Client.Account;
                acc.FlushAsync();

                return true;
            }
        }
    }
}
