using TKR.Shared;
using TKR.Shared.database;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking.packets;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.Shared.database.character;

namespace TKR.WorldServer.core.net.handlers
{
    public sealed class CreateMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.CREATE;

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
                    FailureMessage.MessageWithDisconnect);
                return;
            }

            if (status == DbCreateStatus.SkinUnavailable)
            {
                client.SendFailure("Skin unavailable",
                    FailureMessage.MessageWithDisconnect);
                return;
            }

            if (status == DbCreateStatus.Locked)
            {
                client.SendFailure("Class locked",
                    FailureMessage.MessageWithDisconnect);
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
            });
            client.State = ProtocolState.Ready;
            client.GameServer.ConnectionManager.ClientConnected(client);

            //client.Player?.PlayerUpdate.SendUpdate();
        }
    }
}
