using System.Collections.Generic;
using TKR.WorldServer.core;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.core.objects.connection
{
    public class ConnectedObject : StaticObject
    {
        public ConnectedObject(GameServer manager, ushort objType) : base(manager, objType, null, true, false, true)
        { }

        public ConnectedObjectInfo Connection { get; set; }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            stats[StatDataType.ObjectConnection] = (int)Connection.Bits;

            base.ExportStats(stats, isOtherPlayer);
        }
    }
}
