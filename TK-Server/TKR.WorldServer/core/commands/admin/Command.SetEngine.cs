using TKR.Shared;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using System;
using TKR.WorldServer.core.miscfile.thread;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetEngine : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "se";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var manager = player.GameServer;
                var success = int.TryParse(args, out var amount);
                if (!success)
                {
                    player.SendInfo("Enter valid amount");
                    return true;
                }

                if(!manager.WorldManager.Nexus.TryAddFuelToEngine(player, amount))
                {
                    player.SendError($"Engine is max capacity");
                    return true;
                }

                player.SendInfo($"Added {amount} fuel to engine");
                return true;
            }
        }

        internal class CheckEngineTime : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "enginetime";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var manager = player.GameServer;


                var timeNow = DateTime.UtcNow.ToUnixTimestamp();
                var engineTime = timeNow - manager.WorldManager.Nexus.EngineStageTime + (manager.WorldManager.Nexus.EngineStage * 3600);


                player.SendInfo($"Engine has "+engineTime+" seconds left");
                return true;
            }
        }
    }
}
