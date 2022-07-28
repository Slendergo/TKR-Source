using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class TransferDamageOnDeath : Behavior
    {
        private readonly float _radius;
        private readonly ushort _target;

        public TransferDamageOnDeath(string target, float radius = 50)
        {
            _target = GetObjType(target);
            _radius = radius;
        }

        protected internal override void Resolve(State parent) => parent.Death += (sender, e) =>
        {
            if (!(e.Host is Enemy enemy))
                return;

            if (!(e.Host.GetNearestEntity(_radius, _target) is Enemy targetObj))
                return;

            enemy.DamageCounter.TransferData(targetObj.DamageCounter);
        };

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
