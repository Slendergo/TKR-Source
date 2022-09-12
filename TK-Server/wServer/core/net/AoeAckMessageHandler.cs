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
    public sealed class AoeAckMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.AOEACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var x = rdr.ReadSingle();
            var y = rdr.ReadSingle();

        }
    }
}
