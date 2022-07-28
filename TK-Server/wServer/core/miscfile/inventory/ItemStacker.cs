using common.resources;
using wServer.core.objects;

namespace wServer.core
{
    public class ItemStacker
    {
        public Player Owner;
        private SV<int> _count;

        public ItemStacker(Player owner, int slot, ushort objectType, int count, int maxCount)
        {
            Owner = owner;
            Slot = slot;
            Item = Owner.CoreServerManager.Resources.GameData.Items[objectType];
            MaxCount = maxCount;

            _count = new SV<int>(owner, GetStatsType(slot), count);
        }

        public int Count { get => _count.GetValue(); set => _count.SetValue(value); }

        public Item Item { get; private set; }
        public int MaxCount { get; private set; }
        public int Slot { get; private set; }

        public Item Pop()
        {
            if (Count > 0)
            {
                Count--;
                return Item;
            }
            return null;
        }

        public Item Push(Item item)
        {
            if (Count < MaxCount && item == Item)
            {
                Count++;
                return null;
            }
            return item;
        }

        private static StatDataType GetStatsType(int slot)
        {
            switch (slot)
            {
                case 254:
                    return StatDataType.HealthStackCount;

                case 255:
                    return StatDataType.MagicStackCount;

                default:
                    return StatDataType.None;
            }
        }
    }
}
