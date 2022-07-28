using common.database;
using wServer.networking.packets;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing.market;

namespace wServer.networking.handlers.market
{
    internal class MarketMyOffersHandler : PacketHandlerBase<MarketMyOffers>
    {
        public override PacketId ID => PacketId.MARKET_MY_OFFERS;

        protected override void HandlePacket(Client client, MarketMyOffers packet)
        {
            if (!IsAvailable(client) || !IsEnabledOrIsVipMarket(client))
                return;

            Handle(client);
        }

        private void Handle(Client client)
        {
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
