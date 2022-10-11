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
            if(objectDesc.ObjectId != "Guild Hall Portal")
                Usable = true;
        }

        public override void OnAddedToWorld()
        {
        }

        public override void Update(ref TickTime tickTime) 
        {
        }

        public override void OnRemovedFromWorld() 
        {
        }
    }
}
