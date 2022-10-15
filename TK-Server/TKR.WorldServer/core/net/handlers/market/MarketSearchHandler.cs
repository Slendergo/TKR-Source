using TKR.Shared;
using TKR.Shared.database;
using System;
using System.Collections.Generic;
using System.Linq;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing.market;
using TKR.Shared.database.market;

namespace TKR.WorldServer.core.net.handlers.market
{
    public class MarketSearchHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.MARKET_SEARCH;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime tickTime)
        {
            var itemType = rdr.ReadInt32();

            if (!IsAvailable(client) || !IsEnabledOrIsVipMarket(client))
                return;

            var accountId = client.Account.AccountId;
            var offers = GetOffers(DbMarketData.Get(client.GameServer.Database.Conn, (ushort)itemType), (sellerAccountId) => accountId == sellerAccountId).ToArray();

            if (!HandleEmptyOffer(client, offers.Length))
                return;

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
    }
}