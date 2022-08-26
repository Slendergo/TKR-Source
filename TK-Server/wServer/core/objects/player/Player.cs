using common;
using common.database;
using common.resources;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using wServer.core.net.handlers;
using wServer.core.terrain;
using wServer.core.worlds;
using wServer.core.worlds.logic;
using wServer.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;
using wServer.utils;

namespace wServer.core.objects
{
    internal interface IPlayer
    {
        void Damage(int dmg, Entity src);
        bool IsVisibleToEnemy();
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        public Client Client;

        public int AccountId { get => _accountId.GetValue(); set => _accountId.SetValue(value); }
        public int BaseStat { get => _baseStat.GetValue(); set => _baseStat.SetValue(value); }

        public double Breath
        {
            get => _breath;
            set
            {
                OxygenBar = (int)value;
                _breath = value;
            }
        }

        public int ColorChat { get => _colorchat.GetValue(); set => _colorchat.SetValue(value); }
        public int ColorNameChat { get => _colornamechat.GetValue(); set => _colornamechat.SetValue(value); }
        public int Credits { get => _credits.GetValue(); set => _credits.SetValue(value); }
        public int CurrentFame { get => _currentFame.GetValue(); set => _currentFame.SetValue(value); }
        public RInventory DbLink { get; private set; }
        public int Experience { get => _experience.GetValue(); set => _experience.SetValue(value); }
        public int ExperienceGoal { get => _experienceGoal.GetValue(); set => _experienceGoal.SetValue(value); }
        public int Fame { get => _fame.GetValue(); set => _fame.SetValue(value); }
        public FameCounter FameCounter { get; private set; }
        public int FameGoal { get => _fameGoal.GetValue(); set => _fameGoal.SetValue(value); }
        public int Glow { get => _glow.GetValue(); set => _glow.SetValue(value); }
        public string Guild { get => _guild.GetValue(); set => _guild?.SetValue(value); }
        public int? GuildInvite { get; set; }
        public int GuildRank { get => _guildRank.GetValue(); set => _guildRank.SetValue(value); }
        public bool HasBackpack { get => _hasBackpack.GetValue(); set => _hasBackpack.SetValue(value); }
        public ItemStacker HealthPots { get; private set; }
        public Inventory Inventory { get; private set; }
        public bool IsInvulnerable => HasConditionEffect(ConditionEffects.Paused) || HasConditionEffect(ConditionEffects.Stasis) || HasConditionEffect(ConditionEffects.Invincible) || HasConditionEffect(ConditionEffects.Invulnerable);
        public int LDBoostTime { get; set; }
        public int Level { get => _level.GetValue(); set => _level.SetValue(value); }
        public ItemStacker MagicPots { get; private set; }
        
        public int MP { get => _mp.GetValue(); set => _mp.SetValue(value); }
        public bool Muted { get; set; }
        public bool NameChosen { get => _nameChosen.GetValue(); set => _nameChosen.SetValue(value); }
        public int OxygenBar { get => _oxygenBar.GetValue(); set => _oxygenBar.SetValue(value); }
        public ConcurrentQueue<InboundBuffer> IncomingMessages { get; private set; }
        public Pet Pet { get; set; }
        public int PetId { get; set; }
        public PlayerUpdate PlayerUpdate { get; private set; }
        public Position Pos => new Position() { X = X, Y = Y };
        public int Skin { get => _skin.GetValue(); set => _skin.SetValue(value); }
        public int[] SlotTypes { get; private set; }

        public ItemStacker[] Stacks { get; private set; }
        public int Stars { get => _stars.GetValue(); set => _stars.SetValue(value); }
        public int Texture1 { get => _texture1.GetValue(); set => _texture1.SetValue(value); }
        public int Texture2 { get => _texture2.GetValue(); set => _texture2.SetValue(value); }
        public bool UpgradeEnabled { get; set; }
        public bool XPBoosted { get => _xpBoosted.GetValue(); set => _xpBoosted.SetValue(value); }
        public int XPBoostTime { get; set; }

        private SV<int> _accountId;
        private SV<int> _admin;
        private SV<int> _baseStat;
        private double _breath;
        private int _canApplyEffect0;
        private int _canApplyEffect1;
        private int _canApplyEffect2;
        private int _canApplyEffect3;
        private SV<int> _colorchat;
        private SV<int> _colornamechat;
        private SV<int> _credits;
        private SV<int> _currentFame;
        private bool _dead;
        private SV<int> _experience;
        private SV<int> _experienceGoal;
        private SV<int> _fame;
        private SV<int> _fameGoal;
        private SV<int> _glow;
        private SV<string> _guild;
        private SV<int> _guildRank;
        private SV<bool> _hasBackpack;
        private SV<int> _level;
        private SV<int> _mp;
        private SV<bool> _nameChosen;
        private int _originalSkin;
        private SV<int> _oxygenBar;
        private SV<int> _partyId;
        private SV<int> _rank;
        private SV<int> _skin;

        private SV<int> _stars;
        private SV<int> _texture1;
        private SV<int> _texture2;
        private SV<bool> _xpBoosted;
        internal class APIRank{ public int accID; public int charID; }
        internal class APIResp{ [JsonProperty("rank")] public string charRank { get; set; }}

        public bool IsAdmin => Client.Rank.Rank >= RankingType.Admin;
        public bool IsSupporter1 => Client.Rank.Rank >= RankingType.Supporter1;
        public bool IsSupporter2 => Client.Rank.Rank >= RankingType.Supporter2;
        public bool IsSupporter3 => Client.Rank.Rank >= RankingType.Supporter3;
        public bool IsSupporter4 => Client.Rank.Rank >= RankingType.Supporter4;
        public bool IsSupporter5 => Client.Rank.Rank >= RankingType.Supporter5;

