using common;
using common.resources;
using System;
using wServer.core.objects;
using wServer.core.setpieces;

namespace wServer.core.worlds.logic
{
    public sealed class NexusWorld : World
    {
        private const int ENGINE_STAGE1_TIMEOUT = 3600; //seconds
        private const int ENGINE_STAGE2_TIMEOUT = 10800;
        private const int ENGINE_STAGE3_TIMEOUT = 21600;

        private const int ENGINE_FIRST_STAGE_AMOUNT = 100;
        private const int ENGINE_SECOND_STAGE_AMOUNT = 250 + ENGINE_FIRST_STAGE_AMOUNT;
        private const int ENGINE_THIRD_STAGE_AMOUNT = 500 + ENGINE_SECOND_STAGE_AMOUNT;

        private int MARKET_BOUNDS_SIZE = 31;
        private Rect? MarketBounds;

        public KingdomPortalMonitor PortalMonitor { get; private set; }

        public int EngineStage { get; private set; }
        public int EngineFuel { get; private set; }
        public int EngineStageTime { get; private set; }
        public Engine Engine { get; private set; }

        public NexusWorld(GameServer gameServer, int id, WorldResource resource) : base(gameServer, id, resource)
        {
        }

        public override void Init()
        {
            var engine = GameServer.Database.GetDbEngine();
            EngineStage = engine.EngineStage;
            EngineFuel = engine.EngineFuel;
            EngineStageTime = engine.EngineStageTime;

            var lootRegions = GetRegionPoints(TileRegion.Hallway_1);
            foreach (var loot in lootRegions)
            {
                Engine = (Engine)Entity.Resolve(GameServer, "Engine");
                Engine.Move(loot.Key.X + 0.5f, loot.Key.Y + 0.5f);
                _ = EnterWorld(Engine);
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
            HandleEngineTimeouts(ref time);
            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }

        private void HandleEngineTimeouts(ref TickTime time)
        {
            // todo implement a system to expire the engine stages
            TryAddFuelToEngine(null, 1);
        }

        public bool TryAddFuelToEngine(Player player, int amount)
        {
            if (Engine.CurrentAmount == ENGINE_THIRD_STAGE_AMOUNT)
                return false;

            // clamp it to the max
            var current = Engine.CurrentAmount + amount;

            switch (EngineStage)
            {
                case 0:
                    if (current >= ENGINE_FIRST_STAGE_AMOUNT && current < ENGINE_SECOND_STAGE_AMOUNT)
                        SetEngineSetStage(1);
                    else if (current >= ENGINE_SECOND_STAGE_AMOUNT && current < ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(2);
                    else if (current >= ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(3);
                    break;
                case 1:
                    if (current < ENGINE_FIRST_STAGE_AMOUNT)
                        SetEngineSetStage(0);
                    else if (current >= ENGINE_SECOND_STAGE_AMOUNT && current < ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(2);
                    else if (current >= ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(3);
                    break;
                case 2:
                    if (current < ENGINE_FIRST_STAGE_AMOUNT)
                        SetEngineSetStage(0);
                    else if (current >= ENGINE_FIRST_STAGE_AMOUNT && current < ENGINE_SECOND_STAGE_AMOUNT)
                        SetEngineSetStage(1);
                    else if (current >= ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(3);
                    break;
                case 3:
                    if (current < ENGINE_FIRST_STAGE_AMOUNT)
                        SetEngineSetStage(0);
                    else if (current >= ENGINE_FIRST_STAGE_AMOUNT && current < ENGINE_SECOND_STAGE_AMOUNT)
                        SetEngineSetStage(1);
                    else if (current >= ENGINE_SECOND_STAGE_AMOUNT && current < ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(2);
                    break;
            }

            Engine.CurrentAmount = EngineFuel = Math.Min(current, ENGINE_THIRD_STAGE_AMOUNT);
            Engine.EngineTime = EngineStageTime;

            var engine = GameServer.Database.GetDbEngine();
            engine.AddFuel(player?.Name ?? "Server", amount);
            engine.Save();
            return true;
        }

        public void SetEngine(Engine engine) => Engine = engine;

        private void SetEngineSetStage(int state)
        {
            EngineStage = state;
            var engine = GameServer.Database.GetDbEngine();
            engine.SetEngineStage(state, DateTime.UtcNow.ToUnixTimestamp());
            engine.Save();
        }
    }
}
