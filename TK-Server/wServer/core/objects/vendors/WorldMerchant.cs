using common;
using System.Collections.Generic;

namespace wServer.core.objects.vendors
{
    internal class WorldMerchant : Merchant
    {
        public WorldMerchant(CoreServerManager manager, ushort objType) : base(manager, objType)
        { }

        public List<ISellableItem> ItemList { get; set; }
        public ISellableItem ShopItem { get; set; }

        public override void Reload()
        {
            if (Reloading)
                return;

            Reloading = true;

            int i;

            if (ItemList == null || (i = ItemList.IndexOf(ShopItem)) == -1)
            {
                Owner.LeaveWorld(this);
                return;
            }

            if (ShopItem.Count == 0)
            {
                ItemList.Remove(ShopItem);
                if (ItemList.Count <= 0)
                {
                    Owner.LeaveWorld(this);
                    return;
                }
            }

            var nextItem = ItemList.OneElement(Rand);

            ShopItem = nextItem;
            Item = nextItem.ItemId;
            Price = nextItem.Price;
            Count = nextItem.Count;

            Reloading = false;
        }

        public override void Tick(TickData time)
        {
            if (ShopItem == null && TimeLeft != 0 && Count != 0)
                return;

            base.Tick(time);
        }
    }
}
