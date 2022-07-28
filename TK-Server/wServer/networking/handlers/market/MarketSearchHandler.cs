using common.database;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing.market;

namespace wServer.networking.handlers.market
{
    internal class MarketSearchHandler : PacketHandlerBase<MarketSearch>
    {
        public override PacketId ID => PacketId.MARKET_SEARCH;

        protected override void HandlePacket(Client client, MarketSearch packet)
        {
            if (!IsAvailable(client) || !IsEnabledOrIsVipMarket(client))
                return;

            Handle(client, packet);
        }

        private IEnumerable<MarketData> GetOffers(DbMarketData[] offers, Predicate<int> isSameAccount)
        {
            for (var i = 0; i < offers.Length; i++)
            {
                if (isSameAccount.Invoke(offers[i].SellerId))
                    continue;
                else yield return new MarketData
                {
                    Id = offers[i].Id,
                    ItemType = offers[i].ItemType,
                    SellerName = offers[i].SellerName,
                    SellerId = offers[i].SellerId,
                    Price = offers[i].Price,
                    TimeLeft = offers[i].TimeLeft,
                    StartTime = offers[i].StartTime,
                    Currency = (int)offers[i].Currency,
                    ItemData = offers[i].ItemData
                };
            }
        }

        private void Handle(Client client, MarketSearch packet)
        {
            var accountId = client.Account.AccountId;
            var offers = GetOffers(DbMarketData.Get(client.CoreServerManager.Database.Conn, (ushort)packet.ItemType), (sellerAccountId) => accountId == sellerAccountId).ToArray();

            if (!HandleEmptyOffer(client, offers.Length)) return;

            client.SendPacket(new MarketSearchResult
            {
                Results = offers,
                Description = ""
            });
        }

        private bool HandleEmptyOffer(Client client, int total)
        {
            if (total == 0)
            {
                client.SendPacket(new MarketSearchResult
                {
                    Results = new MarketData[0],
                    Description = "There is no items currently being sold with this type."
                });
                return false;
            }
            return true;
        }
    }
}
