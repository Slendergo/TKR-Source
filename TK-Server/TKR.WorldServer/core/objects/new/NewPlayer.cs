using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Net;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.logic;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.objects.@new
{
    public sealed class NewPlayer : EntityBase
    {
        public const int VISIBILITY_RADIUS = 15;
        public const int VISIBILITY_RADIUS_SQR = VISIBILITY_RADIUS * VISIBILITY_RADIUS;
        public const int VISIBILITY_CIRCUMFERENCE_SQR = (VISIBILITY_RADIUS - 2) * (VISIBILITY_RADIUS - 2);

        private readonly Client Client;

        public int MaximumHealth
        {
            get => StatManager.GetIntStat(StatDataType.MaximumHeath);
            set => StatManager.SetIntStat(StatDataType.MaximumHeath, value);
        }
        public int Health
        {
            get => StatManager.GetIntStat(StatDataType.Health);
            set => StatManager.SetIntStat(StatDataType.Health, value);
        }
        public int MaximumMana
        {
            get => StatManager.GetIntStat(StatDataType.MaximumMana);
            set => StatManager.SetIntStat(StatDataType.MaximumMana, value);
        }
        public int Mana
        {
            get => StatManager.GetIntStat(StatDataType.Mana);
            set => StatManager.SetIntStat(StatDataType.Mana, value);
        }
        public int ExperienceGoal
        {
            get => StatManager.GetIntStat(StatDataType.ExperienceGoal);
            set => StatManager.SetIntStat(StatDataType.ExperienceGoal, value);
        }
        public int Experience
        {
            get => StatManager.GetIntStat(StatDataType.Experience);
            set => StatManager.SetIntStat(StatDataType.Experience, value);
        }
        public int Level
        {
            get => StatManager.GetIntStat(StatDataType.Level);
            set => StatManager.SetIntStat(StatDataType.Level, value);
        }
        //public Inventory Inventory { get; private set; }
        public int Attack
        {
            get => StatManager.GetIntStat(StatDataType.Attack);
            set => StatManager.SetIntStat(StatDataType.Attack, value);
        }
        public int Defense
        {
            get => StatManager.GetIntStat(StatDataType.Defense);
            set => StatManager.SetIntStat(StatDataType.Defense, value);
        }
        public int Speed
        {
            get => StatManager.GetIntStat(StatDataType.Speed);
            set => StatManager.SetIntStat(StatDataType.Speed, value);
        }
        public int Dexterity
        {
            get => StatManager.GetIntStat(StatDataType.Dexterity);
            set => StatManager.SetIntStat(StatDataType.Dexterity, value);
        }
        public int Wisdom
        {
            get => StatManager.GetIntStat(StatDataType.Wisdom);
            set => StatManager.SetIntStat(StatDataType.Wisdom, value);
        }
        public int Vitality
        {
            get => StatManager.GetIntStat(StatDataType.Vitality);
            set => StatManager.SetIntStat(StatDataType.Vitality, value);
        }
        public ConditionEffectManager ConditionEffectManager { get; private set; }
        public int Stars
        {
            get => StatManager.GetIntStat(StatDataType.Stars);
            set => StatManager.SetIntStat(StatDataType.Stars, value);
        }
        public int Texture1
        {
            get => StatManager.GetIntStat(StatDataType.Texture1);
            set => StatManager.SetIntStat(StatDataType.Texture1, value);
        }
        public int Texture2
        {
            get => StatManager.GetIntStat(StatDataType.Texture2);
            set => StatManager.SetIntStat(StatDataType.Texture2, value);
        }
        public int Credits
        {
            get => StatManager.GetIntStat(StatDataType.Credits);
            set => StatManager.SetIntStat(StatDataType.Credits, value);
        }
        public int AccountId
        {
            get => StatManager.GetIntStat(StatDataType.AccountId);
            set => StatManager.SetIntStat(StatDataType.AccountId, value);
        }
        public int CurrentFame
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int HPBoost
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int MPBoost
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int AttackBonus
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int DefenseBonus
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int SpeedBonus
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int DexterityBonus
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int VitalityBonus
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int WisdomBonus
        {
            get => StatManager.GetIntStat(StatDataType.CurrentFame);
            set => StatManager.SetIntStat(StatDataType.CurrentFame, value);
        }
        public int Fame
        {
            get => StatManager.GetIntStat(StatDataType.Fame);
            set => StatManager.SetIntStat(StatDataType.Fame, value);
        }
        public int FameGoal
        {
            get => StatManager.GetIntStat(StatDataType.FameGoal);
            set => StatManager.SetIntStat(StatDataType.FameGoal, value);
        }
        public int Glow
        {
            get => StatManager.GetIntStat(StatDataType.Glow);
            set => StatManager.SetIntStat(StatDataType.Glow, value);
        }
        public int SinkOffset
        {
            get => StatManager.GetIntStat(StatDataType.SinkOffset);
            set => StatManager.SetIntStat(StatDataType.SinkOffset, value);
        }
        public string GuildName
        {
            get => StatManager.GetStringStat(StatDataType.GuildName);
            set => StatManager.SetStringStat(StatDataType.GuildName, value);
        }
        public int GuildRank
        {
            get => StatManager.GetIntStat(StatDataType.GuildRank);
            set => StatManager.SetIntStat(StatDataType.GuildRank, value);
        }
        public float Breath
        {
            get => StatManager.GetFloatStat(StatDataType.Breath);
            set => StatManager.SetFloatValue(StatDataType.Breath, value);
        }
        public int HeathPotionStackCount
        {
            get => StatManager.GetIntStat(StatDataType.HealthStackCount);
            set => StatManager.SetIntStat(StatDataType.HealthStackCount, value);
        }
        public int MagicPotionStackCount
        {
            get => StatManager.GetIntStat(StatDataType.MagicStackCount);
            set => StatManager.SetIntStat(StatDataType.MagicStackCount, value);
        }
        public int Skin
        {
            get => StatManager.GetIntStat(StatDataType.Skin);
            set => StatManager.SetIntStat(StatDataType.Skin, value);
        }
        public int Rank
        {
            get => StatManager.GetIntStat(StatDataType.Rank);
            set => StatManager.SetIntStat(StatDataType.Rank, value);
        }
        public int LootDropBoostTime
        {
            get => StatManager.GetIntStat(StatDataType.LDBoostTime);
            set => StatManager.SetIntStat(StatDataType.LDBoostTime, value);
        }
        public int BaseStat
        {
            get => StatManager.GetIntStat(StatDataType.BaseStat);
            set => StatManager.SetIntStat(StatDataType.BaseStat, value);
        }
        public int ColorChat
        {
            get => StatManager.GetIntStat(StatDataType.ColorChat);
            set => StatManager.SetIntStat(StatDataType.ColorChat, value);
        }
        public int ColorNameChat
        {
            get => StatManager.GetIntStat(StatDataType.ColorNameChat);
            set => StatManager.SetIntStat(StatDataType.ColorNameChat, value);
        }
        public bool XPBoosted
        {
            get => StatManager.GetBoolStat(StatDataType.XPBoosted);
            set => StatManager.SetBoolStat(StatDataType.XPBoosted, value);
        }
        public int XPBoostTime
        {
            get => StatManager.GetIntStat(StatDataType.XPBoostTime);
            set => StatManager.SetIntStat(StatDataType.XPBoostTime, value);
        }
        public int PartyId
        {
            get => StatManager.GetIntStat(StatDataType.PartyId);
            set => StatManager.SetIntStat(StatDataType.PartyId, value);
        }
        //public StatPotionStorage StatPotionStorage { get; private set; }
        public int TalismanEffects
        {
            get => StatManager.GetIntStat(StatDataType.TALISMAN_EFFECT_MASK_STAT);
            set => StatManager.SetIntStat(StatDataType.TALISMAN_EFFECT_MASK_STAT, value);
        }

        private int TickId;

        public NewPlayer(Client client, World world, ObjectDesc objectDesc) : base(world, objectDesc)
        {
            Client = client;

            var account = client.Account;
            var character = client.Character;

            MaximumHealth = character.Stats[0];
            Health = character.Health;
            MaximumMana = character.Stats[1];
            Mana = character.Mana;
            ExperienceGoal = GetExpGoal(character.Level);
            Experience = character.Experience;
            Level = character.Level;
            Attack = character.Stats[2];
            Defense = character.Stats[3];
            Speed = character.Stats[4];
            Dexterity = character.Stats[5];
            Vitality = character.Stats[6];
            Wisdom = character.Stats[7];
            Stars = GetStars();
            Texture1 = character.Texture1;
            Texture2 = character.Texture2;
            Credits = account.Credits;
            AccountId = account.AccountId;
            CurrentFame = account.Fame;
            Fame = character.Fame;
            FameGoal = GetFameGoal(0); // FameCounter.[OClassStatsbjectType].BestFame  
            Glow = 0xFF0000; // todo legends
            //SinkOffset // todo in future

            var guild = world.GameServer.Database.GetGuild(account.GuildId);
            if (guild != null)
            {
                GuildName = guild.Name;
                GuildRank = client.Account.GuildRank;
            }

            if (world.IdName.Equals("Ocean Trench"))
                Breath = 100.0f;

            HeathPotionStackCount = character.HealthStackCount;
            MagicPotionStackCount = character.MagicStackCount;
            Skin = character.Skin;
            Rank = 1000; // todo fix
            LootDropBoostTime = character.LootDropBoostTime;
            BaseStat = account.SetBaseStat;
            ColorChat = account.ColorChat;
            ColorNameChat = account.ColorNameChat;
            if(character.XPBoostTime > 0)
            {
                XPBoosted = true;
                XPBoostTime = character.XPBoostTime;
            }
            PartyId = account.PartyId;
        }

        public override void OnAddedToWorld()
        {
            // do something on added?
        }

        public override void Update(ref TickTime tickTime)
        {
            // handle logic first
            // then handle update
            //then flush state with newtick

            HandleUpdate();
            HandleNewTick(tickTime.ElapsedMsDelta);
        }

        public bool IsInGame { get; private set; }

        private HashSet<IntPoint> VisibleTileList = new HashSet<IntPoint>();
        private Dictionary<int, EntityBase> VisibleEntityList = new Dictionary<int, EntityBase>();
        private Dictionary<int, byte> SeenTiles = new Dictionary<int, byte>();

        private List<IntPoint> PendingTileVisibleList = new List<IntPoint>();
        private List<EntityBase> PendingAddVisibleList = new List<EntityBase>();
        private List<int> PendingRemoveVisibleList = new List<int>();
        private bool NeedsUpdateAck;

        public void UpdateAck()
        {
            foreach (var tile in PendingTileVisibleList)
            {
                SeenTiles[(tile.X << 16) | tile.Y]++;
                _ = VisibleTileList.Add(tile);
            }
            PendingTileVisibleList.Clear();

            foreach (var add in PendingAddVisibleList)
                VisibleEntityList.Add(add.Id, add);
            PendingAddVisibleList.Clear();

            foreach (var drop in PendingRemoveVisibleList)
                _ = VisibleEntityList.Remove(drop);
            PendingRemoveVisibleList.Clear();

            IsInGame = true;
            NeedsUpdateAck = false;
        }

        private void HandleNewTick(int tickTime)
        {
            if (!NeedsUpdateAck)
                return;
            
            TickId++;

            var newTick = new NewTick()
            {
                TickId = TickId,
                TickTime = tickTime,
            };
            newTick.Statuses = new List<ObjectStats>();

            Client.SendMessage(newTick);
        }

        private void HandleUpdate()
        {
            var tiles = GetTiles();
            var objects = GetObjects();
            var drops = GetDrops();

            if (tiles.Count > 0 || objects.Count > 0 || drops.Count > 0)
            {
                var update = new Update(tiles, objects, drops);
                Client.SendMessage(update);
            }
        }

        private List<TileData> GetTiles()
        {
            var ret = new List<TileData>();

            PendingTileVisibleList.Clear();
            switch (World.Blocking)
            {
                case 1:
                    {
                        var px = (int)X;
                        var py = (int)Y;

                        foreach (var point in CircleCircumferenceSightPoints)
                            Visibility.DrawLine(px, py, px + point.X, py + point.Y, (x, y) =>
                            {
                                var tile = World.Map[x, y];

                                var hash = (x << 16) | y;
                                _ = SeenTiles.TryGetValue(hash, out var updateCount);
                                if (tile.TileId != 0xFF || updateCount >= tile.UpdateCount)
                                {
                                    PendingTileVisibleList.Add(new IntPoint(x, y));
                                    ret.Add(new TileData()
                                    {
                                        X = x,
                                        Y = y,
                                        Tile = tile.TileId
                                    });
                                }
                                return World.Visibility.IsBlocking(x, y);
                            });
                    }
                    break;
                default:
                    {
                        foreach (var point in SightPoints)
                        {
                            var x = (int)(point.X + X);
                            var y = (int)(point.Y + Y);

                            var tile = World.Map[x, y];

                            var hash = (x << 16) | y;
                            _ = SeenTiles.TryGetValue(hash, out var updateCount);
                            if (tile.TileId != 0xFF || updateCount >= tile.UpdateCount)
                            {
                                PendingTileVisibleList.Add(new IntPoint(x, y));
                                ret.Add(new TileData()
                                {
                                    X = x,
                                    Y = y,
                                    Tile = tile.TileId
                                });
                            }
                        }
                    }
                    break;

            }
            //FameCounter.TileSent(ret.Count); // adds the new amount to the tiles been sent
            return ret;
        }

        private List<ObjectDef> GetObjects()
        {
            var ret = new List<ObjectDef>();

            var players = World.Census.GetPlayers();
            foreach (var player in players)
            {
                if (player.Dead || VisibleEntityList.ContainsKey(player.Id))
                    continue; 

                PendingAddVisibleList.Add(player);
                ret.Add(player.StatManager.Get());
            }

            var intPoint = new IntPoint(0, 0);
            var entities = World.Census.UpdateEntitiesWithinRadius(X, Y, VISIBILITY_RADIUS);
            foreach (var entity in entities)
            {
                if (entity.Dead)
                    continue;

                if (entity is NewContainer container)
                    if (container.BagOwners.Length > 0 && Array.IndexOf(container.BagOwners, AccountId) == -1)
                        continue;

                intPoint.X = (int)entity.X;
                intPoint.Y = (int)entity.Y;
                if (!PendingTileVisibleList.Contains(intPoint))
                    continue;

                PendingAddVisibleList.Add(entity);
                ret.Add(entity.StatManager.Get());
            }
            return ret;
        }

        private List<int> GetDrops()
        {
            var ret = new List<int>();

            return ret;
        }

        public override void OnRemovedFromWorld()
        {
            // do something on removed?
        }

        public int GetStars()
        {
            // todo
            return 0;

            //int ret = 0;
            //foreach (var i in FameCounter.ClassStats.AllKeys)
            //{
            //    var entry = FameCounter.ClassStats[ushort.Parse(i)];
            //    if (entry.BestFame >= 2000) ret += 5;
            //    else if (entry.BestFame >= 800) ret += 4;
            //    else if (entry.BestFame >= 400) ret += 3;
            //    else if (entry.BestFame >= 150) ret += 2;
            //    else if (entry.BestFame >= 20) ret += 1;
            //}
            //return ret;
        }

        public static int GetLevelExp(int level) => level == 1 ? 0 : 50 * (level - 1) + (level - 2) * (level - 1) * 50;
        public static int GetExpGoal(int level) => 50 + (level - 1) * 100;
        public static int GetFameGoal(int fame)
        {
            if (fame >= 2000) return 0;
            if (fame >= 800) return 2000;
            if (fame >= 400) return 800;
            if (fame >= 150) return 400;
            if (fame >= 20) return 150;
            return 20;
        }

        private static readonly IntPoint[] SurroundingPoints = new IntPoint[]
        {
            new IntPoint(0, 0),
            new IntPoint(1, 0),
            new IntPoint(0, 1),
            new IntPoint(-1, 0),
            new IntPoint(0, -1)
        };

        private static HashSet<IntPoint> CircleCircumferenceSightPoints = CircleCircumferenceSightPoints ??= CachePoints(true);
        private static HashSet<IntPoint> SightPoints = SightPoints ??= CachePoints();
        private static HashSet<IntPoint> CachePoints(bool circumferenceCheck = false)
        {
            var ret = new HashSet<IntPoint>();
            for (var x = -VISIBILITY_RADIUS; x <= VISIBILITY_RADIUS; x++)
                for (var y = -VISIBILITY_RADIUS; y <= VISIBILITY_RADIUS; y++)
                {
                    var flag = x * x + y * y <= VISIBILITY_RADIUS_SQR;
                    if (circumferenceCheck)
                        flag &= x * x + y * y > VISIBILITY_CIRCUMFERENCE_SQR;
                    if (flag)
                        _ = ret.Add(new IntPoint(x, y));
                }
            return ret;
        }
    }
}
