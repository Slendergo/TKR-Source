using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace common.resources
{
    public class ItemPoolType
    {
        public ItemType ItemType { get; set; }
        public string ItemName { get; set; }
        public int Weight { get; set; }
        public int Tier { get; set; }

        public ItemPoolType(XElement e)
        {
            try
            {
                ItemType = (ItemType)Enum.Parse(typeof(ItemType), e.Value, true);
            }
            catch (Exception)
            {
                ItemType = ItemType.None;
            }
            ItemName = e.Value;
            Tier = e.GetAttribute("tier", -1);
            Weight = e.GetAttribute<int>("weight");
            if (Weight == 0)
                throw new Exception("Weight should be set");
        }
    }

    public class ItemDusts
    {
        public List<ItemPool> ItemPools { get; set; } = new List<ItemPool>();
        public List<ItemPool> MagicPools { get; set; } = new List<ItemPool>();
        public List<ItemPool> SpecialPools { get; set; } = new List<ItemPool>();
        public List<ItemPool> MiscPools { get; set; } = new List<ItemPool>();
        public List<ItemPool> PotionPools { get; set; } = new List<ItemPool>();
        public List<ItemPool> TalismanPools { get; set; } = new List<ItemPool>();

        public ItemDusts(XElement element)
        {
            var items = element.Element("ItemDust")?.Elements("ItemPool");
            foreach (var item in items)
                ItemPools.Add(new ItemPool(item));
            items = element.Element("TalismanFragment").Elements("ItemPool");
            foreach (var item in items)
                TalismanPools.Add(new ItemPool(item));
            items = element.Element("MagicDust").Elements("ItemPool");
            foreach (var item in items)
                MagicPools.Add(new ItemPool(item));
            items = element.Element("SpecialDust").Elements("ItemPool");
            foreach (var item in items)
                SpecialPools.Add(new ItemPool(item));
            items = element.Element("MiscDust").Elements("ItemPool");
            foreach (var item in items)
                MiscPools.Add(new ItemPool(item));
            items = element.Element("PotionDust").Elements("ItemPool");
            foreach (var item in items)
                PotionPools.Add(new ItemPool(item));
        }
    }

    public class ItemPool
    {
        public List<ItemPoolType> TieredItems { get; private set; } = new List<ItemPoolType>();
        public List<ItemPoolType> NamedItems { get; private set; } = new List<ItemPoolType>();

        public ItemPool(XElement element)
        {
            var items = element.Elements("TieredItem");
            foreach (var item in items)
                TieredItems.Add(new ItemPoolType(item));
            items = element.Elements("Item");
            foreach (var item in items)
                NamedItems.Add(new ItemPoolType(item));
        }
    }
}
