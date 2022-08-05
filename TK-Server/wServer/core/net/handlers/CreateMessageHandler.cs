using common;
using common.database;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.core.net.handlers;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public sealed class CreateMessageHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.CREATE;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var classType = rdr.ReadUInt16();
            var skinType = rdr.ReadUInt16();

            if (client.State != ProtocolState.Handshaked)
                return;

            var status = client.GameServer.Database.CreateCharacter(client.Account, classType, skinType, out DbChar character);

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
        private void CreatePlayer(Client client, DbChar character)
        {
            client.Character = character;

            var target = client.GameServer.WorldManager.GetWorld(client.TargetWorld);
            if (target == null)
                target = client.GameServer.WorldManager.GetWorld(-2); // return to nexus

            client.Player = new Player(client);

            client.SendPacket(new CreateSuccess()
            {
                CharId = client.Character.CharId,
                ObjectId = target.EnterWorld(client.Player)
            }, PacketPriority.High);
            client.State = ProtocolState.Ready;
            client.GameServer.ConnectionManager.ClientConnected(client);
            //client.Player?.PlayerUpdate.SendUpdate();
        }
    }
}
