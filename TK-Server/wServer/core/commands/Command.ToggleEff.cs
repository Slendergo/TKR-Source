using common.resources;
using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ToggleEff : Command
        {
            public ToggleEff() : base("eff", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (!Enum.TryParse(args, true, out ConditionEffectIndex effect))
                {
                    player.SendError("Invalid effect!");
                    return false;
                }

                var target = player;
                if ((target.ConditionEffects & (ConditionEffects)((ulong)1 << (int)effect)) != 0)
                {
                    //remove
                    target.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = effect,
                        DurationMS = 0
                    });
                }
                else
                {
                    //add
                    target.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = effect,
                        DurationMS = -1
                    });
                }
                return true;
            }
        }
    }
}
