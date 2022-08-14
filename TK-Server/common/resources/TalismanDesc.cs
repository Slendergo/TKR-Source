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
        public readonly int Tier;
        public readonly List<TalismanStatType> StatTypes;

        public TalismanTierDesc(XElement e)
        {
            Tier = e.GetAttribute<int>("tier");

            StatTypes = new List<TalismanStatType>();
            foreach (var te in e.Elements("StatType"))
                StatTypes.Add(new TalismanStatType(te));
        }
    }

    public class TalismanStatType
    {
        public readonly int StatType;
        public readonly int Amount;
        public readonly bool ScalesPerLevel;

        public TalismanStatType(XElement e)
        {
            StatType = e.GetAttribute<int>("type");
            Amount = e.GetAttribute<int>("amount");

            var scale = e.GetAttribute<string>("scale", "flat");
            ScalesPerLevel = scale == "perLevel";
        }
    }
}