        public Player(Client client, bool saveInventory = true) : base(client.GameServer, client.Character.ObjectType)
        {
            var settings = GameServer.Resources.Settings;
            var gameData = GameServer.Resources.GameData;

            Client = client;

            CalculateRank();

            _accountId = new SV<int>(this, StatDataType.AccountId, client.Account.AccountId, true);
            _experience = new SV<int>(this, StatDataType.Experience, client.Character.Experience, true);
            _experienceGoal = new SV<int>(this, StatDataType.ExperienceGoal, 0, true);
            _level = new SV<int>(this, StatDataType.Level, client.Character.Level);
            _currentFame = new SV<int>(this, StatDataType.CurrentFame, client.Account.Fame, true);
            _fame = new SV<int>(this, StatDataType.Fame, client.Character.Fame, true);
            _fameGoal = new SV<int>(this, StatDataType.FameGoal, 0, true);
            _stars = new SV<int>(this, StatDataType.Stars, 0);
            _guild = new SV<string>(this, StatDataType.Guild, "");
            _guildRank = new SV<int>(this, StatDataType.GuildRank, -1);
            _rank = new SV<int>(this, StatDataType.Rank, (int)client.Rank.Rank); // we need to export this to client so dont remove
            _credits = new SV<int>(this, StatDataType.Credits, client.Account.Credits, true);
            _nameChosen = new SV<bool>(this, StatDataType.NameChosen, client.Account.NameChosen, false, v => Client.Account?.NameChosen ?? v);
            _texture1 = new SV<int>(this, StatDataType.Texture1, client.Character.Tex1);
            _texture2 = new SV<int>(this, StatDataType.Texture2, client.Character.Tex2);
            _skin = new SV<int>(this, StatDataType.Skin, 0);
            _glow = new SV<int>(this, StatDataType.Glow, 0);
            _admin = new SV<int>(this, StatDataType.Admin, client.Rank.IsAdmin ? 1 : 0);
            _xpBoosted = new SV<bool>(this, StatDataType.XPBoost, client.Character.XPBoostTime != 0, true);
            _mp = new SV<int>(this, StatDataType.MP, client.Character.MP);
            _hasBackpack = new SV<bool>(this, StatDataType.HasBackpack, client.Character.HasBackpack, true);
            _oxygenBar = new SV<int>(this, StatDataType.OxygenBar, -1, true);
            _baseStat = new SV<int>(this, StatDataType.BaseStat, client.Account.SetBaseStat, true);
            
            _colornamechat = new SV<int>(this, StatDataType.ColorNameChat, 0);
            _colorchat = new SV<int>(this, StatDataType.ColorChat, 0);
            _partyId = new SV<int>(this, StatDataType.PartyId, client.Account.PartyId, true);

            _noManaBar = new SV<int>(this, StatDataType.NoManaBar, 0);

            var addition = 0;
            switch (client.Rank.Rank)
            {
                case RankingType.Supporter1:
                    addition = 10;
                    break;
                case RankingType.Supporter2:
                    addition = 20;
                    break;
                case RankingType.Supporter3:
                    addition = 30;
                    break;
                case RankingType.Supporter4:
                    addition = 40;
                    break;
                case RankingType.Supporter5:
                    addition = 50;
                    break;
            }
            var maxPotionAmount = 50 + addition;
            _SPSLifeCount = new SV<int>(this, StatDataType.SPS_LIFE_COUNT, client.Account.SPSLifeCount, true);
            _SPSLifeCountMax = new SV<int>(this, StatDataType.SPS_LIFE_COUNT_MAX, maxPotionAmount, true);
            _SPSManaCount = new SV<int>(this, StatDataType.SPS_MANA_COUNT, client.Account.SPSManaCount, true);
            _SPSManaCountMax = new SV<int>(this, StatDataType.SPS_MANA_COUNT_MAX, maxPotionAmount, true);
            _SPSDefenseCount = new SV<int>(this, StatDataType.SPS_DEFENSE_COUNT, client.Account.SPSDefenseCount, true);
            _SPSDefenseCountMax = new SV<int>(this, StatDataType.SPS_DEFENSE_COUNT_MAX, maxPotionAmount, true);
            _SPSAttackCount = new SV<int>(this, StatDataType.SPS_ATTACK_COUNT, client.Account.SPSAttackCount, true);
            _SPSAttackCountMax = new SV<int>(this, StatDataType.SPS_ATTACK_COUNT_MAX, maxPotionAmount, true);
            _SPSDexterityCount = new SV<int>(this, StatDataType.SPS_DEXTERITY_COUNT, client.Account.SPSDexterityCount, true);
            _SPSDexterityCountMax = new SV<int>(this, StatDataType.SPS_DEXTERITY_COUNT_MAX, maxPotionAmount, true);
            _SPSSpeedCount = new SV<int>(this, StatDataType.SPS_SPEED_COUNT, client.Account.SPSSpeedCount, true);
            _SPSSpeedCountMax = new SV<int>(this, StatDataType.SPS_SPEED_COUNT_MAX, maxPotionAmount, true);
            _SPSVitalityCount = new SV<int>(this, StatDataType.SPS_VITALITY_COUNT, client.Account.SPSVitalityCount, true);
            _SPSVitalityCountMax = new SV<int>(this, StatDataType.SPS_VITALITY_COUNT_MAX, maxPotionAmount, true);
            _SPSWisdomCount = new SV<int>(this, StatDataType.SPS_WISDOM_COUNT, client.Account.SPSWisdomCount, true);
            _SPSWisdomCountMax = new SV<int>(this, StatDataType.SPS_WISDOM_COUNT_MAX, maxPotionAmount, true);

            IncomingMessages = new ConcurrentQueue<InboundBuffer>();

            Name = client.Account.Name;
            HP = client.Character.HP;
            ConditionEffects = 0;

            XPBoostTime = client.Character.XPBoostTime;
            LDBoostTime = client.Character.LDBoostTime;

            var s = (ushort)client.Character.Skin;
            if (gameData.Skins.Keys.Contains(s))
            {
                SetDefaultSkin(s);
                SetDefaultSize(gameData.Skins[s].Size);
            }

            var guild = GameServer.Database.GetGuild(client.Account.GuildId);
            if (guild?.Name != null)
            {
                Guild = guild.Name;
                GuildRank = client.Account.GuildRank;
            }

            if (Client.Account.Size > 0)
                Size = Client.Account.Size;

            PetId = client.Character.PetId;

            HealthPots = new ItemStacker(this, 254, 0x0A22, count: client.Character.HealthStackCount, settings.MaxStackablePotions);
            MagicPots = new ItemStacker(this, 255, 0x0A23, count: client.Character.MagicStackCount, settings.MaxStackablePotions);
            Stacks = new ItemStacker[] { HealthPots, MagicPots };

            if (client.Character.Datas == null)
                client.Character.Datas = new ItemData[20];

            Inventory = new Inventory(this, Utils.ResizeArray(Client.Character.Items.Select(_ => (_ == 0xffff || !gameData.Items.ContainsKey(_)) ? null : gameData.Items[_]).ToArray(), 20), Client.Character.Datas);

            Inventory.InventoryChanged += (sender, e) => Stats.ReCalculateValues();
            SlotTypes = Utils.ResizeArray(gameData.Classes[ObjectType].SlotTypes, 20);
            Stats = new StatsManager(this);

            GameServer.Database.IsMuted(client.IpAddress).ContinueWith(t =>
            {
                Muted = !Client.Rank.IsAdmin && t.IsCompleted && t.Result;
            });

            GameServer.Database.IsLegend(AccountId).ContinueWith(t =>
            {
                Glow = t.Result && client.Account.GlowColor == 0 ? 0xFF0000 : client.Account.GlowColor;
            });

            LoadTalismanData();
        }

