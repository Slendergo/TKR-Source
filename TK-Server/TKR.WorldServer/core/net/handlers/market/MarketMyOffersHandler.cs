using TKR.Shared;
using TKR.Shared.database.market;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing.market;

namespace TKR.WorldServer.core.net.handlers.market
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

            client.SendMessage(new MarketMyOffersResult { Results = offers });
        }
    }
}
