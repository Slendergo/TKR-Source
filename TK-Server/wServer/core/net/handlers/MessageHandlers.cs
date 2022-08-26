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
        public abstract MessageId MessageId { get; }
        public abstract void Handle(Client client, NReader rdr, ref TickTime time);

        public static bool IsAvailable(Client client)
        {
            if (client == null || client.Account == null || client.Player == null || client.Player.World == null || client?.Player?.World is TestWorld)
                return false;
            return true;
        }

        public static bool IsEnabledOrIsVipMarket(Client client)
        {
            var config = client.Player.GameServer.Configuration;

            if (config.serverInfo.adminOnly)
            {
                if (!client.Player.GameServer.IsWhitelisted(client.Player.AccountId) || !client.Player.IsAdmin)
                {
                    client.Player.SendError("Admin Only, you need to be Whitelisted to use this.");
                    return false;
                }
            }
            return true;
        }
    }

    public static class MessageHandlers
    {
        private static Dictionary<MessageId, IMessageHandler> Handlers;

        static MessageHandlers()
        {
            Handlers = new Dictionary<MessageId, IMessageHandler>();
            foreach (var type in Assembly.GetAssembly(typeof(IMessageHandler)).GetTypes().Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(IMessageHandler))))
            {
                var handler = (IMessageHandler)Activator.CreateInstance(type);
                Handlers.Add(handler.MessageId, handler);
            }
        }

        public static IMessageHandler GetHandler(MessageId messageId) => Handlers.TryGetValue(messageId, out var ret) ? ret : null;
    }
}   