        private void CalculateRank()
        {
            if (Client.Rank.IsAdmin)
                return;

            var newAmountDonated = Client.Rank.NewAmountDonated; // add $10
            var amountDonated = Client.Rank.TotalAmountDonated;

            var rank = Client.Rank.Rank;
            while (newAmountDonated > 0)
            {
                amountDonated++;
                newAmountDonated--;

                if (rank == RankingType.Regular && amountDonated >= 10 && amountDonated < 20)
                {
                    rank = RankingType.Supporter1;
                    GameServer.Database.UpdateCredit(Client.Account, 750);
                }
                else if (rank == RankingType.Supporter1 && amountDonated >= 20 && amountDonated < 30)
                {
                    rank = RankingType.Supporter2;
                    GameServer.Database.UpdateCredit(Client.Account, 1500);
                }
                else if (rank == RankingType.Supporter2 && amountDonated >= 30 && amountDonated < 40)
                {
                    rank = RankingType.Supporter3;
                    GameServer.Database.UpdateCredit(Client.Account, 2625);
                }
                else if (rank == RankingType.Supporter3 && amountDonated >= 40 && amountDonated < 50)
                {
                    rank = RankingType.Supporter4;
                    GameServer.Database.UpdateCredit(Client.Account, 3750);
                }
                else if (rank == RankingType.Supporter4 && amountDonated < 50)
                {
                    rank = RankingType.Supporter5;
                    GameServer.Database.UpdateCredit(Client.Account, 5000);
                }
            }

            Client.Rank.TotalAmountDonated = amountDonated;
            Client.Rank.NewAmountDonated = newAmountDonated;
            Client.Rank.Rank = rank;
            Client.Rank.Flush();
        }

        public bool ApplyEffectCooldown(int slot)
        {
            if (slot == 0)
                if (_canApplyEffect0 > 0)
                    return false;
            if (slot == 1)
                if (_canApplyEffect1 > 0)
                    return false;
            if (slot == 2)
                if (_canApplyEffect2 > 0)
                    return false;
            if (slot == 3)
                if (_canApplyEffect3 > 0)
                    return false;
            return true;
        }

        public override bool CanBeSeenBy(Player player) => Client?.Account != null && Client.Account.Hidden ? false : true;

        public void Damage(int dmg, Entity src)
        {
            if (IsInvulnerable)
                return;

            dmg = (int)Stats.GetDefenseDamage(dmg, false);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            World.BroadcastIfVisibleExclude(new Damage()
            {
                TargetId = Id,
                Effects = 0,
                DamageAmount = (ushort)dmg,
                Kill = HP <= 0,
                BulletId = 0,
                ObjectId = src.Id
            }, this, this);

            if (HP <= 0)
                Death(src.ObjectDesc.DisplayId ??
                      src.ObjectDesc.ObjectId,
                      src);
        }

        public void Death(string killer, Entity entity = null, WmapTile tile = null, bool rekt = false)
        {
            if (Client.State == ProtocolState.Disconnected || _dead)
                return;

            _dead = true;

            if (tile != null && tile.Spawned)
                rekt = true;

            if (World is VaultWorld)
            {
                Rekted(true);
                return;
            }

            if (Rekted(rekt))
                return;

            if (NonPermaKillEnemy(entity, killer))
                return;

            if (Resurrection())
                return;

            if (TestWorld(killer))
                return;

            SaveToCharacter();
            GameServer.Database.Death(GameServer.Resources.GameData, Client.Account, Client.Character, FameCounter.Stats, killer);

            GenerateGravestone();
            AnnounceDeath(killer);

            Console.WriteLine($"Sending death packet {AccountId} {Client.Character.CharId} {killer}");
            Client.SendPacket(new Death()
            {
                AccountId = AccountId,
                CharId = Client.Character.CharId,
                KilledBy = killer
            });

            World.Timers.Add(new WorldTimer(1000, (w, t) =>
            {
                if (Client.Player != this)
                    return;
                Client.Disconnect("Death");
            }));
        }

        public int GetCurrency(CurrencyType currency)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    return Credits;

                case CurrencyType.Fame:
                    return CurrentFame;

