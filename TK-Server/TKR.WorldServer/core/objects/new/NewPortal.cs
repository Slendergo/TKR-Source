using System.Net.Sockets;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.objects.@new
{
    public sealed class NewPortal : EntityBase
    {
        public bool Usable
        {
            get => StatManager.GetBoolStat(StatDataType.PortalUsable);
            private set => StatManager.SetBoolStat(StatDataType.PortalUsable, value);
        }

        public NewPortal(World world, ObjectDesc objectDesc) : base(world, objectDesc)
        {
            Usable = objectDesc.Locked;
        }

        public override void OnAddedToWorld() => throw new System.NotImplementedException();
        public override void Update(ref TickTime tickTime) => throw new System.NotImplementedException();
        public override void OnRemovedFromWorld() => throw new System.NotImplementedException();
    }
}
