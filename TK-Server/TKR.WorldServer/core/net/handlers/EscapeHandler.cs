using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.net.handlers
{
    public class EscapeHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.ESCAPE;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime tickTime)
        {
            if (client.Player == null || client.Player.World == null)
                return;

            var map = client.Player.World;
            if (map.Id == World.NEXUS_ID)
            {
                client.Disconnect("Already in Nexus!");
                return;
            }

            client.Player.SendInfo("You issued a nexus, if you die its because you dont see this");
            client.Player.ApplyPermanentConditionEffect(ConditionEffectIndex.Invincible);
            client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = client.GameServer.Configuration.serverInfo.port,
                GameId = World.NEXUS_ID,
                Name = "Nexus"
            });
        }
    }
}