                default:
                    return 0;
            }
        }

        public int GetMaxedStats()
        {
            var playerDesc = GameServer.Resources.GameData.Classes[ObjectType];
            return playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count() + (UpgradeEnabled ? playerDesc.Stats.Where((t, i) => i == 0 ? Stats.Base[i] >= t.MaxValue + 50 : i == 1 ? Stats.Base[i] >= t.MaxValue + 50 : Stats.Base[i] >= t.MaxValue + 10).Count() : 0);
        }

        public override bool HitByProjectile(Projectile projectile, TickTime time)
        {
            if (projectile.Host is Player || IsInvulnerable)
                return false;

            #region Item Effects

            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item == null || !item.Legendary && !item.Revenge && !item.Eternal && !item.Mythical)
                    continue;

                /* Eternal Effects */
                if (item.MonkeyKingsWrath)
                {
                    MonkeyKingsWrath(i);
                }
                /* Revenge Effects */
                if (item.GodTouch)
                {
                    GodTouch(i);
                }

                if (item.GodBless)
                {
                    GodBless(i);
                }

                /* Legendary Effects */
                if (item.Clarification)
                {
                    Clarification(i);
                }
            }

            #endregion Item Effects

            var dmg = (int)Stats.GetDefenseDamage(projectile.Damage, projectile.ProjDesc.ArmorPiercing);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            ApplyConditionEffect(projectile.ProjDesc.Effects);
            World.BroadcastIfVisibleExclude(new Damage()
            {
                TargetId = Id,
                Effects = HasConditionEffect(ConditionEffects.Invincible) ? 0 : projectile.ConditionEffects,
                DamageAmount = (ushort)dmg,
                Kill = HP <= 0,
                BulletId = projectile.ProjectileId,
                ObjectId = projectile.Host.Id
            }, this, this);

            if (HP <= 0)
                Death(projectile.Host.ObjectDesc.DisplayId ?? projectile.Host.ObjectDesc.ObjectId, projectile.Host);

            return base.HitByProjectile(projectile, time);
        }

        public override void Init(World owner)
        {
            base.Init(owner);

            var x = 0;
            var y = 0;
            var spawnRegions = owner.GetSpawnPoints();
            if (spawnRegions.Any())
            {
                var rand = new System.Random();
                var sRegion = spawnRegions.ElementAt(rand.Next(0, spawnRegions.Length));
                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }
            Move(x + 0.5f, y + 0.5f);

            // spawn pet if player has one attached
            SpawnPetIfAttached(owner);

            FameCounter = new FameCounter(this);
            FameGoal = GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
            ExperienceGoal = GetExpGoal(Client.Character.Level);
            Stars = GetStars();

            if (owner.IdName.Equals("Ocean Trench"))
                Breath = 100;

            SetNewbiePeriod();
            PlayerUpdate = new PlayerUpdate(this);
        }

        public void HandleIO(ref TickTime time)
        {
            while (IncomingMessages.TryDequeue(out var incomingMessage))
            {
                if (incomingMessage.Client.State == ProtocolState.Disconnected)
                    break;

                var handler = MessageHandlers.GetHandler(incomingMessage.MessageId);
                if (handler == null)
                {
                    incomingMessage.Client.PacketSpamAmount++;
                    if (incomingMessage.Client.PacketSpamAmount > 32)
                        incomingMessage.Client.Disconnect($"Packet Spam: {incomingMessage.Client.IpAddress}");
                    StaticLogger.Instance.Error($"Unknown MessageId: {incomingMessage.MessageId} - {Client.IpAddress}");
                    continue;
                }

                try
                {
                    NReader rdr = null;
                    if (incomingMessage.Payload.Length != 0)
                        rdr = new NReader(new MemoryStream(incomingMessage.Payload));
                    handler.Handle(incomingMessage.Client, rdr, ref time);
                    rdr?.Dispose();
                }
                catch (Exception ex)
                {
                    if (!(ex is EndOfStreamException))
                        StaticLogger.Instance.Error($"Error processing packet ({((incomingMessage.Client.Account != null) ? incomingMessage.Client.Account.Name : "")}, {incomingMessage.Client.IpAddress})\n{ex}");
                    incomingMessage.Client.SendFailure("An error occurred while processing data from your client.", FailureMessage.MessageWithDisconnect);
                }
            }
        }

        public void Reconnect(World world)
        {
            if (world == null)
                SendError("Portal Not Implemented!");
            else
            {
                Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = GameServer.Configuration.serverInfo.port,
                    GameId = world.Id,
                    Name = world.IdName
                });
                var party = DbPartySystem.Get(Client.Account.Database, Client.Account.PartyId);
                if (party != null && party.PartyLeader.Item1 == Client.Account.Name && party.PartyLeader.Item2 == Client.Account.AccountId)
                    party.WorldId = -1;
            }
        }

        public void RestoreDefaultSkin() => Skin = _originalSkin;

        public void SaveToCharacter()
        {
            if (Client == null || Client.Character == null) 
                return;

            var chr = Client.Character;
            chr.Level = Level;
            chr.Experience = Experience;
            chr.Fame = Fame;
            chr.HP = HP <= 0 ? 1 : HP;
            chr.MP = MP;
            chr.Stats = Stats.Base.GetStats();
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Skin = _originalSkin;
            chr.FameStats = FameCounter?.Stats?.Write() ?? chr.FameStats;
            chr.LastSeen = DateTime.Now;
            chr.HealthStackCount = HealthPots.Count;
            chr.MagicStackCount = MagicPots.Count;
            chr.HasBackpack = HasBackpack;
            chr.PetId = PetId;
            chr.Items = Inventory.GetItemTypes();
            chr.XPBoostTime = XPBoostTime;
            chr.LDBoostTime = LDBoostTime;
            chr.UpgradeEnabled = UpgradeEnabled;
            chr.Datas = Inventory.Data.GetDatas();

            Client.Account.TotalFame = Client.Account.Fame;
            Stats.ReCalculateValues();
        }

        public void SetDefaultSkin(int skin)
        {
            _originalSkin = skin;
            Skin = skin;
        }

        public void Teleport(TickTime time, int objId, bool ignoreRestrictions = false)
        {
            if (IsInMarket && (World is NexusWorld))
            {
                SendError("You cannot teleport while inside the market.");
                RestartTPPeriod();
                return;
            }

            var obj = World.GetEntity(objId);
            if (obj == null)
            {
                SendError("Target does not exist.");
                RestartTPPeriod();
                return;
            }

            if (!ignoreRestrictions)
            {
                if (Id == objId)
                {
                    SendInfo("You are already at yourself, and always will be!");
                    return;
                }

                if (!World.AllowTeleport && !IsAdmin)
                {
                    SendError("Cannot teleport here.");
                    RestartTPPeriod();
                    return;
                }

                if (HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport while paused.");
                    RestartTPPeriod();
                    return;
                }

                if (!(obj is Player))
                {
                    SendError("Can only teleport to players.");
                    RestartTPPeriod();
                    return;
                }

                if (obj.HasConditionEffect(ConditionEffects.Invisible))
                {
                    SendError("Cannot teleport to an invisible player.");
                    RestartTPPeriod();
                    return;
                }

                if (obj.HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport to a paused player.");
                    RestartTPPeriod();
                    return;
                }

                if (obj.HasConditionEffect(ConditionEffects.Hidden))
                {
                    SendError("Target does not exist.");
                    RestartTPPeriod();
                    return;
                }
            }

            ApplyConditionEffect(ConditionEffectIndex.Invincible, 2000);
            ApplyConditionEffect(ConditionEffectIndex.Stunned, 1000);
            TeleportPosition(time, obj.X, obj.Y, ignoreRestrictions);
        }

        public void TeleportPosition(TickTime time, float x, float y, bool ignoreRestrictions = false) => TeleportPosition(time, new Position() { X = x, Y = y }, ignoreRestrictions);

        public void TeleportPosition(TickTime time, Position position, bool ignoreRestrictions = false)
        {
            if (!ignoreRestrictions)
            {
                if (!TPCooledDown())
                {
                    SendError("Too soon to teleport again!");
                    return;
                }

                SetTPDisabledPeriod();
                SetNewbiePeriod();
                FameCounter.Teleport();
            }

            var id = Id;
            var tpPkts = new OutgoingMessage[]
            {
                new Goto()
                {
                    ObjectId = id,
                    Pos = position
                },
                new ShowEffect()
                {
                    EffectType = EffectType.Teleport,
                    TargetObjectId = id,
                    Pos1 = position,
                    Color = new ARGB(0xFFFFFFFF)
                }
            };

            World.ForeachPlayer(_ =>
            {
                _.AwaitGotoAck(time.TotalElapsedMs);
                if (_ == null || _.Client == null)
                {
                    var nullPlayer = _ == null;
                    var nullClient = _?.Client ?? null;
                    Console.WriteLine($"Error NULL | Player: {nullPlayer} | Client: {nullClient}");
                }
                else
                {
                    _.Client.SendPackets(tpPkts);
                }
            });

            PlayerUpdate.UpdateTiles = true;
        }

        public override void Tick(ref TickTime time)
        {
            if (KeepAlive(time))
            {
                PlayerUpdate.SendUpdate();
                PlayerUpdate.SendNewTick(time.ElaspedMsDelta);

                HandleTalismans(ref time);

                HandleBreath(ref time);

                if(World is NexusWorld)
                    HandleNexus(ref time);

                CheckTradeTimeout(time);
                HandleQuest(time);

                HandleEffects(ref time);
                HandleRegen(ref time);

                GroundEffect(time);
                TickActivateEffects(time);

                /* Item Effects */
                TimeEffects(time);
                SpecialEffects();

                FameCounter.Tick(time);

                CerberusClaws(time);
                CerberusCore(time);
            }
            base.Tick(ref time);
        }

        public bool IsInMarket { get; private set; }

        public void HandleNexus(ref TickTime time)
        {
            var inMarket = (World as NexusWorld).WithinBoundsOfMarket(X, Y);
            if (inMarket && !IsInMarket)
                SendInfo("You have entered the market");
            if (!inMarket && IsInMarket)
            {
                SendInfo("You have left the market");
                
                if(tradeTarget != null)
                    CancelTrade(true);
            }
            IsInMarket = inMarket;
        }

        public void HandleBreath(ref TickTime time)
        {
            if (World.IdName != "Ocean Trench")
                return;
            if (Breath > 0)
                Breath -= 5 * time.DeltaTime * 5;
            else
                HP -= (int)(5 * time.DeltaTime * 5);

            if (HP < 0)
            {
                Death("Suffocation");
                return;
            }
        }

        internal void setCooldownTime(int time, int slot)
        {
            if (slot == 0)
                _canApplyEffect0 = time * 1000;
            if (slot == 1)
                _canApplyEffect1 = time * 1000;
            if (slot == 2)
                _canApplyEffect2 = time * 1000;
            if (slot == 3)
                _canApplyEffect3 = time * 1000;
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            base.ExportStats(stats, isOtherPlayer);
            if (!isOtherPlayer)
            {
                ExportSelf(stats);
                ExportOther(stats);
                return;
            }
            ExportOther(stats);
        }

        private void ExportSelf(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatDataType.Inventory5] = Inventory[5]?.ObjectType ?? -1;
            stats[StatDataType.Inventory6] = Inventory[6]?.ObjectType ?? -1;
            stats[StatDataType.Inventory7] = Inventory[7]?.ObjectType ?? -1;
            stats[StatDataType.Inventory8] = Inventory[8]?.ObjectType ?? -1;
            stats[StatDataType.Inventory9] = Inventory[9]?.ObjectType ?? -1;
            stats[StatDataType.Inventory10] = Inventory[10]?.ObjectType ?? -1;
            stats[StatDataType.Inventory11] = Inventory[11]?.ObjectType ?? -1;
            stats[StatDataType.BackPack0] = Inventory[12]?.ObjectType ?? -1;
            stats[StatDataType.BackPack1] = Inventory[13]?.ObjectType ?? -1;
            stats[StatDataType.BackPack2] = Inventory[14]?.ObjectType ?? -1;
            stats[StatDataType.BackPack3] = Inventory[15]?.ObjectType ?? -1;
            stats[StatDataType.BackPack4] = Inventory[16]?.ObjectType ?? -1;
            stats[StatDataType.BackPack5] = Inventory[17]?.ObjectType ?? -1;
            stats[StatDataType.BackPack6] = Inventory[18]?.ObjectType ?? -1;
            stats[StatDataType.BackPack7] = Inventory[19]?.ObjectType ?? -1;
            stats[StatDataType.Attack] = Stats[2];
            stats[StatDataType.Defense] = Stats[3];
            stats[StatDataType.Speed] = Stats[4];
            stats[StatDataType.Dexterity] = Stats[5];
            stats[StatDataType.Vitality] = Stats[6];
            stats[StatDataType.Wisdom] = Stats[7];
            stats[StatDataType.AttackBonus] = Stats.Boost[2];
            stats[StatDataType.DefenseBonus] = Stats.Boost[3];
            stats[StatDataType.SpeedBonus] = Stats.Boost[4];
            stats[StatDataType.DexterityBonus] = Stats.Boost[5];
            stats[StatDataType.VitalityBonus] = Stats.Boost[6];
            stats[StatDataType.WisdomBonus] = Stats.Boost[7];
            stats[StatDataType.HealthStackCount] = HealthPots.Count;
            stats[StatDataType.MagicStackCount] = MagicPots.Count;
            stats[StatDataType.HasBackpack] = HasBackpack ? 1 : 0;
            stats[StatDataType.LDBoostTime] = LDBoostTime / 1000;
            stats[StatDataType.XPBoost] = (XPBoostTime != 0) ? 1 : 0;
            stats[StatDataType.XPBoostTime] = XPBoostTime / 1000;
            stats[StatDataType.BaseStat] = Client?.Account?.SetBaseStat ?? 0;
            stats[StatDataType.InventoryData4] = Inventory.Data[4]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData5] = Inventory.Data[5]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData6] = Inventory.Data[6]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData7] = Inventory.Data[7]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData8] = Inventory.Data[8]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData9] = Inventory.Data[9]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData10] = Inventory.Data[10]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData11] = Inventory.Data[11]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData0] = Inventory.Data[12]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData1] = Inventory.Data[13]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData2] = Inventory.Data[14]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData3] = Inventory.Data[15]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData4] = Inventory.Data[16]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData5] = Inventory.Data[17]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData6] = Inventory.Data[18]?.GetData() ?? "{}";
            stats[StatDataType.BackPackData7] = Inventory.Data[19]?.GetData() ?? "{}";
            stats[StatDataType.Credits] = Credits;
            if (World is VaultWorld)
            {
                stats[StatDataType.SPS_LIFE_COUNT] = SPSLifeCount;
                stats[StatDataType.SPS_MANA_COUNT] = SPSManaCount;
                stats[StatDataType.SPS_ATTACK_COUNT] = SPSAttackCount;
                stats[StatDataType.SPS_DEFENSE_COUNT] = SPSDefenseCount;
                stats[StatDataType.SPS_DEXTERITY_COUNT] = SPSDexterityCount;
                stats[StatDataType.SPS_WISDOM_COUNT] = SPSWisdomCount;
                stats[StatDataType.SPS_SPEED_COUNT] = SPSSpeedCount;
                stats[StatDataType.SPS_VITALITY_COUNT] = SPSVitalityCount;
                stats[StatDataType.SPS_LIFE_COUNT_MAX] = SPSLifeCountMax;
                stats[StatDataType.SPS_MANA_COUNT_MAX] = SPSManaCountMax;
                stats[StatDataType.SPS_ATTACK_COUNT_MAX] = SPSAttackCountMax;
                stats[StatDataType.SPS_DEFENSE_COUNT_MAX] = SPSDefenseCountMax;
                stats[StatDataType.SPS_DEXTERITY_COUNT_MAX] = SPSDexterityCountMax;
                stats[StatDataType.SPS_WISDOM_COUNT_MAX] = SPSWisdomCountMax;
                stats[StatDataType.SPS_SPEED_COUNT_MAX] = SPSSpeedCountMax;
                stats[StatDataType.SPS_VITALITY_COUNT_MAX] = SPSVitalityCountMax;
            }
        }
        // minimal export for other players
        // things we wont see or need to know dont get exported
        private void ExportOther(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.AccountId] = AccountId;
            stats[StatDataType.Experience] = Experience - GetLevelExp(Level);
            stats[StatDataType.ExperienceGoal] = ExperienceGoal;
            stats[StatDataType.Level] = Level;
            stats[StatDataType.CurrentFame] = CurrentFame;
            stats[StatDataType.Fame] = Fame;
            stats[StatDataType.FameGoal] = FameGoal;
            stats[StatDataType.Stars] = Stars;
            stats[StatDataType.Guild] = Guild;
            stats[StatDataType.GuildRank] = GuildRank;
            stats[StatDataType.NameChosen] = (Client.Account?.NameChosen ?? NameChosen) ? 1 : 0;
            stats[StatDataType.Texture1] = Texture1;
            stats[StatDataType.Texture2] = Texture2;
            stats[StatDataType.Skin] = Skin;
            stats[StatDataType.Glow] = Glow;
            stats[StatDataType.MP] = MP;
            stats[StatDataType.Inventory0] = Inventory[0]?.ObjectType ?? -1;
            stats[StatDataType.Inventory1] = Inventory[1]?.ObjectType ?? -1;
            stats[StatDataType.Inventory2] = Inventory[2]?.ObjectType ?? -1;
            stats[StatDataType.Inventory3] = Inventory[3]?.ObjectType ?? -1;
            stats[StatDataType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatDataType.MaximumHP] = Stats[0];
            stats[StatDataType.MaximumMP] = Stats[1];
            stats[StatDataType.HPBoost] = Stats.Boost[0];
            stats[StatDataType.MPBoost] = Stats.Boost[1];
            stats[StatDataType.OxygenBar] = OxygenBar;
            stats[StatDataType.ColorNameChat] = ColorNameChat;
            stats[StatDataType.ColorChat] = ColorChat;
            stats[StatDataType.PartyId] = Client.Account.PartyId;
            stats[StatDataType.InventoryData0] = Inventory.Data[0]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData1] = Inventory.Data[1]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData2] = Inventory.Data[2]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData3] = Inventory.Data[3]?.GetData() ?? "{}";
        }

        private void CerberusClaws(TickTime time)
        {
            var elasped = time.TotalElapsedMs;
            if (elasped % 2000 == 0)
                Stats.Boost.ReCalculateValues();
        }

        private void CerberusCore(TickTime time)
        {
            var elasped = time.TotalElapsedMs;
            if (elasped % 15000 == 0)
                ApplyConditionEffect(ConditionEffectIndex.Berserk, 5000);
        }
        private string CheckRankAPI(int accID, int charID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://tkr.gg/api/getRank");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new APIRank()
                {
                    accID = accID,
                    charID = charID
                });
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<APIResp>(result);
                    if (!result.Contains("200")) //200 is normal result, if it doesn't contains it, somethingb bad happened
                        Console.WriteLine(result);
                    return data.charRank;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "n/a";
            }
        }

        private void AnnounceDeath(string killer)
        {
            //var charRank = CheckRankAPI(Client.Player.AccountId, Client.Character.CharId);
            var maxed = GetMaxedStats();
            var deathMessage = Name + " (" + maxed + (UpgradeEnabled ? "/16, " : "/8, ") + Client.Character.Fame + ") has been killed by " + killer + "! ";
//            if (maxed >= 6 && Rank <= 60)
//            {
//                var deathNote = "They were ranked #" + charRank + " on the alive character leaderboards.";
//                deathMessage += deathNote;
//                try
//                {
//                    var discord = World.GameServer.Configuration.discordIntegration;
//                    var players = World.Players.Count(p => p.Value.Client != null);
//                    var builder = discord.MakeDeathAnnounce(
//                        World.GameServer.Configuration.serverInfo,
//                        World.IsRealm ? World.DisplayName : World.IdName,
//                        players,
//                        World.MaxPlayers,
//                        World.InstanceType == WorldResourceInstanceType.Dungeon,
//                        discord.ripIco,
//                        Client.Character.CharId,
//                        Name,
//                        Rank,
//                        Stars,
//                        ObjectDesc.ObjectId,
//                        Level,
//                        Fame,
//                        UpgradeEnabled,
//                        maxed,
//                        killer,
//                        charRank
//                    );
//#pragma warning disable
//                    discord.SendWebhook(discord.webhookDeathEvent, builder.Value);
//#pragma warning restore
//                }catch(Exception ex)
//                {
//                    Console.WriteLine($"[Death] Discord Intergration error");
//                }
//            }

            if ((maxed >= 6 || Fame >= 1000) && !IsAdmin)
            {
                var worlds = GameServer.WorldManager.GetWorlds();
                foreach(var world in worlds)
                    world.ForeachPlayer(_ => _.DeathNotif(deathMessage));
                return;
            }

            var pGuild = Client.Account.GuildId;

            // guild case, only for level 20
            if (pGuild > 0 && Level == 20)
            {
                var worlds = GameServer.WorldManager.GetWorlds();
                foreach (var world in worlds)
                    world.ForeachPlayer(_ =>
                    {
                        if (_.Client.Account.GuildId == pGuild)
                            _.DeathNotif(deathMessage);
                    });
                World.ForeachPlayer(_ =>
                {
                    if (_.Client.Account.GuildId != pGuild)
                        _.DeathNotif(deathMessage);
                });
            }
            else
                // guild less case
                World.ForeachPlayer(_ => _.DeathNotif(deathMessage));
        }

        private void Clarification(int slot)
        {
            if (World.Random.NextDouble() < 0.1 && ApplyEffectCooldown(slot))
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xff00A6FF),
                    Pos1 = new Position() { X = 3 }
                }, this);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "Clarification!",
                    Color = new ARGB(0xFF00A6FF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this);

                ActivateHealMp(this, 30 * Stats[1] / 100);
                setCooldownTime(15, slot);
            }
        }

        private void EternalEffects(Item item, int slot)
        {
            if (item.MonkeyKingsWrath)
            {
                if (World.Random.NextDouble() < .5 && ApplyEffectCooldown(slot))// 50 % chance
                {
                    Size = 100;
                    setCooldownTime(10, slot);
                    World.BroadcastIfVisible(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xFF98ff98),
                        Pos1 = new Position() { X = 3 }
                    }, this);

                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Monkey King's Wrath!",
                        Color = new ARGB(0xFF98ff98),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);
                    //TO BE DECIDED
                    Size = 300;
                }
            }
        }

        public void CleanupPlayerUpdate()
        {
            //Inventory = null; todo figure out how to dispose better
            PlayerUpdate?.Dispose();
            PlayerUpdate = null;
        }

        private void GenerateGravestone(bool phantomDeath = false)
        {
            var playerDesc = GameServer.Resources.GameData.Classes[ObjectType];
            //var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count();
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count() + (UpgradeEnabled ? playerDesc.Stats.Where((t, i) => i == 0 ? Stats.Base[i] >= t.MaxValue + 50 : i == 1 ? Stats.Base[i] >= t.MaxValue + 50 : Stats.Base[i] >= t.MaxValue + 10).Count() : 0);
            ushort objType;
            int time;
            switch (maxed)
            {
                case 16: objType = 0xa00e; time = 600000; break;
                case 15: objType = 0xa00d; time = 600000; break;
                case 14: objType = 0xa00c; time = 600000; break;
                case 13: objType = 0xa00b; time = 600000; break;
                case 12: objType = 0xa00a; time = 600000; break;
                case 11: objType = 0xa009; time = 600000; break;
                case 10: objType = 0xa008; time = 600000; break;
                case 9: objType = 0xa007; time = 600000; break;
                case 8: objType = 0x0735; time = 600000; break;
                case 7: objType = 0x0734; time = 600000; break;
                case 6: objType = 0x072b; time = 600000; break;
                case 5: objType = 0x072a; time = 600000; break;
                case 4: objType = 0x0729; time = 600000; break;
                case 3: objType = 0x0728; time = 600000; break;
                case 2: objType = 0x0727; time = 600000; break;
                case 1: objType = 0x0726; time = 600000; break;
                default:
                    objType = 0x0725; time = 300000;
                    if (Level < 20) { objType = 0x0724; time = 60000; }
                    if (Level <= 1) { objType = 0x0723; time = 30000; }
                    break;
            }

            var deathMessage = Name + " (" + maxed + (UpgradeEnabled ? "/16, " : "/8, ") + Client.Character.Fame + ")";
            //var deathMessage = Name + " (" + maxed + ("/8, ") + _client.Character.Fame + ")";
            var obj = new StaticObject(GameServer, objType, time, true, true, false);
            obj.Move(X, Y);
            obj.Name = (!phantomDeath) ? deathMessage : $"{Name} got rekt";
            World.EnterWorld(obj);
        }

        private void GodBless(int slot)
        {
            if (World.Random.NextDouble() < 0.03 && ApplyEffectCooldown(slot))
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffA1A1A1),
                    Pos1 = new Position() { X = 3 }
                }, this);
                World.BroadcastIfVisible(new Notification()
                {
                    Message = "God Bless!",
                    Color = new ARGB(0xFFFFFFFF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this);

                ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
                setCooldownTime(5, slot);
            }
        }

        private void GodTouch(int slot)
        {
            if (World.Random.NextDouble() < 0.02 && ApplyEffectCooldown(slot))
            {
                ActivateHealHp(this, 25 * Stats[0] / 100);
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 3 }
                }, this);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "God Touch!",
                    Color = new ARGB(0xFFFFFFFF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this);
                setCooldownTime(30, slot);
            }
        }

        private double HealthRegenCarry;
        private double ManaRegenCarry;

        private void HandleRegen(ref TickTime time)
        {
            var maxHP = Stats[0];

            if(CanHpRegen() && HP < maxHP)
            {
                var vitalityStat = Stats[6];

                HealthRegenCarry += (1 + 0.24 * vitalityStat);
                if(TalismanExtraLifeRegen > 0.0f)
                    HealthRegenCarry += (HealthRegenCarry * TalismanExtraLifeRegen);
                if(TalismanHealthHPRegen > 0.0f)
                    HealthRegenCarry += (HealthRegenCarry * TalismanHealthHPRegen);
                HealthRegenCarry *= time.DeltaTime;

                if (HasConditionEffect(ConditionEffects.Healing))
                    HealthRegenCarry += 20.0 * time.DeltaTime;

                var regen = (int)HealthRegenCarry;
                if (regen > 0)
                {
                    HP = Math.Min(HP + regen, maxHP);
                    HealthRegenCarry -= regen;
                }
            }

            var maxMP = Stats[1];
            if (CanMpRegen() && MP < maxMP)
            {
                var wisdomStat = Stats[7];

                ManaRegenCarry += (0.5 + 0.12 * wisdomStat);
                if(TalismanExtraManaRegen > 0.0f)
                    ManaRegenCarry += (ManaRegenCarry * TalismanExtraManaRegen);
                ManaRegenCarry *= time.DeltaTime;

                if (HasConditionEffect(ConditionEffects.MPTRegeneration))
                    HealthRegenCarry += 20.0 * time.DeltaTime;

                var regen = (int)ManaRegenCarry;
                if (regen > 0)
                {
                    MP = Math.Min(MP + regen, maxMP);
                    ManaRegenCarry -= regen;
                }
            }
        }

        private void LegendaryEffects(Item item, int slot)
        {
            var Slot = slot;

            if (item.OutOfOneMind)
            {
                if (World.Random.NextDouble() < 0.02 && ApplyEffectCooldown(Slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Out of One's Mind!",
                        Color = new ARGB(0xFF00D5D8),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);

                    ApplyConditionEffect(ConditionEffectIndex.Berserk, 3000);
                    setCooldownTime(10, Slot);
                }
            }

            if (item.SteamRoller)
            {
                if (World.Random.NextDouble() < 0.05 && ApplyEffectCooldown(Slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Steam Roller!",
                        Color = new ARGB(0xFF717171),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);

                    ApplyConditionEffect(ConditionEffectIndex.Armored, 5000);
                    setCooldownTime(10, Slot);
                }
            }

            if (item.Mutilate)
            {
                if (World.Random.NextDouble() < 0.08 && ApplyEffectCooldown(Slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Mutilate!",
                        Color = new ARGB(0xFFFF4600),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);

                    ApplyConditionEffect(ConditionEffectIndex.Damaging, 3000);
                    setCooldownTime(10, Slot);
                }
            }
        }

        private void MonkeyKingsWrath(int slot)
        {
            if (World.Random.NextDouble() < .5 && ApplyEffectCooldown(slot))// 50 % chance
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffff0000),
                    Pos1 = new Position() { X = 3 }
                }, this);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "Monkey King's Wrath!",
                    Color = new ARGB(0xFFFF0000),
                    PlayerId = Id,
                    ObjectId = Id
                }, this);
                //TO BE DECIDED
                this.Client.SendPacket(new GlobalNotification() { Text = "monkeyKing" });
                setCooldownTime(10, slot);
            }
        }

        private bool NonPermaKillEnemy(Entity entity, string killer)
        {
            if (entity == null)
                return false;

            if (!entity.Spawned && entity.Controller == null)
                return false;

            GenerateGravestone(true);
            ReconnectToNexus();
            return true;
        }

        private void ReconnectToNexus()
        {
            HP = 1;
            Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = GameServer.Configuration.serverInfo.port,
                GameId = World.NEXUS_ID,
                Name = "Nexus"
            });
            var party = DbPartySystem.Get(Client.Account.Database, Client.Account.PartyId);
            if (party != null && party.PartyLeader.Item1 == Client.Account.Name && party.PartyLeader.Item2 == Client.Account.AccountId)
                party.WorldId = -1;
        }

        private bool Rekted(bool rekt)
        {
            if (!rekt)
                return false;
            GenerateGravestone(true);
            ReconnectToNexus();
            return true;
        }

        private bool Resurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                var item = Inventory[i];

                if (item == null || !item.Resurrects)
                    continue;

                Inventory[i] = null;
                World.ForeachPlayer(_ => _.SendInfo($"{Name}'s {item.DisplayName} breaks and he disappears"));
                ReconnectToNexus();
                return true;
            }
            return false;
        }

        private void RevengeEffects(Item item, int slot)
        {
            if (item.Lucky)
            {
                if (World.Random.NextDouble() < 0.1 && ApplyEffectCooldown(slot))
                {
                    setCooldownTime(20, slot);
                    for (var j = 0; j < 8; j++)
                        Stats.Boost.ActivateBoost[j].Push(j == 0 ? 100 : j == 1 ? 100 : 15, true);
                    Stats.ReCalculateValues();

                    #region Boosted Eff

                    ApplyConditionEffect(ConditionEffectIndex.HPBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.MPBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.AttBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.DefBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.DexBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.SpdBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.WisBoost, 5000);
                    ApplyConditionEffect(ConditionEffectIndex.VitBoost, 5000);

                    #endregion Boosted Eff

                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Boosted!",
                        Color = new ARGB(0xFF00FF00),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);

                    World.Timers.Add(new WorldTimer(5000, (world, t) =>
                    {
                        for (var i = 0; i < 8; i++)
                            Stats.Boost.ActivateBoost[i].Pop(i == 0 ? 100 : i == 1 ? 100 : 15, true);
                        Stats.ReCalculateValues();
                    }));
                }
            }

            if (item.Insanity)
            {
                if (World.Random.NextDouble() < 0.05 && ApplyEffectCooldown(slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Insanity!",
                        Color = new ARGB(0xFFFF0000),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);

                    setCooldownTime(10, slot);
                    ApplyConditionEffect(ConditionEffectIndex.Berserk, 3000);
                    ApplyConditionEffect(ConditionEffectIndex.Damaging, 3000);
                }
            }

            if (item.HolyProtection)
            {
                if (World.Random.NextDouble() < 0.1 && ApplyEffectCooldown(slot))
                {
                    if (!(HasConditionEffect(ConditionEffects.Quiet)
                        || HasConditionEffect(ConditionEffects.Weak)
                        || HasConditionEffect(ConditionEffects.Slowed)
                        || HasConditionEffect(ConditionEffects.Sick)
                        || HasConditionEffect(ConditionEffects.Dazed)
                        || HasConditionEffect(ConditionEffects.Stunned)
                        || HasConditionEffect(ConditionEffects.Blind)
                        || HasConditionEffect(ConditionEffects.Hallucinating)
                        || HasConditionEffect(ConditionEffects.Drunk)
                        || HasConditionEffect(ConditionEffects.Confused)
                        || HasConditionEffect(ConditionEffects.Paralyzed)
                        || HasConditionEffect(ConditionEffects.Bleeding)
                        || HasConditionEffect(ConditionEffects.Hexed)
                        || HasConditionEffect(ConditionEffects.Unstable)
                        || HasConditionEffect(ConditionEffects.Curse)
                        || HasConditionEffect(ConditionEffects.Petrify)
                        || HasConditionEffect(ConditionEffects.Darkness)))
                        return;
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Holy Protection!",
                        Color = new ARGB(0xFFFFFFFF),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this);

                    setCooldownTime(7, slot);
                    ApplyConditionEffect(NegativeEffs);
                }
            }

            /* God Touch, God Bless in HitByProjectile */

            /* Electrify in HitByProjectile (Enemy) */
        }

        private void SonicBlaster(int slot)
        {
            if (ApplyEffectCooldown(slot))
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xff6F00C0),
                    Pos1 = new Position() { X = 3 }
                }, this);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "Sonic Blaster!",
                    Color = new ARGB(0xFF9300FF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this);

                ApplyConditionEffect(ConditionEffectIndex.Invisible, 6000);
                ApplyConditionEffect(ConditionEffectIndex.Speedy, 6000);
                setCooldownTime(30, slot);
            }
        }

        private void SpawnPetIfAttached(World owner)
        {
            // despawn old pet if found
            if (Pet != null)
                owner.LeaveWorld(Pet);

            if (Client.Account.Hidden)
                return;

            // create new pet
            var petId = PetId;
            if (petId != 0)
            {
                var pet = new Pet(GameServer, this, (ushort)petId);
                pet.Move(X, Y);
                owner.EnterWorld(pet);
                pet.SetDefaultSize(pet.ObjectDesc.Size);
                Pet = pet;
            }
        }

        private void SpecialEffects()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item == null || !item.Legendary && !item.Revenge && !item.Mythical)
                    continue;

                if (item.Mythical || item.Revenge || item.ObjectId == "Possessed Halberd" || item.ObjectId == "The Horn Breaker")
                    RevengeEffects(item, i);

                if (item.Legendary)
                    LegendaryEffects(item, i);

                if (item.Eternal)
                    EternalEffects(item, i);
            }
        }

        private bool TestWorld(string killer)
        {
            if (!(World is TestWorld))
                return false;
            GenerateGravestone();
            ReconnectToNexus();
            return true;
        }

        private void TickActivateEffects(TickTime time)
        {
            var dt = time.ElaspedMsDelta;
            if (World is VaultWorld || World is NexusWorld || World.InstanceType == WorldResourceInstanceType.Guild || World.Id == 10)
                return;
            
            if (XPBoostTime > 0)
            {    
                if (Level >= 20)
                    XPBoostTime = 0;

                XPBoostTime = Math.Max(XPBoostTime - dt, 0);
                if (XPBoostTime == 0)
                    XPBoosted = false;
            }

            if (LDBoostTime > 0)
                LDBoostTime = Math.Max(LDBoostTime - dt, 0);
        }

        private void TimeEffects(TickTime time)
        {
            if (_canApplyEffect0 > 0)
            {
                _canApplyEffect0 -= time.ElaspedMsDelta;
                if (_canApplyEffect0 < 0)
                    _canApplyEffect0 = 0;
            }

            if (_canApplyEffect1 > 0)
            {
                _canApplyEffect1 -= time.ElaspedMsDelta;
                if (_canApplyEffect1 < 0)
                    _canApplyEffect1 = 0;
            }

            if (_canApplyEffect2 > 0)
            {
                _canApplyEffect2 -= time.ElaspedMsDelta;
                if (_canApplyEffect2 < 0)
                    _canApplyEffect2 = 0;
            }

            if (_canApplyEffect3 > 0)
            {
                _canApplyEffect3 -= time.ElaspedMsDelta;
                if (_canApplyEffect3 < 0)
                    _canApplyEffect3 = 0;
            }
        }
    }
}
