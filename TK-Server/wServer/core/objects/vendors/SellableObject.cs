using common;
using common.resources;
using System;
using System.Collections.Generic;
using wServer.core.worlds.logic;

namespace wServer.core.objects.vendors
{
    public abstract class SellableObject : StaticObject
    {
        protected static Random Rand = new Random();

        private SV<CurrencyType> _currency;
        private SV<int> _price;
        private SV<int> _rankReq;

        protected SellableObject(CoreServerManager manager, ushort objType) : base(manager, objType, null, true, false, false)
        {
            _price = new SV<int>(this, StatDataType.SellablePrice, 0);
            _currency = new SV<CurrencyType>(this, StatDataType.SellablePriceCurrency, 0);
            _rankReq = new SV<int>(this, StatDataType.SellableRankRequirement, 0);
        }

        public CurrencyType Currency { get => _currency.GetValue(); set => _currency.SetValue(value); }
        public int Price { get => _price.GetValue(); set => _price.SetValue(value); }
        public int RankReq { get => _rankReq.GetValue(); set => _rankReq.SetValue(value); }
        public int Tax { get; set; }

        public virtual void Buy(Player player) => SendFailed(player, BuyResult.Uninitialized);

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.SellablePrice] = Price;
            stats[StatDataType.SellablePriceCurrency] = (int)Currency;
            stats[StatDataType.SellableRankRequirement] = RankReq;

            base.ExportStats(stats);
        }

        protected void SendFailed(Player player, BuyResult result) => player.Client.SendPacket(new networking.packets.outgoing.BuyResult
        {
            Result = 1,
            ResultString = $"Purchase Error: {result.GetDescription()}"
        });

        protected BuyResult ValidateCustomer(Player player, Item item)
        {
            if (Owner is Test)
                return BuyResult.IsTestMap;

            if (player.Stars < RankReq)
                return BuyResult.InsufficientRank;

            var acc = player.Client.Account;

            if (acc.Guest)
            {
                // reload guest prop just in case user registered in game
                acc.Reload("guest");

                if (acc.Guest)
                    return BuyResult.IsGuest;
            }

            if (player.GetCurrency(Currency) < Price)
                return BuyResult.InsufficientFunds;

            if (item != null) // not perfect, but does the job for now
            {
                var availableSlot = player.Inventory.CreateTransaction().GetAvailableInventorySlot(item);

                if (availableSlot == -1)
                    return BuyResult.NotEnoughSlots;
            }

            return BuyResult.Ok;
        }
    }
}
