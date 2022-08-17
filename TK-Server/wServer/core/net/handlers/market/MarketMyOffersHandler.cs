using common;
using common.database;
using wServer.networking;
using wServer.networking.packets.outgoing.market;

namespace wServer.core.net.handlers.market
{
    public class MarketMyOffersHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.MARKET_MY_OFFERS;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            if (!IsAvailable(client) || !IsEnabledOrIsVipMarket(client))
                return;

            client?.Account?.Reload("marketOffers");
            MarketData[] offers = new MarketData[client.Account.MarketOffers.Length];
            var playerOffers = client.Account.MarketOffers;

            for (var i = 0; i < playerOffers.Length; i++)
            {
                var data = DbMarketData.GetSpecificOffer(client.Account.Database, playerOffers[i]);
                if (data == null)
                    continue;

                var mData = new MarketData
                {
                    Id = data.Id,
                    ItemType = data.ItemType,
                    SellerId = data.SellerId,
                    SellerName = data.SellerName,
                    Currency = (int)data.Currency,
                    Price = data.Price,
                    StartTime = data.StartTime,
                    TimeLeft = data.TimeLeft,
                    ItemData = data.ItemData
                };

                offers[i] = mData;
            }

            client.SendPacket(new MarketMyOffersResult { Results = offers });
        }
    }
}
