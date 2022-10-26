﻿using TKR.Shared;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class TeleportHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.TELEPORT;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime tickTime)
        {
            var objectId = rdr.ReadInt32();

            var player = client.Player;
            if (player == null || player.World == null)
                return;

            player.Teleport(tickTime, objectId);
        }
    }
}
