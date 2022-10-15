using TKR.Shared;
using TKR.Shared.database;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking.packets;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.Shared.database.character;
using System;

namespace TKR.WorldServer.core.net.handlers
{
    public sealed class CreateMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.CREATE;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime time)
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


            var x = 0;
            var y = 0;

            var spawnRegions = target.GetSpawnPoints();
            if (spawnRegions.Length > 0)
            {
                var sRegion = Random.Shared.NextLength(spawnRegions);
                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }

            var player = client.Player = target.CreateNewPlayer(client, x, y);

            client.SendPacket(new CreateSuccess()
            {
                CharId = client.Character.CharId,
                ObjectId = player.Id
            });

            if (target is RealmWorld realm)
                realm.KingdomManager.OnPlayerEntered(player);

            client.State = ProtocolState.Ready;
            client.GameServer.ConnectionManager.ClientConnected(client);

            //client.Player?.PlayerUpdate.SendUpdate();
        }
    }
}
