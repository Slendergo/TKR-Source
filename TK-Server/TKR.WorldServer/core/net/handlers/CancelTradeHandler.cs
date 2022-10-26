using TKR.Shared;
using TKR.Shared.database;
using TKR.Shared.resources;
using System.Collections.Generic;
using System.Linq;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.objects.vendors;
using TKR.WorldServer.networking.packets;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.core.worlds.impl;
using TKR.WorldServer.networking;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.net.handlers
{
    public sealed class CancelTradeHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.CANCELTRADE;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime time)
        {
            var player = client.Player;
            if (player == null || client?.Player?.World is TestWorld)
                return;
            player.CancelTrade();
        }
    }
}
