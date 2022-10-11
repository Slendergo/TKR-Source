using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.objects.@new
{
    public sealed class NewStatic : EntityBase
    {
        public int Connects
        {
            get => StatManager.GetIntStat(StatDataType.ObjectConnection);
            set => StatManager.SetIntStat(StatDataType.ObjectConnection, value);
        }

        public NewStatic(World world, ObjectDesc objectDesc) : base(world, objectDesc)
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
