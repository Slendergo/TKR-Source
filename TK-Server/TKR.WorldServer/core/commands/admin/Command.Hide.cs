﻿using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class Hide : Command
        {
            public override RankingType RankRequirement => RankingType.CommunityModerator;
            public override string CommandName => "hide";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var acc = player.Client.Account;

                acc.Hidden = !acc.Hidden;
                acc.FlushAsync();

                if (acc.Hidden)
                {
                    player.ApplyPermanentConditionEffect(ConditionEffectIndex.Hidden);
                    player.ApplyPermanentConditionEffect(ConditionEffectIndex.Invincible);
                    player.GameServer.ConnectionManager.Clients[player.Client].Hidden = true;
                }
                else
                {
                    player.RemoveCondition(ConditionEffectIndex.Hidden);
                    player.RemoveCondition(ConditionEffectIndex.Invincible);
                    player.GameServer.ConnectionManager.Clients[player.Client].Hidden = false;
                }

                return true;
            }
        }
    }
}
