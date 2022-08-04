using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using wServer.core;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;

namespace wServer.core.net.handlers
{
    public abstract class IMessageHandler
    {
        public abstract PacketId MessageId { get; }
        public abstract void Handle(Client client, NReader rdr, ref TickTime time);

        public static bool IsAvailable(Client client)
        {
            if (client == null || client.Account == null || client.Player == null || client.Player.World == null || client?.Player?.World is TestWorld)
                return false;
            return true;
        }

        public static bool IsEnabledOrIsVipMarket(Client client)
        {
            var config = Program.CoreServerManager.ServerConfig;

            if (config.serverInfo.adminOnly)
            {
                if (!Program.CoreServerManager.IsWhitelisted(client.Player.AccountId) || client.Player?.Rank < 110)
                {
                    client.Player.SendError("Admin Only, you need to be Whitelisted to use this.");
                    return false;
                }
            }
            else
            {
                if (!client.Player.CanUseThisFeature(core.objects.Player.GenericRank.VIP))
                {
                    client.Player.SendError("You can't use this Feature.");
                    return false;
                }
            }
            return true;
        }
    }

    public static class MessageHandlers
    {
        private static Dictionary<PacketId, IMessageHandler> Handlers;

        public static IMessageHandler GetHandler(PacketId messageId)
        {
            if (Handlers == null)
            {
                Handlers = new Dictionary<PacketId, IMessageHandler>();
                foreach (var type in Assembly.GetAssembly(typeof(IMessageHandler)).GetTypes().Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(IMessageHandler))))
                {
                    var handler = (IMessageHandler)Activator.CreateInstance(type);
                    Handlers.Add(handler.MessageId, handler);
                }
            }
            return Handlers.ContainsKey(messageId) ? Handlers[messageId] : null;
        }
    }
}   
