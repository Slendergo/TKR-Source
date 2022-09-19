using TKR.Shared;
using TKR.Shared.database;
using TKR.Shared.resources;
using System.Collections.Generic;
using System.Linq;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.objects.vendors;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking.packets;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.utils;
using System.Text;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.net.handlers;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public sealed class ShootAckMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.SHOOTACK;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
        }
    }
}
