using common.resources;
using wServer.core.objects;

namespace wServer.core.worlds.logic
{
    public sealed class NexusWorld : World
    {
        public KingdomPortalMonitor PortalMonitor { get; private set; }

        public NexusWorld(int id, WorldResource resource) : base(id, resource)
        {
        }

        public override void Init()
        {
            PortalMonitor = new KingdomPortalMonitor(GameServer, this);
            base.Init();
        }

        //hacky ATTEMPT TO FIX THIS DAMN THING cus the damn edit xOffset:0.5;yOffset:0.5; wont work
        //GIWEBULANWAOI[GAWJQAWP'GAW
        public override int EnterWorld(Entity entity)
        {
            System.Console.WriteLine(entity.ObjectDesc.ObjectId);
            if (entity.ObjectDesc.ObjectId == "Market NPC")
                entity.Move(entity.X + 0.5f, entity.Y + 0.5f);
            return base.EnterWorld(entity);
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }
    }
}
