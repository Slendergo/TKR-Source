using TKR.Shared.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using TKR.WorldServer.core;
using TKR.WorldServer.logic.loot;

namespace TKR.WorldServer.core.objects.inventory
{
    public sealed class ItemDustWeights
    {
        public ItemDustPools ItemDusts { get; private set; } = new ItemDustPools();
        public ItemDustPools MagicDust { get; private set; } = new ItemDustPools();
        public ItemDustPools SpecialDust { get; private set; } = new ItemDustPools();
        public ItemDustPools MiscDust { get; private set; } = new ItemDustPools();
        public ItemDustPools PotionDust { get; private set; } = new ItemDustPools();
        public ItemDustPools TalismanFragment { get; private set; } = new ItemDustPools();
        public ItemDustPools FrozenCoin { get; private set; } = new ItemDustPools();

        private readonly GameServer GameServer;

        public ItemDustWeights(GameServer gameServer) => GameServer = gameServer;

        public void Initialize()
        {
            var xmlData = GameServer.Resources.GameData;
            foreach (var items in xmlData.ItemDusts.ItemPools)
                ItemDusts.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.MagicPools)
                MagicDust.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.SpecialPools)
                SpecialDust.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.MiscPools)
                MiscDust.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.TalismanPools)
                TalismanFragment.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.FrozenCoinPools)
                FrozenCoin.AddPool(GetItems(items, xmlData));
            foreach (var items in xmlData.ItemDusts.PotionPools)
                PotionDust.AddPool(GetItems(items, xmlData));
        }

        private List<KeyValuePair<Item, int>> GetItems(ItemPool items, XmlData xmlData)
        {
            var poolItems = new List<KeyValuePair<Item, int>>();
            foreach (var tieredItem in items.TieredItems)
            {
                var slotTypes = TierLoot.GetSlotTypes(tieredItem.ItemType);

                var tieredItems = xmlData.Items
                    .Where(item => Array.IndexOf(slotTypes, item.Value.SlotType) != -1)
                    .Where(item => item.Value.Tier == tieredItem.Tier)
                    .Select(item => item.Value);

                foreach (var item in tieredItems)
                    poolItems.Add(new KeyValuePair<Item, int>(item, tieredItem.Weight));
            }

            foreach (var namedItem in items.NamedItems)
            {
                var foundItem = xmlData.Items.Values.FirstOrDefault(item => item.ObjectId == namedItem.ItemName);
                if (foundItem == null)
                    throw new Exception("Invalid Name of item");
                poolItems.Add(new KeyValuePair<Item, int>(foundItem, namedItem.Weight));
            }
            return poolItems;
        }
    }
}
