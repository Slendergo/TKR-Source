using System.Collections.Generic;

namespace wServer.core.objects.connection
{
    public class ConnectedObject : StaticObject
    {
        public ConnectedObject(CoreServerManager manager, ushort objType) : base(manager, objType, null, true, false, true)
        { }

        public ConnectedObjectInfo Connection { get; set; }

        public override bool HitByProjectile(Projectile projectile, TickTime time) => true;

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.ObjectConnection] = (int)Connection.Bits;

            base.ExportStats(stats);
        }
    }
}
