using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace common.resources
{
    public class TalismanDesc
    {
        public readonly int Type;
        public readonly string Name;
        public readonly string ItemAssociatedWith;
        public readonly int BaseUpgradeCost;
        public readonly int TierUpgradeCost;
        public readonly float CostMultiplier;
        public readonly int MaxLevels;
        public readonly bool Requires16;
        public readonly Dictionary<int, TalismanTierDesc> Tiers = new Dictionary<int, TalismanTierDesc>();

        public TalismanDesc(XElement e)
        {
            Type = e.GetAttribute<int>("type");
            Name = e.GetValue<string>("Name");
            ItemAssociatedWith = e.GetValue<string>("ItemAssociatedWith");
            BaseUpgradeCost = e.GetValue<int>("BaseUpgradeCost");
            TierUpgradeCost = e.GetValue<int>("TierUpgradeCost");
            CostMultiplier = e.GetValue<float>("CostMultiplier");
            MaxLevels = e.GetValue<int>("MaxLevels");
            Requires16 = e.HasElement("Requires16");

            Tiers = new Dictionary<int, TalismanTierDesc>();
            foreach (var te in e.Elements("Tier"))
            {
                var tierDesc = new TalismanTierDesc(te);
                Tiers.Add(tierDesc.Tier, tierDesc);
            }
        }

        public TalismanTierDesc GetTierDesc(int tier) => Tiers.TryGetValue(tier, out var ret) ? ret : null;
    }

    public class TalismanTierDesc
    {
        public readonly string RestictionKey;
        public readonly int Tier;
        public readonly List<TalismanStatType> StatTypes;
        public readonly List<TalismanLootBoost> LootBoosts;
        public readonly List<TalismanLootBoost> LootBoostPerPlayer;
        public readonly List<ConditionEffectIndex> ImmuneTo;
        public readonly List<TalismanLeech> Leech;
        public readonly List<TalismanHealth> Health;
        public readonly List<TalismanExtra> Extra;
        public readonly bool CantGetLoot;
        public readonly List<float> FameGainBonus;
        public readonly bool NoPotionHealing;
        //public readonly List<TalismanPotionStack> PotionStack;
        //public readonly bool ShotsPierceArmour;
        //public readonly bool DamageIsAverage;
        public readonly bool RemoveManaBar;
        public readonly float AbilityLifeCost;
        public readonly bool CanOnlyGetWhiteBags;
        //public readonly List<TalismanExtraDamageOn> ExtraDamageOn;

        public TalismanTierDesc(XElement e)
        {
            Tier = e.GetAttribute<int>("tier");
            RestictionKey = e.Element("Restrictions")?.GetAttribute<string>("type");
            
            StatTypes = new List<TalismanStatType>();
            foreach (var te in e.Elements("StatType"))
                StatTypes.Add(new TalismanStatType(te));

            LootBoosts = new List<TalismanLootBoost>();
            foreach (var te in e.Elements("LootBoost"))
                LootBoosts.Add(new TalismanLootBoost(te));

            LootBoostPerPlayer = new List<TalismanLootBoost>();
            foreach (var te in e.Elements("LootBoostPerPlayer"))
                LootBoostPerPlayer.Add(new TalismanLootBoost(te));

            ImmuneTo = new List<ConditionEffectIndex>();
            foreach (var te in e.Elements("ImmuneTo"))
                ImmuneTo.Add(Utils.GetEffect(te.GetAttribute<string>("effect")));

            Leech = new List<TalismanLeech>();
            foreach (var te in e.Elements("Leech"))
                Leech.Add(new TalismanLeech(te));

            Extra = new List<TalismanExtra>();
            foreach (var te in e.Elements("Extra"))
                Extra.Add(new TalismanExtra(te));

            CantGetLoot = e.HasElement("CantGetLoot");
            RemoveManaBar = e.HasElement("RemoveManaBar");
            AbilityLifeCost = e.Element("AbilityLifeCost")?.GetAttribute<float>("percentage") ?? 0.0f;

            FameGainBonus = new List<float>();
            foreach (var te in e.Elements("Extra"))
                FameGainBonus.Add(te.GetAttribute<float>("percentage"));

            Health = new List<TalismanHealth>();
            foreach (var te in e.Elements("Health"))
                Health.Add(new TalismanHealth(te));

            NoPotionHealing = e.HasElement("NoPotionHealing");
            CanOnlyGetWhiteBags = e.HasElement("CanOnlyGetWhiteBags");
        }
    }

    public class TalismanStatType
    {
        public readonly int StatType;
        public readonly float Amount;
        public readonly float Percentage;
        public readonly bool ScalesPerLevel;

        public TalismanStatType(XElement e)
        {
            StatType = e.GetAttribute<int>("type");
            Amount = e.GetAttribute<float>("amount");
            Percentage = e.GetAttribute<float>("Percentage", 1.0f);

            var scale = e.GetAttribute<string>("scale", "flat");
            ScalesPerLevel = scale == "perLevel";
        }
    }
    public class TalismanLootBoost
    {
        public readonly float Percentage;
        public readonly bool ScalesPerLevel;

        public TalismanLootBoost(XElement e)
        {
            Percentage = e.GetAttribute<float>("percentage");

            var scale = e.GetAttribute<string>("scale", "flat");
            ScalesPerLevel = scale == "perLevel";
        }
    }

    public class TalismanHealth
    {
        public const byte RATE_OF_FIRE = 0;
        public const byte HEALTH_REGEN = 1;

        public readonly byte Type;
        public readonly bool Above;
        public readonly float HealthPercent;
        public readonly float AddPercent;
        public readonly bool ScalesPerLevel;

        public TalismanHealth(XElement e)
        {
            HealthPercent = e.GetAttribute<float>("percent");
            AddPercent = e.GetAttribute<float>("add");
            Above = e.GetAttribute<string>("condition", "above") == "above";

            var t = e.Value;
            switch (t)
            {
                case "RateOfFire":
                    Type = RATE_OF_FIRE;
                    break;
                case "HealthRegen":
                    Type = HEALTH_REGEN;
                    break;
            }
        var scale = e.GetAttribute<string>("scale", "flat");
            ScalesPerLevel = scale == "perLevel";
        }
    }

    public class TalismanLeech
    {
        public readonly float Probability;
        public readonly byte Type;
        public readonly float Percentage;
        public readonly bool ScalesPerLevel;

        public TalismanLeech(XElement e)
        {

            Probability = e.GetAttribute<float>("prob");
            Type = (byte)(e.GetAttribute<string>("type") == "life" ? 0 : 1);
            Percentage = e.GetAttribute<float>("percentage");
            var scale = e.GetAttribute<string>("scale", "flat");
            ScalesPerLevel = scale == "perLevel";
        }
    }

    public class TalismanExtra
    {
        public const byte ABILITY_DAMAGE = 0;
        public const byte LIFE_REGEN = 1;
        public const byte MANA_REGEN = 2;

        public readonly byte Type;
        public readonly float Percentage;
        public readonly bool ScalesPerLevel;

        public TalismanExtra(XElement e)
        {
            Percentage = e.GetAttribute<float>("percentage");

            var t = e.Value;
            switch (t)
            {
                case "AbilityDamage":
                    Type = ABILITY_DAMAGE;
                    break;
                case "LifeRegen":
                    Type = LIFE_REGEN;
                    break;
                case "ManaRegen":
                    Type = MANA_REGEN;
                    break;
            }

            var scale = e.GetAttribute<string>("scale", "flat");
            ScalesPerLevel = scale == "perLevel";
        }
    }
}
