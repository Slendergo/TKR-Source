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

        public override void OnDeath(Entity host, ref TickTime time)
        {
            if (!(host is Enemy enemy))
                return;

            if (!(host.GetNearestEntity(_radius, _target) is Enemy targetObj))
                return;

            enemy.DamageCounter.TransferData(targetObj.DamageCounter);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
