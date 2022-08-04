using common;
using common.database;
using common.resources;
using System.Collections.Generic;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.core.objects.vendors;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.core.net.handlers;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public sealed class CancelTradeHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.CANCELTRADE;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var player = client.Player;
            if (player == null || client?.Player?.World is TestWorld)
                return;
            player.CancelTrade();
        }
    }
}
