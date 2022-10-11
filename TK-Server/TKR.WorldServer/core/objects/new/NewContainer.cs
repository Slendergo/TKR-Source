using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.objects.@new
{
    public sealed class NewContainer : EntityBase
    {
        public int[] BagOwners { get; private set; }
        //public ContainerInventory { get; private int; }

        public NewContainer(World world, ObjectDesc objectDesc) : base(world, objectDesc)
        {
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
