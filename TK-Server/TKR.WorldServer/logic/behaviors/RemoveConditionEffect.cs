using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.behaviors
{
    internal class RemoveConditionalEffect : Behavior
    {
        private readonly ConditionEffectIndex _effect;

        public RemoveConditionalEffect(ConditionEffectIndex effect) => _effect = effect;

        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => host.RemoveCondition(_effect);

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
