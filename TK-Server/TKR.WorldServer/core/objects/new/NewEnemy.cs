using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.objects.@new
{
    public sealed class NewEnemy : EntityBase
    {
        public NewEnemy(World world, ObjectDesc objectDesc) : base(world, objectDesc)
        {

        }

        public override void OnAddedToWorld() => throw new System.NotImplementedException();
        public override void Update(ref TickTime tickTime) => throw new System.NotImplementedException();
        public override void OnRemovedFromWorld() => throw new System.NotImplementedException();
    }
}
