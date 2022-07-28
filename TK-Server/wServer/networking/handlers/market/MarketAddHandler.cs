using common;
using common.database;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing.market;

namespace wServer.networking.handlers.market
{
    internal class MarketAddHandler : PacketHandlerBase<MarketAdd>
    {
        public override PacketId ID => PacketId.MARKET_ADD;

        protected override void HandlePacket(Client client, MarketAdd packet)
        {
            if (!IsAvailable(client) || !IsEnabledOrIsVipMarket(client))
                return;

            Handle(client, packet);
        }

        private void Handle(Client client, MarketAdd packet)
        {
            var player = client.Player;

            if (!HandleInvalidUptime(client, packet.Hours) || !HandleInvalidPrice(client, packet.Price) || !HandleInvalidCurrency(client, packet.Currency))
                return;

            var amountOfItems = -1;
            var transaction = client.Player.Inventory.CreateTransaction();
            var dataTrans = client.Player.Inventory.CreateDataTransaction();
            var pendingItems = new List<(byte slotId, Item item, ItemData data)>();

            for (var i = 0; i < packet.Slots.Length; i++)
            {
                var slotId = packet.Slots[i];
                var item = transaction[slotId];
                var data = dataTrans[slotId];

                if (item == null)
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.SLOT_IS_NULL,
                        Description = $"There is no item on slot {slotId + 1}."
                    });
                    return;
                }

                if (item.Soulbound)
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.ITEM_IS_SOULBOUND,
                        Description = "You cannot sell soulbound items."
                    });
                    return;
                }

                pendingItems.Add((slotId, item, data));
            }

            if (pendingItems.Count == 0)
            {
                client.Player.SendError("There is no item to perform this action, try again later.");
                return;
            }

            if (!transaction.Validate() || !dataTrans.Validate())
            {
                client.Player.SendError("Your inventory was recently updated, try again later.");
                return;
            }

            for (var j = 0; j < pendingItems.Count; j++)
            {
                var slotId = pendingItems[j].slotId;

                transaction[slotId] = null;
                dataTrans[slotId] = null;
            }

            amountOfItems = pendingItems.Count;

            client.Player = OverrideInventory(transaction.ChangedItems, dataTrans.ChangedItems, client.Player);

            var db = client.CoreServerManager.Database;
            var task = db.AddMarketEntrySafety(
                client.Account,
                pendingItems.Select(pendingItem => (pendingItem.item.ObjectType, pendingItem.data?.GetData() ?? null)).ToList(),
                client.Player.AccountId,
                client.Player.Name,
                packet.Price,
                DateTime.UtcNow.AddHours(packet.Hours).ToUnixTimestamp(),
                (CurrencyType)packet.Currency
            );

            if (task.IsCanceled)
            {
                client.Player = OverrideInventory(transaction.OriginalItems, dataTrans.OriginalItems, client.Player);
                client.Player.SendError("D'oh! Something went wrong, try again later...");
                return;
            }

            client.SendPacket(new MarketAddResult
            {
                Code = -1,
                Description = $"Successfully added {amountOfItems} item{(amountOfItems > 1 ? "s" : "")} to the market."
            });
        }

        private bool HandleInvalidCurrency(Client client, int currency)
        {
            if (!Enum.IsDefined(typeof(CurrencyType), currency) || currency == (int)CurrencyType.GuildFame)
            {
                client.SendPacket(new MarketAddResult
                {
                    Code = MarketAddResult.INVALID_CURRENCY,
                    Description = "Invalid currency."
                });
                return false;
            }

            return true;
        }

        private bool HandleInvalidPrice(Client client, int price)
        {
            if (price <= 0)
            {
                client.SendPacket(new MarketAddResult
                {
                    Code = MarketAddResult.INVALID_PRICE,
                    Description = "You cannot sell items for 0 or less."
                });
                return false;
            }

            return true;
        }

        private bool HandleInvalidUptime(Client client, int hours)
        {
            if (hours <= 0 || hours > 24)
            {
                client.SendPacket(new MarketAddResult
                {
                    Code = MarketAddResult.INVALID_UPTIME,
                    Description = "Only 1-24 hours uptime allowed."
                });
                return false;
            }

            return true;
        }

        private Player OverrideInventory(Item[] items, ItemData[] datas, Player player)
        {
            player.Inventory.SetItems(items);
            player.Inventory.Data.SetDatas(datas);
            player.SaveToCharacter();

            return player;
        }
    }
}
