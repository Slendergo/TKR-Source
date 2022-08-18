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

        public int CurrentRealmCount = 0;
        public int Max = 0;

        public override void Init()
        {
            PortalMonitor = new KingdomPortalMonitor(GameServer, this);
            base.Init();

            var lootRegions = GetRegionPoints(TileRegion.Loot);
            foreach (var loot in lootRegions)
            {
                var market = Entity.Resolve(GameServer, "Market NPC");
                market.Move(loot.Key.X + 1.0f, loot.Key.Y + 1.0f);
                EnterWorld(market);
            }
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            var count = (GameServer.ConnectionManager.PlayerCount() + 15) / GameServer.ConnectionManager.MaxPlayerCount;
            if (count != 0 && CurrentRealmCount != Max)
            {
                while (count != 0)
                {
                    count--;
                    CurrentRealmCount++;
                    PortalMonitor.CreateNewRealm();
                }
            }

            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }
    }
}
