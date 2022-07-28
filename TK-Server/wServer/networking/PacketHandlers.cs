using System;
using System.Collections.Generic;
using wServer.networking.packets;

namespace wServer.networking
{
    internal static class PacketHandlers
    {
        public static Dictionary<PacketId, IPacketHandler> Handlers = new Dictionary<PacketId, IPacketHandler>();

        static PacketHandlers()
        {
            foreach (var i in typeof(Packet).Assembly.GetTypes())
                if (typeof(IPacketHandler).IsAssignableFrom(i) && !i.IsAbstract && !i.IsInterface)
                {
                    IPacketHandler pkt = (IPacketHandler)Activator.CreateInstance(i);
                    Handlers.Add(pkt.ID, pkt);
                }
        }
    }
}
