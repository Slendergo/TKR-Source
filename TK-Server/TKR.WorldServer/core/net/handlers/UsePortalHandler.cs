using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class UsePortalHandler : IMessageHandler
    {
        private const string PORTAL_TO_NEXUS = "Portal To Nexus";
        private const string TOMB_PORTAL_OF_COWARDICE = "Tomb Portal of Cowardice";
        private const string PORTAL_OF_COWARDICE = "Portal of Cowardice";
        private const string GLOWING_PORTAL_OF_COWARDICE = "Glowing Portal of Cowardice";
        private const string RANDOM_REALM_PORTAL = "Random Realm Portal";
        private const string REALM_PORTAL = "Realm Portal";
        private const string GLOWING_REALM_PORTAL = "Glowing Realm Portal";

        public override MessageId MessageId => MessageId.USEPORTAL;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var player = client.Player;
            if (player == null || player?.World == null || client?.Player?.World is TestWorld)
                return;

            var objectId = rdr.ReadInt32();

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
            if (world == null)
            {
                switch (portal.ObjectDesc.ObjectId)
                {
                    case PORTAL_TO_NEXUS:
                        {
                            world = player.GameServer.WorldManager.Nexus;
                            player.Reconnect(world);
                        }
                        break;
                    case TOMB_PORTAL_OF_COWARDICE:
                    case PORTAL_OF_COWARDICE:
                    case GLOWING_PORTAL_OF_COWARDICE:
                    case RANDOM_REALM_PORTAL:
                    case REALM_PORTAL:
                    case GLOWING_REALM_PORTAL:
                        {
                            world = player.GameServer.WorldManager.GetRandomRealm();
                            if (world == null)
                                world = player.GameServer.WorldManager.Nexus;
                            player.Reconnect(world);
                        }
                        break;
                }

                if (world != null)
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

            if (world.IdName == "Trial of Souls")
            {
                if (player.Client.Character.CompletedTrialOfSouls)
                {
                    player.SendError($"You have already completed the trial of souls");
                    return;
                }

                if (player.GetMaxedStats() != 8)
                {
                    player.SendError($"You must be 8/8 to enter this dungeon");
                    return;
                }
            }

            if (world.InstanceType == WorldResourceInstanceType.Vault)
                (world as VaultWorld).SetOwner(player.AccountId);
            else if (!world.CreateInstance)
                portal.WorldInstance = world;
            player.Reconnect(world);

            if (player.Pet != null)
                player.World.LeaveWorld(player.Pet);
        }
    }
}
