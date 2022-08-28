using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.core.objects.vendors;
using wServer.core.setpieces;

namespace wServer.core.worlds.logic
{
    public sealed class MerchantData
    {
        public TileRegion TileRegion;
        public float TimeToSpawn;
        public IntPoint Position;
        public NexusMerchant NewMerchant;
        public ISellableItem SellableItem;
        public CurrencyType CurrencyType;
        public int RankRequired;
    }

    public sealed class NexusWorld : World
    {
        private const int ENGINE_STAGE1_TIMEOUT = 3600; //seconds
        private const int ENGINE_STAGE2_TIMEOUT = ENGINE_STAGE1_TIMEOUT * 2;
        private const int ENGINE_STAGE3_TIMEOUT = ENGINE_STAGE1_TIMEOUT * 3;

        private const int ENGINE_FIRST_STAGE_AMOUNT = 100;
        private const int ENGINE_SECOND_STAGE_AMOUNT = 250 + ENGINE_FIRST_STAGE_AMOUNT;
        private const int ENGINE_THIRD_STAGE_AMOUNT = 500 + ENGINE_SECOND_STAGE_AMOUNT;

        private int MARKET_BOUNDS_WIDTH = 33;
        private int MARKET_BOUNDS_HEIGHT = 43;

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
                    w = MARKET_BOUNDS_WIDTH,
                    h = MARKET_BOUNDS_HEIGHT
                };
            }


            PortalMonitor = new KingdomPortalMonitor(GameServer, this);

            foreach (var shop in MerchantLists.Shops)
            {
                var positions = Map.Regions.Where(r => shop.Key == r.Value);
                foreach (var data in positions)
                {
                    var merchantData = new MerchantData();
                    merchantData.TileRegion = data.Value;
                    merchantData.Position = data.Key;
                    InactiveStorePoints.Add(merchantData);
                }
            }

            base.Init();
        }
        public override int EnterWorld(Entity entity)
        {
            if((entity is Player) && EngineStage != 0) 
                (entity as Player).EngineNotif($"The engine runs smoothly with enough fuel to last a while.");
            return base.EnterWorld(entity);
        }

        private List<MerchantData> InactiveStorePoints = new List<MerchantData>();
        private List<MerchantData> ActiveStorePoints = new List<MerchantData>();

        public void SendOutMerchant(MerchantData merchantData)
        {
            InactiveStorePoints.Remove(merchantData);
            if (MerchantLists.Shops.TryGetValue(merchantData.TileRegion, out var data))
            {
                var items = data.Item1;
                if (items.Count == 0)
                    return;

                var x = merchantData.Position.X;
                var y = merchantData.Position.Y;
                merchantData.NewMerchant = new NexusMerchant(GameServer, 0x01ca);
                merchantData.NewMerchant.Move(x + 0.5f, y + 0.5f);
                merchantData.CurrencyType = data.Item2;
                merchantData.RankRequired = data.Item3;

                merchantData.SellableItem = Random.NextLength(items);
                merchantData.NewMerchant.SetData(merchantData);
                _ = MerchantLists.Shops[merchantData.TileRegion].Item1.Remove(merchantData.SellableItem);

                _ = EnterWorld(merchantData.NewMerchant);
                ActiveStorePoints.Add(merchantData);
            }
        }

        private void HandleMerchants(ref TickTime time)
        {
            var merchantsToAdd = new List<MerchantData>();
            foreach (var merchantData in InactiveStorePoints)
            {
                merchantData.TimeToSpawn -= time.DeltaTime;
                if(merchantData.TimeToSpawn <= 0.0f)
                    merchantsToAdd.Add(merchantData);
            }

            foreach (var merchant in merchantsToAdd)
                SendOutMerchant(merchant);
        }

        public void ReturnMerchant(MerchantData merchantData)
        {
            merchantData.TimeToSpawn = 10.0f;
            MerchantLists.Shops[merchantData.TileRegion].Item1.Add(merchantData.SellableItem);
            InactiveStorePoints.Add(merchantData);
            LeaveWorld(merchantData.NewMerchant);
            merchantData.NewMerchant = null;
        }

        public bool WithinBoundsOfMarket(float x, float y)
        {
            if (!MarketBounds.HasValue)
                return false;
            return Rect.ContainsPoint(MarketBounds.Value, x, y);
        }

        protected override void UpdateLogic(ref TickTime time)
        {
            HandleMerchants(ref time);
            HandleEngineTimeouts(ref time);
            PortalMonitor.Update(ref time);
            base.UpdateLogic(ref time);
        }

        private void HandleEngineTimeouts(ref TickTime time)
        {
            var currentTime = DateTime.UtcNow.ToUnixTimestamp();
            switch (EngineStage)
            {
                case 1:
                    if (currentTime >= EngineStageTime + ENGINE_STAGE1_TIMEOUT)
                    {
                        ResetEngineState(1);
                        EngineStageTime = currentTime;
                    }
                    break;
                case 2:
                    if (currentTime >= EngineStageTime + ENGINE_STAGE2_TIMEOUT)
                    {
                        ResetEngineState(2);
                        EngineStageTime = currentTime;
                    }
                    break;
                case 3:
                    if (currentTime >= EngineStageTime + ENGINE_STAGE3_TIMEOUT)
                    {
                        ResetEngineState(3);
                        EngineStageTime = currentTime;
                    }
                    break;
                default: break;
            }
        }

        private void ResetEngineState(int state)
        {
            //Player.GameServer.ChatManager.AnnounceEngine($"The machine slowly powers down");
            TryAddFuelToEngine(null, state == 1 ? -100 : state == 2 ? -250 : state == 3 ? -500 : 0);
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
                        SetEngineSetStage(1, player);
                    else if (current >= ENGINE_SECOND_STAGE_AMOUNT && current < ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(2, player);
                    else if (current >= ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(3, player);
                    break;
                case 1:
                    if (current < ENGINE_FIRST_STAGE_AMOUNT)
                        SetEngineSetStage(0, player);
                    else if (current >= ENGINE_SECOND_STAGE_AMOUNT && current < ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(2, player);
                    else if (current >= ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(3, player);
                    break;
                case 2:
                    if (current < ENGINE_FIRST_STAGE_AMOUNT)
                        SetEngineSetStage(0, player);
                    else if (current >= ENGINE_FIRST_STAGE_AMOUNT && current < ENGINE_SECOND_STAGE_AMOUNT)
                        SetEngineSetStage(1, player);
                    else if (current >= ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(3, player);
                    break;
                case 3:
                    if (current < ENGINE_FIRST_STAGE_AMOUNT)
                        SetEngineSetStage(0, player);
                    else if (current >= ENGINE_FIRST_STAGE_AMOUNT && current < ENGINE_SECOND_STAGE_AMOUNT)
                        SetEngineSetStage(1, player);
                    else if (current >= ENGINE_SECOND_STAGE_AMOUNT && current < ENGINE_THIRD_STAGE_AMOUNT)
                        SetEngineSetStage(2, player);
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

        private void SetEngineSetStage(int state, Player player)
        {
            EngineStage = state;
            if(player != null)
                GameServer.ChatManager.AnnounceEngine($"[{player.Name}] adds the last bit of fuel and kicks the machine, it powers on to Stage "+state+"!");
            else
                GameServer.ChatManager.AnnounceEngine($"The Strange Engine slowly powers down.");
            var engine = GameServer.Database.GetDbEngine();
            engine.SetEngineStage(state, DateTime.UtcNow.ToUnixTimestamp());
            engine.Save();
        }
    }
}
