using common.resources;
using System;
using wServer.core.objects;

namespace wServer.core
{
    public class InventoryItems
    {
        private Item[] _items;
        private SV<int>[] _itemTypes;

        public InventoryItems(IContainer container, Item[] items)
        {
            _itemTypes = new SV<int>[items.Length];
            _items = new Item[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                var sti = (int)StatDataType.Inventory0 + i;
                if (i >= 12)
                    sti = (int)StatDataType.BackPack0 + i - 12;

                _itemTypes[i] = new SV<int>(container as Entity, (StatDataType)sti, items[i]?.ObjectType ?? -1, container is Player && i > 3);
                _items[i] = items[i];
            }
        }

        public int Length => _items.Length;

        public Item this[int index]
        {
            get => _items[index];
            set
            {
                _itemTypes[index].SetValue(value?.ObjectType ?? -1);
                _items[index] = value;
            }
        }

        public Item[] GetItems() => (Item[])_items.Clone();

        public void SetItems(Item[] items)
        {
            if (items.Length > Length)
                throw new InvalidOperationException("Item array must be <= the size of the initialized array.");

            for (var i = 0; i < items.Length; i++)
            {
                _itemTypes[i].SetValue(items[i]?.ObjectType ?? -1);
                _items[i] = items[i];
            }
        }
    }
}
