using common;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class UsePortalHandler : IMessageHandler
    {
        private readonly int[] _realmPortals = new int[] { 0x0704, 0x070e, 0x071c, 0x703, 0x070d, 0x0d40 };

        public override MessageId MessageId => MessageId.USEPORTAL;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var objectId = rdr.ReadInt32();
            var player = client.Player;
            if (player?.World == null || client?.Player?.World is TestWorld)
                return;

            var entity = player.World.GetEntity(objectId);
            if (entity == null)
                return;

            if (entity is GuildHallPortal)
            {
                HandleGuildPortal(player, entity as GuildHallPortal);
                return;
            }

            HandlePortal(player, entity as Portal);
        }

        private void HandleGuildPortal(Player player, GuildHallPortal portal)
        {
            if (string.IsNullOrEmpty(player.Guild))
            {
                player.SendError("You are not in a guild.");
                return;
            }

            var manager = player.Client.GameServer;
            var guildId = player.Client.Account.GuildId;

            var world = manager.WorldManager.GetGuild(guildId);
            if (world == null)
            {
                var guild = player.Client.GameServer.Database.GetGuild(guildId);

                // this is mandatory
                var dungeonName = $"{portal.PortalDescr.DungeonName} {guild.Level + 1}";

                world = manager.WorldManager.CreateNewWorld(dungeonName, null, player.World);
                if (world != null)
                    manager.WorldManager.AddGuildInstance(guildId, world);
            }

            if (world != null)
                player.Reconnect(world);
            else
                player.SendInfo("[Bug] Unable to Create Guild.");
        }

        private void HandlePortal(Player player, Portal portal)
        {
            if (!portal.Usable)
            {
                player.SendInfo("Portal is unusable!");
                return;
            }

            var world = portal.WorldInstance;
            if (world == null && _realmPortals.Contains(portal.ObjectType))
            {
                var nextWorldInChain = player.World.ParentWorld;
                if (nextWorldInChain == null)
                {
                    System.Console.WriteLine("NULL");
                    // random realm?
                    return;
                }
                player.Reconnect(nextWorldInChain);
                return;
            }

            if (world != null)
            {
                if (world.IsPlayersMax())
                {
                    player.SendError("Dungeon is full.");
                    return;
                }

                if (world is RealmWorld && !player.GameServer.Resources.GameData.ObjectTypeToId[portal.ObjectDesc.ObjectType].Contains("Cowardice"))
                    player.FameCounter.CompleteDungeon(player.World.IdName);

                player.Reconnect(world);
                return;
            }

            var dungeonName = portal.PortalDescr.DungeonName;

            world = portal.GameServer.WorldManager.CreateNewWorld(dungeonName, null, player.World);
            if (world == null)
            {
                player.SendError($"[Bug] Unable to create: {dungeonName}");
                return;
            }

            if (world.InstanceType == common.resources.WorldResourceInstanceType.Vault)
                (world as VaultWorld).SetClient(player.Client);
            else
                portal.WorldInstance = world;
            player.Reconnect(world);
        }
    }
}
