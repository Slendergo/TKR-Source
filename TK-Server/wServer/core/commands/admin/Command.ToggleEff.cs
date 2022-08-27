using common;
using common.resources;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ToggleEff : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "eff";

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (!Enum.TryParse(args, true, out ConditionEffectIndex effect))
                {
                    player.SendError("Invalid effect!");
                    return false;
                }

                if (!player.HasConditionEffect(effect))
                {
                    player.ApplyPermanentConditionEffect(effect);
                    player.SendInfo($"{effect} Has been added!");
                }
                else
                {
                    player.RemoveCondition(effect);
                    player.SendInfo($"{effect} Has been removed!");
                }
                return true;
            }
        }
    }
}
