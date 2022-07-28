using System.Collections.Generic;

namespace wServer.core.objects
{
    public class ConnectedObject : StaticObject
    {
        public ConnectedObject(CoreServerManager manager, ushort objType) : base(manager, objType, null, true, false, true)
        { }

        public ConnectionInfo Connection { get; set; }

        public override bool HitByProjectile(Projectile projectile, TickData time) => true;

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.ObjectConnection] = (int)Connection.Bits;

            base.ExportStats(stats);
        }
    }
}
