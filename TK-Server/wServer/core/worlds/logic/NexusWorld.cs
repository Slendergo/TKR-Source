using common.resources;
using wServer.core.objects;
using wServer.core.setpieces;

namespace wServer.core.worlds.logic
{
    public sealed class NexusWorld : World
    {
        private int MARKET_BOUNDS_SIZE = 31;
        private Rect? MarketBounds;

        public KingdomPortalMonitor PortalMonitor { get; private set; }

        public NexusWorld(GameServer gameServer, int id, WorldResource resource) : base(gameServer, id, resource)
        {
        }

        public override void Init()
        {
            var marketRegions = GetRegionPoints(TileRegion.Hallway);

            if (marketRegions.Length > 0)
            {
                var point = marketRegions[0];

                MarketBounds = new Rect()
                {
                    x = point.Key.X,
                    y = point.Key.Y,
                    w = MARKET_BOUNDS_SIZE,
                    h = MARKET_BOUNDS_SIZE
                };
            }


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

        public bool WithinBoundsOfMarket(float x, float y)
        {
            if (!MarketBounds.HasValue)
                return false;
            return Rect.ContainsPoint(MarketBounds.Value, x, y);
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }
    }
}
