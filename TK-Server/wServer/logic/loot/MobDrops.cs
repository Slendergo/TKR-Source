using System;
using System.Collections.Generic;
using System.Linq;
using common.resources;
using NLog;
using wServer.core;

namespace wServer.logic.loot
{
    public abstract class MobDrops
    {
        protected static XmlData XmlData;
        protected readonly IList<LootDef> LootDefs = new List<LootDef>();

        public static void Initialize(GameServer gameServer)
        {
            if (XmlData != null)
                throw new Exception("MobDrops already initialized");
            XmlData = gameServer.Resources.GameData;
        }

        public virtual void Populate(IList<LootDef> lootDefs, LootDef overrides = null)
        {
            if (overrides == null)
            {
                foreach (var lootDef in LootDefs)
                    lootDefs.Add(lootDef);
                return;
            }

            foreach (var lootDef in LootDefs)
            {

                // sample:
                // var previousDropRate = lootDef.Probabilty;
                // Console.WriteLine($"[MT tier] update drop rate -> item: '{lootDef.Item.ObjectId}' from '{previousDropRate:0.00%}' to '{lootDef.Probabilty:0.00%}'");

                if (lootDef.Item.Mythical || lootDef.Item.Revenge)
                {
                    lootDef.Probabilty = Loot.DropRates.MT;
                    lootDef.Threshold = Loot.DropRates.MT_THRESHOLD;
                }

                if (lootDef.Item.Legendary)
                {
                    lootDef.Probabilty = Loot.DropRates.LG;
                    lootDef.Threshold = Loot.DropRates.LG_THRESHOLD;
                }

                if (Loot.DropRates.FRAGMENTS_NAMES.Contains(lootDef.Item.ObjectId))
                {
                    lootDef.Probabilty = Loot.DropRates.FRAGMENTS;
                    lootDef.Threshold = Loot.DropRates.FRAGMENT_THRESHOLD;
                }

                if (Loot.DropRates.ORYX_ITEMS_NAMES.Contains(lootDef.Item.ObjectId))
                {
                    lootDef.Probabilty = Loot.DropRates.ORYX_ITEMS;
                    lootDef.Threshold = Loot.DropRates.ORYX_ITEMS_THRESHOLD;
                }
                if (Loot.DropRates.LG_TALISMAN_NAMES.Contains(lootDef.Item.ObjectId))
                {
                    lootDef.Probabilty = Loot.DropRates.LG_TALISMAN;
                    lootDef.Threshold = Loot.DropRates.LG_TALISMAN_THRESHOLD;
                }
                if (Loot.DropRates.MT_TALISMAN_NAMES.Contains(lootDef.Item.ObjectId))
                {
                    lootDef.Probabilty = Loot.DropRates.MT_TALISMAN;
                    lootDef.Threshold = Loot.DropRates.MT_TALISMAN_THRESHOLD;
                }
                if (Loot.DropRates.HARD_BOUNTY_NAMES.Contains(lootDef.Item.ObjectId))
                {
                    lootDef.Probabilty = Loot.DropRates.HARD_BOUNTY;
                    lootDef.Threshold = Loot.DropRates.HARD_BOUNTY_THRESHOLD;
                }

                lootDefs.Add(new LootDef(
                    lootDef.Item,
                    overrides.Probabilty >= 0 ? overrides.Probabilty : lootDef.Probabilty,               
                    overrides.Threshold >= 0 ? overrides.Threshold : lootDef.Threshold));
            }
        }
    }

    public class ItemLoot : MobDrops
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ItemLoot(string item, double probability = 1, int numRequired = 0, double threshold = 0)
        {
            try
            {
                LootDefs.Add(new LootDef(
                    XmlData.Items[XmlData.IdToObjectType[item]],
                    probability,
                    threshold));
            }
            catch (Exception)
            {
                Log.Warn($"Problem adding {item} to mob loot table.");
            }
        }
    }

    public class TierLoot : MobDrops
    {
        private static readonly int[] TalismanT = new int[] { 26 };
        private static readonly int[] WeaponT = new int[] { 1, 2, 3, 8, 17, 24 }; //26 == Talisman of Looting
        private static readonly int[] AbilityT = new int[] { 4, 5, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 25, };
        private static readonly int[] ArmorT = new int[] { 6, 7, 14, };
        private static readonly int[] RingT = new int[] { 9 };
        private static readonly int[] PotionT = new int[] { 10 };

        public static int[] GetSlotTypes(ItemType itemType)
        {
            int[] types;
            switch (itemType)
            {
                case ItemType.Weapon:
                    types = WeaponT; break;
                case ItemType.Ability:
                    types = AbilityT; break;
                case ItemType.Armor:
                    types = ArmorT; break;
                case ItemType.Ring:
                    types = RingT; break;
                case ItemType.Potion:
                    types = PotionT; break;
                case ItemType.Talisman:
                    types = TalismanT; break;
                default:
                    throw new NotSupportedException(itemType.ToString());
            }
            return types;
        }

        public TierLoot(byte tier, ItemType type, double probability = 1, int numRequired = 0, double threshold = 0)
        {
            var types = GetSlotTypes(type);

            var items = XmlData.Items
                .Where(item => Array.IndexOf(types, item.Value.SlotType) != -1)
                .Where(item => item.Value.Tier == tier)
                .Select(item => item.Value)
                .ToArray();

            foreach (var item in items)
                LootDefs.Add(new LootDef(
                    item,
                    probability / items.Length,
                    threshold));
        }
    }

    public class LootTemplates : MobDrops
    {
        public static MobDrops[] DustLoot()
        {
            return new MobDrops[]
            {
                new ItemLoot("Potion Dust", 0.01), // 1.0%
                new ItemLoot("Item Dust", 0.025),  // 2.5%
                new ItemLoot("Miscellaneous Dust", 0.030), // 3%
                new ItemLoot("Special Dust", 0.0025) // 0.25%
             };
        }
    }

    public class Threshold : MobDrops
    {
        public Threshold(double threshold, params MobDrops[] children)
        {
            foreach (var i in children)
                i.Populate(LootDefs, new LootDef(null, -1, threshold));
        }
    }
}
