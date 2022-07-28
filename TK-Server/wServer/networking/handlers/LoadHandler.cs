using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class LoadHandler : PacketHandlerBase<Load>
    {
        public override PacketId ID => PacketId.LOAD;

        protected override void HandlePacket(Client client, Load packet) 
        {
            if (client.State != ProtocolState.Handshaked)
                return;

            var target = client.CoreServerManager.WorldManager.GetWorld(client.TargetWorld);

            if (target == null)
            {
                client.SendFailure($"Unable to find world: {client.TargetWorld}", Failure.MessageWithDisconnect);
                return;
            }

            client.Character = client.CoreServerManager.Database.LoadCharacter(client.Account, packet.CharId);

            if (client.Character == null)
                client.SendFailure("Failed to load character", Failure.MessageWithDisconnect);
            else if (client.Character.Dead)
                client.SendFailure("Character is dead", Failure.MessageWithDisconnect);
            else
            {
                client.Player = new Player(client);
                client.SendPacket(new CreateSuccess()
                {
                    CharId = client.Character.CharId,
                    ObjectId = target.EnterWorld(client.Player)
                }, PacketPriority.High);
                client.State = ProtocolState.Ready;
                client.CoreServerManager.ConnectionManager.ClientConnected(client);
                //client.Player?.PlayerUpdate.SendUpdate();
            }
        }
    }
}
