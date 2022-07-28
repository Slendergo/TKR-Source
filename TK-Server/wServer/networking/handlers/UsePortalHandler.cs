using System.Linq;
using System.Threading.Tasks;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class UsePortalHandler : PacketHandlerBase<UsePortal>
    {
        private readonly int[] _realmPortals = new int[] { 0x0704, 0x070e, 0x071c, 0x703, 0x070d, 0x0d40 };

        public override PacketId ID => PacketId.USEPORTAL;

        protected override void HandlePacket(Client client, UsePortal packet) => Handle(client, packet);

        private void Handle(Client client, UsePortal packet)
        {
            var player = client.Player;
            if (player?.Owner == null || IsTest(client))
                return;

            var entity = player.Owner.GetEntity(packet.ObjectId);
            if (entity == null) return;

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

            if (portal.ObjectType == 0x072f)
            {
                var proto = player.CoreServerManager.Resources.Worlds["GuildHall"];
                var world = player.CoreServerManager.WorldManager.GetWorld(proto.id);
                player.Reconnect(world.GetInstance(player.Client));
                return;
            }

            player.SendInfo("Portal not implemented.");
        }

        private void HandlePortal(Player player, Portal portal)
        {
            if (portal == null || !portal.Usable)
                return;

            using (TimedLock.Lock(portal.CreateWorldLock))
            {
                var world = portal.WorldInstance;

                // special portal case lookup
                if (world == null && _realmPortals.Contains(portal.ObjectType))
                {
                    world = player.CoreServerManager.WorldManager.GetRandomGameWorld();
                    if (world == null)
                        return;
                }

                if (world is Realm && !player.CoreServerManager.Resources.GameData.ObjectTypeToId[portal.ObjectDesc.ObjectType].Contains("Cowardice"))
                {
                    player.FameCounter.CompleteDungeon(player.Owner.Name);
                }

                if (world != null)
                {
                    if (world.IsPlayersMax())
                    {
                        player.SendError("Dungeon is full.");
                        return;
                    }

                    player.Reconnect(world);
                    return;
                }

                // dynamic case lookup
                if (portal.CreateWorldTask == null || portal.CreateWorldTask.IsCompleted)
                    portal.CreateWorldTask = Task.Factory
                        .StartNew(() => portal.CreateWorld(player))
                        .ContinueWith(e =>
                            Log.Error(e.Exception.InnerException.ToString()),
                            TaskContinuationOptions.OnlyOnFaulted);

                portal.WorldInstanceSet += player.Reconnect;
            }
        }
    }
}
