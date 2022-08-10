using common;
using common.resources;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Hide : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "hide";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var acc = player.Client.Account;

                acc.Hidden = !acc.Hidden;
                acc.FlushAsync();

                if (acc.Hidden)
                {
                    player.ApplyConditionEffect(ConditionEffectIndex.Hidden);
                    player.ApplyConditionEffect(ConditionEffectIndex.Invincible);
                    player.GameServer.ConnectionManager.Clients[player.Client].Hidden = true;
                }
                else
                {
                    player.ApplyConditionEffect(ConditionEffectIndex.Hidden, 0);
                    player.ApplyConditionEffect(ConditionEffectIndex.Invincible, 0);
                    player.GameServer.ConnectionManager.Clients[player.Client].Hidden = false;
                }

                return true;
            }
        }
    }
}
