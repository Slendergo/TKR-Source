using common.database;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class CreateHandler : PacketHandlerBase<Create>
    {
        public override PacketId ID => PacketId.CREATE;

        protected override void HandlePacket(Client client, Create packet) => Handle(client, packet);

        private void CreatePlayer(Client client, DbChar character)
        {
            client.Character = character;

            var target = client.CoreServerManager.WorldManager.Worlds[client.TargetWorld];

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

        private void Handle(Client client, Create packet)
        {
            if (client.State != ProtocolState.Handshaked)
                return;

            var status = client.CoreServerManager.Database.CreateCharacter(client.Account, packet.ClassType, packet.SkinType, out DbChar character);

            if (status == DbCreateStatus.ReachCharLimit)
            {
                client.SendFailure("Too many characters",
                    Failure.MessageWithDisconnect);
                return;
            }

            if (status == DbCreateStatus.SkinUnavailable)
            {
                client.SendFailure("Skin unavailable",
                    Failure.MessageWithDisconnect);
                return;
            }

            if (status == DbCreateStatus.Locked)
            {
                client.SendFailure("Class locked",
                    Failure.MessageWithDisconnect);
                return;
            }

            CreatePlayer(client, character);
        }
    }
}
