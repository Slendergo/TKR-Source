﻿using common;
using wServer.core.worlds;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public class EscapeHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.ESCAPE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            if (client.Player == null || client.Player.World == null)
                return;

            var map = client.Player.World;

            if (map.Id == World.Nexus)
            {
                client.Disconnect("Already in Nexus!");
                return;
            }

            client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = client.CoreServerManager.ServerConfig.serverInfo.port,
                GameId = World.Nexus,
                Name = "Nexus"
            });
        }
    }
}