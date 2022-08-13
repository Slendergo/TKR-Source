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
                Tiers.Add(tierDesc.Type, tierDesc);
            }
        }
    }

    public class TalismanTierDesc
    {
        public readonly int Type;

        public TalismanTierDesc(XElement e)
        {
            Type = e.GetAttribute<int>("type");

        }
    }
}
