using TKR.Shared;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.impl;
using System;
using TKR.WorldServer.core.worlds;

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

                if(manager.WorldManager.Nexus.EngineStage == 0)
                {
                    player.SendInfo("Engine isnt running");
                    return true;
                }

                var timeNow = DateTime.UtcNow.ToUnixTimestamp();
                var engineTime = (manager.WorldManager.Nexus.EngineStageTime + (manager.WorldManager.Nexus.EngineStage * 3600)) - timeNow;

                int hours = engineTime / 3600;
                int minutes = (engineTime % 3600) / 60;
                int seconds = engineTime % 60;

                string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";

                player.SendInfo($"Engine time remaining: {formattedTime}");
                return true;
            }
        }
    }
}
