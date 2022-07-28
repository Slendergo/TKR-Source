using CA.Extensions.Concurrent;
using common.database;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class KillAll : Command
        {
            public KillAll() : base("killAll", permLevel: 60, alias: "ka")
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (!(player.Owner is Vault) && player.Rank < 110)
                {
                    player.SendError("Only in your Vault.");
                    return false;
                }

                var eligibleEnemies = player.Owner.Enemies
                    .ValueWhereAsParallel(_ => _.ObjectDesc != null
                        && _.ObjectDesc.ObjectId != null
                        && _.ObjectDesc.Enemy
                        && _.ObjectDesc.ObjectId != "Tradabad Nexus Crier"
                        && _.ObjectDesc.ObjectId.ContainsIgnoreCase(args)
                    );
                var total = eligibleEnemies.Length;
                for (var i = 0; i < total; i++)
                {
                    var enemy = eligibleEnemies[i];
                    enemy.Spawned = true;
                    enemy.Death(time);
                }

                player.SendInfo($"{total} enem{(total > 1 ? "ies" : "y")} killed!");
                return true;
            }
        }
    }
}
