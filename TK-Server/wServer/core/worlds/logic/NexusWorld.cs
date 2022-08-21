using common;
using common.resources;
using System;
using wServer.core.objects;
using wServer.core.setpieces;

namespace wServer.core.worlds.logic
{
    public sealed class NexusWorld : World
    {
        private int ENGINE_STAGE1_TIMEOUT = 3600; //seconds
        private int ENGINE_STAGE2_TIMEOUT = 10800;
        private int ENGINE_STAGE3_TIMEOUT = 21600;
        private int MARKET_BOUNDS_SIZE = 31;
        private Rect? MarketBounds;

        public KingdomPortalMonitor PortalMonitor { get; private set; }

        public int EngineState;
        public int State;

        public bool s1;
        public bool s2;
        public bool s3;
        public bool change = true;

        public NexusWorld(GameServer gameServer, int id, WorldResource resource) : base(gameServer, id, resource)
        {
            //EngineState = 3;
        }

        public override void Init()
        {
            var lootRegions = GetRegionPoints(TileRegion.Hallway_1);
            foreach (var loot in lootRegions)
            {
                var market = Entity.Resolve(GameServer, "Engine");
                market.Move(loot.Key.X + 0.5f, loot.Key.Y + 0.5f);
                EnterWorld(market);
            }

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
        }

        public bool WithinBoundsOfMarket(float x, float y)
        {
            if (!MarketBounds.HasValue)
                return false;
            return Rect.ContainsPoint(MarketBounds.Value, x, y);
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            System.Console.WriteLine("CurrentValue:" + GameServer.Database.GetEngineFuel()); //DateTime.UtcNow.ToUnixTimestamp()
            var timeNow = DateTime.UtcNow.ToUnixTimestamp();
            var fuel = GameServer.Database.GetEngineFuel();
            if (!s1)
            {
                if (fuel >= 100)
                {
                    State = 1;
                    s1 = true;
                    change = true;
                    GameServer.Database.EngineSetTime(1, DateTime.UtcNow.ToUnixTimestamp());
                }
            }
            else if (!s2)
            {
                if (fuel >= 250)
                {
                    State = 2;
                    s2 = true;
                    change = true;
                    GameServer.Database.EngineSetTime(2, DateTime.UtcNow.ToUnixTimestamp());
                }
            }
            else if (!s3)
            {
                if (fuel >= 500)
                {
                    State = 3;
                    s3 = true;
                    change = true;
                    GameServer.Database.EngineSetTime(3, DateTime.UtcNow.ToUnixTimestamp());
                }
            }
            else
            {
                if (fuel < 100)
                {
                    State = 0;
                    s1 = false;
                    s2 = false;
                    s3 = false;
                    change = true;
                }
            }
            if (GameServer.Database.GetEngineTime() != 0 && GameServer.Database.GetEngineTime() + (s3 ? ENGINE_STAGE3_TIMEOUT : s2 ? ENGINE_STAGE2_TIMEOUT : s1 ? ENGINE_STAGE1_TIMEOUT : 0) < timeNow)
            {
                GameServer.Database.EngineFlush();
                State = 0;
                s1 = false;
                s2 = false;
                s3 = false;
                change = true;
            }
            if (change)
            {
                EngineState = State;
                change = false;
            }
            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }
    }
}
