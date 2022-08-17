using common;
using NLog;
using wServer.core;
using wServer.core.net.handlers;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    internal class ChangeTradeHandler : IMessageHandler
    {
        private static readonly Logger CheatLog = LogManager.GetLogger("CheatLog");

        public override MessageId MessageId => MessageId.CHANGETRADE;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var offer = new bool[rdr.ReadInt16()];
            for (int i = 0; i < offer.Length; i++)
                offer[i] = rdr.ReadBoolean();
            
            var sb = false;
            var player = client.Player;

            if (player == null || client?.Player?.World is TestWorld)
                return;

            if (player.tradeTarget == null)
                return;

            for (int i = 0; i < offer.Length; i++)
                if (offer[i])
                    if (player.Inventory[i].Soulbound)
                    {
                        sb = true;
                        offer[i] = false;
                    }

            player.tradeAccepted = false;
            player.tradeTarget.tradeAccepted = false;
            player.trade = offer;
            player.tradeTarget.Client.SendPacket(new TradeChanged()
            {
                Offer = player.trade
            });

            if (sb)
            {
                CheatLog.Info("User {0} tried to trade a Soulbound item.", player.Name);
                player.SendError("You can't trade Soulbound items.");
            }
        }
    }
}
