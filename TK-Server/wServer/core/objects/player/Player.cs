using common;
using common.database;
using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public const int DONOR_1 = 10;
        public const int DONOR_2 = 20;
        public const int DONOR_3 = 30;
        public const int DONOR_4 = 40;
        public const int DONOR_5 = 50;
        public const int VIP = 60;

        public Client Client;

        public int AccountId { get => _accountId.GetValue(); set => _accountId.SetValue(value); }
        public int Admin { get => _admin.GetValue(); set => _admin.SetValue(value); }
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
        public int Points { get => _points.GetValue(); set => _points.SetValue(value); }
        public Position Pos => new Position() { X = X, Y = Y };
        public int Rank { get => _rank.GetValue(); set => _rank.SetValue(value); }
        public int Skin { get => _skin.GetValue(); set => _skin.SetValue(value); }
        public int[] SlotTypes { get; private set; }
        public int Node1TickMin { get => _node1TickMin.GetValue(); set => _node1TickMin.SetValue(value); }
        public int Node1TickMaj { get => _node1TickMaj.GetValue(); set => _node1TickMaj.SetValue(value); }
        public int Node1Med { get => _node1Med.GetValue(); set => _node1Med.SetValue(value); }
        public int Node1Big { get => _node1Big.GetValue(); set => _node1Big.SetValue(value); }
        public int Node2TickMin { get => _node2TickMin.GetValue(); set => _node2TickMin.SetValue(value); }
        public int Node2TickMaj { get => _node2TickMaj.GetValue(); set => _node2TickMaj.SetValue(value); }
        public int Node2Med { get => _node2Med.GetValue(); set => _node2Med.SetValue(value); }
        public int Node2Big { get => _node2Big.GetValue(); set => _node2Big.SetValue(value); }
        public int Node3TickMin { get => _node3TickMin.GetValue(); set => _node3TickMin.SetValue(value); }
        public int Node3TickMaj { get => _node3TickMaj.GetValue(); set => _node3TickMaj.SetValue(value); }
        public int Node3Med { get => _node3Med.GetValue(); set => _node3Med.SetValue(value); }
        public int Node3Big { get => _node3Big.GetValue(); set => _node3Big.SetValue(value); }
        public int Node4TickMin { get => _node4TickMin.GetValue(); set => _node4TickMin.SetValue(value); }
        public int Node4TickMaj { get => _node4TickMaj.GetValue(); set => _node4TickMaj.SetValue(value); }
        public int Node4Med { get => _node4Med.GetValue(); set => _node4Med.SetValue(value); }
        public int Node4Big { get => _node4Big.GetValue(); set => _node4Big.SetValue(value); }
        public int Node5TickMin { get => _node5TickMin.GetValue(); set => _node5TickMin.SetValue(value); }
        public int Node5TickMaj { get => _node5TickMaj.GetValue(); set => _node5TickMaj.SetValue(value); }
        public int Node5Med { get => _node5Med.GetValue(); set => _node5Med.SetValue(value); }
        public int Node5Big { get => _node5Big.GetValue(); set => _node5Big.SetValue(value); }

        public ItemStacker[] Stacks { get; private set; }
        public int Stars { get => _stars.GetValue(); set => _stars.SetValue(value); }
        public int Texture1 { get => _texture1.GetValue(); set => _texture1.SetValue(value); }
        public int Texture2 { get => _texture2.GetValue(); set => _texture2.SetValue(value); }
        public bool UpgradeEnabled { get => _upgradeEnabled.GetValue(); set => _upgradeEnabled.SetValue(value); }
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
        private float _hpRegenCounter;
        private SV<int> _level;
        private SV<int> _mp;
        private float _mpRegenCounter;
        private SV<bool> _nameChosen;
        private int _originalSkin;
        private SV<int> _oxygenBar;
        private SV<int> _partyId;
        private SV<int> _points;
        private SV<int> _rank;
        private SV<int> _skin;

        private SV<int> _node1TickMin;
        private SV<int> _node1TickMaj;
        private SV<int> _node1Med;
        private SV<int> _node1Big;
        private SV<int> _node2TickMin;
        private SV<int> _node2TickMaj;
        private SV<int> _node2Med;
        private SV<int> _node2Big;
        private SV<int> _node3TickMin;
        private SV<int> _node3TickMaj;
        private SV<int> _node3Med;
        private SV<int> _node3Big;
        private SV<int> _node4TickMin;
        private SV<int> _node4TickMaj;
        private SV<int> _node4Med;
        private SV<int> _node4Big;
        private SV<int> _node5TickMin;
        private SV<int> _node5TickMaj;
        private SV<int> _node5Med;
        private SV<int> _node5Big;

        private SV<int> _stars;
        private SV<int> _texture1;
        private SV<int> _texture2;
        private SV<bool> _upgradeEnabled;
        private SV<bool> _xpBoosted;

        public Player(Client client, bool saveInventory = true) : base(client.GameServer, client.Character.ObjectType)
        {
            var settings = GameServer.Resources.Settings;
            var gameData = GameServer.Resources.GameData;

            Client = client;

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
            _rank = new SV<int>(this, StatDataType.Rank, client.Account.Rank);
            _credits = new SV<int>(this, StatDataType.Credits, client.Account.Credits, true);
            _nameChosen = new SV<bool>(this, StatDataType.NameChosen, client.Account.NameChosen, false, v => Client.Account?.NameChosen ?? v);
            _texture1 = new SV<int>(this, StatDataType.Texture1, client.Character.Tex1);
            _texture2 = new SV<int>(this, StatDataType.Texture2, client.Character.Tex2);
            _skin = new SV<int>(this, StatDataType.Skin, 0);
            _glow = new SV<int>(this, StatDataType.Glow, 0);
            _admin = new SV<int>(this, StatDataType.Admin, client.Account.Admin ? 1 : 0);
            _xpBoosted = new SV<bool>(this, StatDataType.XPBoost, client.Character.XPBoostTime != 0, true);
            _mp = new SV<int>(this, StatDataType.MP, client.Character.MP);
            _hasBackpack = new SV<bool>(this, StatDataType.HasBackpack, client.Character.HasBackpack, true);
            _oxygenBar = new SV<int>(this, StatDataType.OxygenBar, -1, true);
            _baseStat = new SV<int>(this, StatDataType.BaseStat, client.Account.SetBaseStat, true);
            _points = new SV<int>(this, StatDataType.Points, client.Character.Points, true);
            _maxedLife = new SV<bool>(this, StatDataType.MaxedLife, client.Character.MaxedLife, true);
            _maxedMana = new SV<bool>(this, StatDataType.MaxedMana, client.Character.MaxedMana, true);
            _maxedAtt = new SV<bool>(this, StatDataType.MaxedAtt, client.Character.MaxedAtt, true);
            _maxedDef = new SV<bool>(this, StatDataType.MaxedDef, client.Character.MaxedDef, true);
            _maxedSpd = new SV<bool>(this, StatDataType.MaxedSpd, client.Character.MaxedSpd, true);
            _maxedDex = new SV<bool>(this, StatDataType.MaxedDex, client.Character.MaxedDex, true);
            _maxedVit = new SV<bool>(this, StatDataType.MaxedVit, client.Character.MaxedVit, true);
            _maxedWis = new SV<bool>(this, StatDataType.MaxedWis, client.Character.MaxedWis, true);
            _superMaxedLife = new SV<bool>(this, StatDataType.SuperMaxedLife, client.Character.SuperMaxedLife, true);
            _superMaxedMana = new SV<bool>(this, StatDataType.SuperMaxedMana, client.Character.SuperMaxedMana, true);
            _superMaxedAtt = new SV<bool>(this, StatDataType.SuperMaxedAtt, client.Character.SuperMaxedAtt, true);
            _superMaxedDef = new SV<bool>(this, StatDataType.SuperMaxedDef, client.Character.SuperMaxedDef, true);
            _superMaxedSpd = new SV<bool>(this, StatDataType.SuperMaxedSpd, client.Character.SuperMaxedSpd, true);
            _superMaxedDex = new SV<bool>(this, StatDataType.SuperMaxedDex, client.Character.SuperMaxedDex, true);
            _superMaxedVit = new SV<bool>(this, StatDataType.SuperMaxedVit, client.Character.SuperMaxedVit, true);
            _superMaxedWis = new SV<bool>(this, StatDataType.SuperMaxedWis, client.Character.SuperMaxedWis, true);

            _node1TickMin = new SV<int>(this, StatDataType.Node1TickMin, client.Character.Node1TickMin, true);
            _node1TickMaj = new SV<int>(this, StatDataType.Node1TickMaj, client.Character.Node1TickMaj, true);
            _node1Med = new SV<int>(this, StatDataType.Node1Med, client.Character.Node1Med, true);
            _node1Big = new SV<int>(this, StatDataType.Node1Big, client.Character.Node1Big, true);

            _node2TickMin = new SV<int>(this, StatDataType.Node2TickMin, client.Character.Node2TickMin, true);
            _node2TickMaj = new SV<int>(this, StatDataType.Node2TickMaj, client.Character.Node2TickMaj, true);
            _node2Med = new SV<int>(this, StatDataType.Node2Med, client.Character.Node2Med, true);
            _node2Big = new SV<int>(this, StatDataType.Node2Big, client.Character.Node2Big, true);

            _node3TickMin = new SV<int>(this, StatDataType.Node3TickMin, client.Character.Node3TickMin, true);
            _node3TickMaj = new SV<int>(this, StatDataType.Node3TickMaj, client.Character.Node3TickMaj, true);
            _node3Med = new SV<int>(this, StatDataType.Node3Med, client.Character.Node3Med, true);
            _node3Big = new SV<int>(this, StatDataType.Node3Big, client.Character.Node3Big, true);

            _node4TickMin = new SV<int>(this, StatDataType.Node4TickMin, client.Character.Node4TickMin, true);
            _node4TickMaj = new SV<int>(this, StatDataType.Node4TickMaj, client.Character.Node4TickMaj, true);
            _node4Med = new SV<int>(this, StatDataType.Node4Med, client.Character.Node4Med, true);
            _node4Big = new SV<int>(this, StatDataType.Node4Big, client.Character.Node4Big, true);

            _node5TickMin = new SV<int>(this, StatDataType.Node5TickMin, client.Character.Node5TickMin, true);
            _node5TickMaj = new SV<int>(this, StatDataType.Node5TickMaj, client.Character.Node5TickMaj, true);
            _node5Med = new SV<int>(this, StatDataType.Node5Med, client.Character.Node5Med, true);
            _node5Big = new SV<int>(this, StatDataType.Node5Big, client.Character.Node5Big, true);

            _colornamechat = new SV<int>(this, StatDataType.ColorNameChat, 0);
            _colorchat = new SV<int>(this, StatDataType.ColorChat, 0);
            _upgradeEnabled = new SV<bool>(this, StatDataType.UpgradeEnabled, client.Character.UpgradeEnabled, true);
            _partyId = new SV<int>(this, StatDataType.PartyId, client.Account.PartyId, true);

            var maxPotionAmount = 50 + Client.Account.Rank * 2;

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
                Muted = !Client.Account.Admin && t.IsCompleted && t.Result;
            });

            GameServer.Database.IsLegend(AccountId).ContinueWith(t =>
            {
                Glow = t.Result && client.Account.GlowColor == 0 ? 0xFF0000 : client.Account.GlowColor;
            });
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

        public void CheckMaxedStats()
        {
            var classes = GameServer?.Resources?.GameData?.Classes;

            if (classes == null)
                return;

            if (!classes.ContainsKey(ObjectType))
            {
                SLogger.Instance.Error($"There is no class for object type '{ObjectType}'.");
                return;
            }

            var desc = classes[ObjectType];

            if (desc == null)
            {
                SLogger.Instance.Error($"There is no player description for object type '{ObjectType}'.");
                return;
            }

            var statInfo = desc.Stats;
            var chr = Client.Character;

            if (Stats.Base[0] >= statInfo[0].MaxValue)
            {
                if (!MaxedLife)
                {
                    Points += 6;
                    MaxedLife = true;
                    chr.Points = Points;
                    chr.MaxedLife = MaxedLife;
                }
            }
            if (Stats.Base[1] >= statInfo[1].MaxValue)
            {
                if (!MaxedMana)
                {
                    Points += 5;
                    MaxedMana = true;
                    chr.Points = Points;
                    chr.MaxedMana = MaxedMana;
                }
            }
            if (Stats.Base[2] >= statInfo[2].MaxValue)
            {
                if (!MaxedAtt)
                {
                    Points += 3;
                    MaxedAtt = true;
                    chr.Points = Points;
                    chr.MaxedAtt = MaxedAtt;
                }
            }
            if (Stats.Base[3] >= statInfo[3].MaxValue)
            {
                if (!MaxedDef)
                {
                    Points += 3;
                    MaxedDef = true;
                    chr.Points = Points;
                    chr.MaxedDef = MaxedDef;
                }
            }
            if (Stats.Base[4] >= statInfo[4].MaxValue)
            {
                if (!MaxedSpd)
                {
                    Points += 3;
                    MaxedSpd = true;
                    chr.Points = Points;
                    chr.MaxedSpd = MaxedSpd;
                }
            }
            if (Stats.Base[5] >= statInfo[5].MaxValue)
            {
                if (!MaxedDex)
                {
                    Points += 3;
                    MaxedDex = true;
                    chr.Points = Points;
                    chr.MaxedDex = MaxedDex;
                }
            }
            if (Stats.Base[6] >= statInfo[6].MaxValue)
            {
                if (!MaxedVit)
                {
                    Points += 3;
                    MaxedVit = true;
                    chr.Points = Points;
                    chr.MaxedVit = MaxedVit;
                }
            }
            if (Stats.Base[7] >= statInfo[7].MaxValue)
            {
                if (!MaxedWis)
                {
                    Points += 3;
                    MaxedWis = true;
                    chr.Points = Points;
                    chr.MaxedWis = MaxedWis;
                }
            }

            var upgradeCount = 0;
            if (Stats.Base[0] >= statInfo[0].MaxValue + 50)
            {
                if (!SuperMaxedLife)
                {
                    Points += 5;
                    SuperMaxedLife = true;
                    chr.SuperMaxedLife = SuperMaxedLife;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[1] >= statInfo[1].MaxValue + 50)
            {
                if (!SuperMaxedMana)
                {
                    Points += 4;
                    SuperMaxedMana = true;
                    chr.SuperMaxedMana = SuperMaxedMana;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[2] >= statInfo[2].MaxValue + 10)
            {
                if (!SuperMaxedAtt)
                {
                    Points += 2;
                    SuperMaxedAtt = true;
                    chr.SuperMaxedAtt = SuperMaxedAtt;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[3] >= statInfo[3].MaxValue + 10)
            {
                if (!SuperMaxedDef)
                {
                    Points += 2;
                    SuperMaxedDef = true;
                    chr.SuperMaxedDef = SuperMaxedDef;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[4] >= statInfo[4].MaxValue + 10)
            {
                if (!SuperMaxedSpd)
                {
                    Points += 2;
                    SuperMaxedSpd = true;
                    chr.SuperMaxedSpd = SuperMaxedSpd;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[5] >= statInfo[5].MaxValue + 10)
            {
                if (!SuperMaxedDex)
                {
                    Points += 2;
                    SuperMaxedDex = true;
                    chr.SuperMaxedDex = SuperMaxedDex;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[6] >= statInfo[6].MaxValue + 10)
            {
                if (!SuperMaxedVit)
                {
                    Points += 2;
                    SuperMaxedVit = true;
                    chr.SuperMaxedVit = SuperMaxedVit;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            if (Stats.Base[7] >= statInfo[7].MaxValue + 10)
            {
                if (!SuperMaxedWis)
                {
                    Points += 2;
                    SuperMaxedWis = true;
                    chr.SuperMaxedWis = SuperMaxedWis;
                    chr.Points = Points;
                }
                upgradeCount++;
            }
            //we have 50 points + (1)56 Fame <<FIRST
            //Console.WriteLine("upgradeCount: " + upgradeCount);
            if (upgradeCount > 7)
            {
                Points = 50 + (Fame > 1449 ? 29 : Fame / 50);
                chr.Points = Points;
            }
        }

        public void checkSkillStats()
        {
            if (Node1TickMaj > 10)
                Node1TickMaj = 10;
            if (Node1Med > 3)
                Node1Med = 3;
            if (Node1Big > 2)
                Node1Big = 2;
            if (Node2TickMin > 5)
                Node2TickMin = 5;
            if (Node2TickMaj > 10)
                Node2TickMaj = 10;
            if (Node2Med > 3)
                Node2Med = 3;
            if (Node2Big > 2)
                Node2Big = 2;
            if (Node3TickMin > 5)
                Node3TickMin = 5;
            if (Node3TickMaj > 10)
                Node3TickMaj = 10;
            if (Node3Med > 3)
                Node3Med = 3;
            if (Node3Big > 2)
                Node3Big = 2;
            if (Node4TickMin > 5)
                Node4TickMin = 5;
            if (Node4TickMaj > 10)
                Node4TickMaj = 10;
            if (Node4Med > 3)
                Node4Med = 3;
            if (Node4Big > 2)
                Node4Big = 2;
            if (Node5TickMin > 5)
                Node5TickMin = 5;
            if (Node5TickMaj > 10)
                Node5TickMaj = 10;
            if (Node5Med > 3)
                Node5Med = 3;
            if (Node5Big > 2)
                Node5Big = 2;
        }

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
            }, this, this, PacketPriority.Low);

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

            Client.SendPacket(new Death()
            {
                AccountId = AccountId,
                CharId = Client.Character.CharId,
                KilledBy = killer
            }, PacketPriority.High);

            World.Timers.Add(new WorldTimer(200, (w, t) =>
            {
                if (Client.Player != this)
                    return;
                Client.Disconnect("Death");
            }));
        }

        public void DropNextRandom() => Client.Random.NextInt();

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
            if (projectile.ProjectileOwner is Player || IsInvulnerable)
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
                ObjectId = projectile.ProjectileOwner.Self.Id
            }, this, this, PacketPriority.Normal);

            if (HP <= 0)
                Death(projectile.ProjectileOwner.Self.ObjectDesc.DisplayId ?? projectile.ProjectileOwner.Self.ObjectDesc.ObjectId, projectile.ProjectileOwner.Self);

            return base.HitByProjectile(projectile, time);
        }

        public override void Init(World owner)
        {
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

            base.Init(owner);

            FameCounter = new FameCounter(this);
            PlayerUpdate = new PlayerUpdate(this);
        }

        public void HandleIO(ref TickTime time)
        {
            while (IncomingMessages.Count > 0)
            {
                if (!IncomingMessages.TryDequeue(out var incomingMessage))
                    continue;

                if (incomingMessage.Client.State == ProtocolState.Disconnected)
                    continue;

                var handler = MessageHandlers.GetHandler(incomingMessage.MessageId);
                if (handler == null)
                {
                    SLogger.Instance.Error($"Unknown MessageId: {incomingMessage.MessageId} - {Client.IpAddress}");
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
                        SLogger.Instance.Error("Error processing packet ({0}, {1}, {2})\n{3}", (incomingMessage.Client.Account != null) ? incomingMessage.Client.Account.Name : "", incomingMessage.Client.IpAddress, incomingMessage.Client.Id, ex);
                    incomingMessage.Client.SendFailure("An error occurred while processing data from your client.", Failure.MessageWithDisconnect);
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
            if (Client == null || Client.Character == null) return;

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

        public void SetCurrency(CurrencyType currency, int amount)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    Credits = amount; break;
                case CurrencyType.Fame:
                    CurrentFame = amount; break;
            }
        }

        public void SetDefaultSkin(int skin)
        {
            _originalSkin = skin;
            Skin = skin;
        }

        public void Teleport(TickTime time, int objId, bool ignoreRestrictions = false)
        {
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

                if (!World.AllowTeleport && Rank < 100)
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

            ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 1500);
            ApplyConditionEffect(ConditionEffectIndex.Stunned, 1500);
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
                _._tps += 1;
                _.Client.SendPackets(tpPkts, PacketPriority.Low);
            });
        }

        public override void Tick(TickTime time)
        {
            if (KeepAlive(time))
            {
                PlayerUpdate.SendUpdate();
                PlayerUpdate.SendNewTick(time.ElaspedMsDelta);

                if (World.IdName.Equals("Ocean Trench"))
                {
                    if (Breath > 0)
                        Breath -= 2 * time.DeltaTime * 5;
                    else
                        HP -= 5;

                    if (HP < 0)
                    {
                        Death("Suffocation");
                        return;
                    }
                }

                CheckTradeTimeout(time);
                //HandleSpecialEnemies(time);
                HandleQuest(time);

                if (!HasConditionEffect(ConditionEffects.Paused))
                {
                    HandleRegen(time);
                    HandleEffects(time);

                    GroundEffect(time);
                    TickActivateEffects(time);

                    /* Skill Tree */
                    checkSkillStats();
                    CheckMaxedStats();

                    /* Item Effects */
                    TimeEffects(time);
                    SpecialEffects();

                    FameCounter.Tick(time);

                    CerberusClaws(time);
                    CerberusCore(time);
                }
            }
            base.Tick(time);
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

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            base.ExportStats(stats);
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
            stats[StatDataType.Rank] = Rank;
            stats[StatDataType.Credits] = Credits;
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
            stats[StatDataType.MaximumHP] = Stats[0];
            stats[StatDataType.MaximumMP] = Stats[1];
            stats[StatDataType.Attack] = Stats[2];
            stats[StatDataType.Defense] = Stats[3];
            stats[StatDataType.Speed] = Stats[4];
            stats[StatDataType.Dexterity] = Stats[5];
            stats[StatDataType.Vitality] = Stats[6];
            stats[StatDataType.Wisdom] = Stats[7];
            stats[StatDataType.HPBoost] = Stats.Boost[0];
            stats[StatDataType.MPBoost] = Stats.Boost[1];
            stats[StatDataType.AttackBonus] = Stats.Boost[2];
            stats[StatDataType.DefenseBonus] = Stats.Boost[3];
            stats[StatDataType.SpeedBonus] = Stats.Boost[4];
            stats[StatDataType.DexterityBonus] = Stats.Boost[5];
            stats[StatDataType.VitalityBonus] = Stats.Boost[6];
            stats[StatDataType.WisdomBonus] = Stats.Boost[7];
            stats[StatDataType.HealthStackCount] = HealthPots.Count;
            stats[StatDataType.MagicStackCount] = MagicPots.Count;
            stats[StatDataType.HasBackpack] = HasBackpack ? 1 : 0;
            stats[StatDataType.OxygenBar] = OxygenBar;
            stats[StatDataType.LDBoostTime] = LDBoostTime / 1000;
            stats[StatDataType.XPBoost] = (XPBoostTime != 0) ? 1 : 0;
            stats[StatDataType.XPBoostTime] = XPBoostTime / 1000;
            stats[StatDataType.BaseStat] = Client?.Account?.SetBaseStat ?? 0;
            stats[StatDataType.Points] = Points;
            stats[StatDataType.MaxedLife] = MaxedLife;
            stats[StatDataType.MaxedMana] = MaxedMana;
            stats[StatDataType.MaxedAtt] = MaxedAtt;
            stats[StatDataType.MaxedDef] = MaxedDef;
            stats[StatDataType.MaxedSpd] = MaxedSpd;
            stats[StatDataType.MaxedDex] = MaxedDex;
            stats[StatDataType.MaxedVit] = MaxedVit;
            stats[StatDataType.MaxedWis] = MaxedWis;
            stats[StatDataType.SuperMaxedLife] = SuperMaxedLife;
            stats[StatDataType.SuperMaxedMana] = SuperMaxedMana;
            stats[StatDataType.SuperMaxedAtt] = SuperMaxedAtt;
            stats[StatDataType.SuperMaxedDef] = SuperMaxedDef;
            stats[StatDataType.SuperMaxedSpd] = SuperMaxedSpd;
            stats[StatDataType.SuperMaxedDex] = SuperMaxedDex;
            stats[StatDataType.SuperMaxedVit] = SuperMaxedVit;
            stats[StatDataType.SuperMaxedWis] = SuperMaxedWis;
            stats[StatDataType.Node1TickMin] = Node1TickMin;
            stats[StatDataType.Node1TickMaj] = Node1TickMaj;
            stats[StatDataType.Node1Med] = Node1Med;
            stats[StatDataType.Node1Big] = Node1Big;
            stats[StatDataType.Node2TickMin] = Node2TickMin;
            stats[StatDataType.Node2TickMaj] = Node2TickMaj;
            stats[StatDataType.Node2Med] = Node2Med;
            stats[StatDataType.Node2Big] = Node2Big;
            stats[StatDataType.Node3TickMin] = Node3TickMin;
            stats[StatDataType.Node3TickMaj] = Node3TickMaj;
            stats[StatDataType.Node3Med] = Node3Med;
            stats[StatDataType.Node3Big] = Node3Big;
            stats[StatDataType.Node4TickMin] = Node4TickMin;
            stats[StatDataType.Node4TickMaj] = Node4TickMaj;
            stats[StatDataType.Node4Med] = Node4Med;
            stats[StatDataType.Node4Big] = Node4Big;
            stats[StatDataType.Node5TickMin] = Node5TickMin;
            stats[StatDataType.Node5TickMaj] = Node5TickMaj;
            stats[StatDataType.Node5Med] = Node5Med;
            stats[StatDataType.Node5Big] = Node5Big;
            stats[StatDataType.ColorNameChat] = ColorNameChat;
            stats[StatDataType.ColorChat] = ColorChat;
            stats[StatDataType.UpgradeEnabled] = UpgradeEnabled ? 1 : 0;
            stats[StatDataType.PartyId] = Client.Account.PartyId;
            stats[StatDataType.InventoryData0] = Inventory.Data[0]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData1] = Inventory.Data[1]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData2] = Inventory.Data[2]?.GetData() ?? "{}";
            stats[StatDataType.InventoryData3] = Inventory.Data[3]?.GetData() ?? "{}";
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

        private void AnnounceDeath(string killer)
        {
            var maxed = GetMaxedStats();
            var deathMessage = Name + " (" + maxed + (UpgradeEnabled ? "/16, " : "/8, ") + Client.Character.Fame + ") has been killed by " + killer + "!";

            if ((maxed >= 6 || Fame >= 1000) && !Client.Account.Admin)
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
            if (_random.NextDouble() < 0.1 && ApplyEffectCooldown(slot))
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xff00A6FF),
                    Pos1 = new Position() { X = 3 }
                }, this, PacketPriority.Low);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "Clarification!",
                    Color = new ARGB(0xFF00A6FF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this, PacketPriority.Low);

                ActivateHealMp(this, 30 * Stats[1] / 100);
                setCooldownTime(15, slot);
            }
        }

        private void EternalEffects(Item item, int slot)
        {
            if (item.MonkeyKingsWrath)
            {
                if (_random.NextDouble() < .5 && ApplyEffectCooldown(slot))// 50 % chance
                {
                    Size = 100;
                    setCooldownTime(10, slot);
                    World.BroadcastIfVisible(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xFF98ff98),
                        Pos1 = new Position() { X = 3 }
                    }, this, PacketPriority.Low);

                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Monkey King's Wrath!",
                        Color = new ARGB(0xFF98ff98),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this, PacketPriority.Low);
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
            if (_random.NextDouble() < 0.03 && ApplyEffectCooldown(slot))
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffA1A1A1),
                    Pos1 = new Position() { X = 3 }
                }, this, PacketPriority.Low);
                World.BroadcastIfVisible(new Notification()
                {
                    Message = "God Bless!",
                    Color = new ARGB(0xFFFFFFFF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this, PacketPriority.Low);

                ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
                setCooldownTime(5, slot);
            }
        }

        private void GodTouch(int slot)
        {
            if (_random.NextDouble() < 0.02 && ApplyEffectCooldown(slot))
            {
                ActivateHealHp(this, 25 * Stats[0] / 100);
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 3 }
                }, this, PacketPriority.Low);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "God Touch!",
                    Color = new ARGB(0xFFFFFFFF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this, PacketPriority.Low);
                setCooldownTime(30, slot);
            }
        }

        private void HandleRegen(TickTime time)
        {
            // hp regen
            if (HP == Stats[0] || !CanHpRegen())
                _hpRegenCounter = 0;
            else
            {
                _hpRegenCounter += Stats.GetHPRegen() * time.ElaspedMsDelta / 1000f;
                var regen = (int)_hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0], HP + regen);
                    _hpRegenCounter -= regen;
                }
            }

            // mp regen
            if (MP == Stats[1] || !CanMpRegen())
                _mpRegenCounter = 0;
            else
            {
                _mpRegenCounter += Stats.GetMPRegen() * time.ElaspedMsDelta / 1000f;
                var regen = (int)_mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(Stats[1], MP + regen);
                    _mpRegenCounter -= regen;
                }
            }
        }

        private void LegendaryEffects(Item item, int slot)
        {
            var Slot = slot;

            if (item.OutOfOneMind)
            {
                if (_random.NextDouble() < 0.02 && ApplyEffectCooldown(Slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Out of One's Mind!",
                        Color = new ARGB(0xFF00D5D8),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this, PacketPriority.Low);

                    ApplyConditionEffect(ConditionEffectIndex.Berserk, 3000);
                    setCooldownTime(10, Slot);
                }
            }

            if (item.SteamRoller)
            {
                if (_random.NextDouble() < 0.05 && ApplyEffectCooldown(Slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Steam Roller!",
                        Color = new ARGB(0xFF717171),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this, PacketPriority.Low);

                    ApplyConditionEffect(ConditionEffectIndex.Armored, 5000);
                    setCooldownTime(10, Slot);
                }
            }

            if (item.Mutilate)
            {
                if (_random.NextDouble() < 0.08 && ApplyEffectCooldown(Slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Mutilate!",
                        Color = new ARGB(0xFFFF4600),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this, PacketPriority.Low);

                    ApplyConditionEffect(ConditionEffectIndex.Damaging, 3000);
                    setCooldownTime(10, Slot);
                }
            }
        }

        private void MonkeyKingsWrath(int slot)
        {
            if (_random.NextDouble() < .5 && ApplyEffectCooldown(slot))// 50 % chance
            {
                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffff0000),
                    Pos1 = new Position() { X = 3 }
                }, this, PacketPriority.Low);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "Monkey King's Wrath!",
                    Color = new ARGB(0xFFFF0000),
                    PlayerId = Id,
                    ObjectId = Id
                }, this, PacketPriority.Low);
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
                GameId = World.Nexus,
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
                if (_random.NextDouble() < 0.1 && ApplyEffectCooldown(slot))
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
                    }, this, PacketPriority.Low);

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
                if (_random.NextDouble() < 0.05 && ApplyEffectCooldown(slot))
                {
                    World.BroadcastIfVisible(new Notification()
                    {
                        Message = "Insanity!",
                        Color = new ARGB(0xFFFF0000),
                        PlayerId = Id,
                        ObjectId = Id
                    }, this, PacketPriority.Low);

                    setCooldownTime(10, slot);
                    ApplyConditionEffect(ConditionEffectIndex.Berserk, 3000);
                    ApplyConditionEffect(ConditionEffectIndex.Damaging, 3000);
                }
            }

            if (item.HolyProtection)
            {
                if (_random.NextDouble() < 0.1 && ApplyEffectCooldown(slot))
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
                    }, this, PacketPriority.Low);

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
                }, this, PacketPriority.Low);

                World.BroadcastIfVisible(new Notification()
                {
                    Message = "Sonic Blaster!",
                    Color = new ARGB(0xFF9300FF),
                    PlayerId = Id,
                    ObjectId = Id
                }, this, PacketPriority.Low);

                ApplyConditionEffect(ConditionEffectIndex.Invisible, 6000);
                ApplyConditionEffect(ConditionEffectIndex.Speedy, 6000);
                setCooldownTime(30, slot);
            }
        }

        private void SpawnPetIfAttached(World owner)
        {
            // despawn old pet if found
            Pet?.World?.LeaveWorld(Pet);

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

                if (item.Mythical || item.Revenge || item.ObjectId == "Possessed Halberd")
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
            if (World == null)
                return;

            if (World is VaultWorld || World is NexusWorld || World.InstanceType == WorldResourceInstanceType.Guild || World.Id == 10)
                return;

            if (XPBoostTime != 0)
                if (Level >= 20)
                    XPBoostTime = 0;

            if (XPBoostTime > 0)
                XPBoostTime = Math.Max(XPBoostTime - dt, 0);
            if (XPBoostTime == 0)
                XPBoosted = false;
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
