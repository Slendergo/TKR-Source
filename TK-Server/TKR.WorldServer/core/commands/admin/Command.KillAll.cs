using System.Linq;
using TKR.Shared;
using TKR.Shared.utils;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class KillAll : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "killall";

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (!(player.World is VaultWorld) && !player.IsAdmin)
                {
                    player.SendError("Only in your Vault.");
                    return false;
                }

                var total = 0;
                foreach(var entity in player.World.Enemies.Values)
                {
                    if(entity.Dead || entity.ObjectDesc == null
                        || entity.ObjectDesc.IdName == null
                        || !entity.ObjectDesc.Enemy
                        || !entity.ObjectDesc.IdName.ContainsIgnoreCase(args))
                        continue;
                    entity.Expunge();
                    total++;
                }
                player.SendInfo($"{total} enem{(total > 1 ? "ies" : "y")} killed!");
                return true;
            }
        }
    }
}
