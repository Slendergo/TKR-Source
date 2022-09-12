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
using wServer.utils;
using System.Text;

namespace wServer.core.net.handlers
{
    public sealed class ShootAckMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.SHOOTACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();

            var player = client.Player;

            // time is used to set projectile start time
            var pendingServerShoot = player.PendingShootAcks.Dequeue();
            if (time == -1)
            {
                // check if entity was removed like how -1 works in client
                return;
            }

            var projectiles = pendingServerShoot.Acknowledge(player.GameServer, time);
            foreach (var projectile in projectiles.Value)
                player.AddProjectile(projectile);
        }
    }
}
