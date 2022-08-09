using common;
using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public class LoadHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.LOAD;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var charId = rdr.ReadInt32();

            if (client.State != ProtocolState.Handshaked)
                return;

            var target = client.GameServer.WorldManager.GetWorld(client.TargetWorld);

            if (target == null)
            {
                client.SendFailure($"Unable to find world: {client.TargetWorld}", FailureMessage.MessageWithDisconnect);
                return;
            }

            client.Character = client.GameServer.Database.LoadCharacter(client.Account, charId);

            if (client.Character == null)
                client.SendFailure("Failed to load character", FailureMessage.MessageWithDisconnect);
            else if (client.Character.Dead)
                client.SendFailure("Character is dead", FailureMessage.MessageWithDisconnect);
            else
            {
                client.Player = new Player(client);
                client.SendPacket(new CreateSuccess()
                {
                    CharId = client.Character.CharId,
                    ObjectId = target.EnterWorld(client.Player)
                });
                client.State = ProtocolState.Ready;
                client.GameServer.ConnectionManager.ClientConnected(client);
                //client.Player?.PlayerUpdate.SendUpdate();
            }
        }
    }
}
