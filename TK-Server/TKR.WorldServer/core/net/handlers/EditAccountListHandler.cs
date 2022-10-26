using TKR.Shared;
using TKR.WorldServer.core.worlds.impl;
using TKR.WorldServer.networking;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.net.handlers
{
    public class EditAccountListHandler : IMessageHandler
    {
        private const int LockAction = 0;
        private const int IgnoreAction = 1;

        public override MessageId MessageId => MessageId.EDITACCOUNTLIST;

        public override void Handle(Client client, NetworkReader rdr, ref TickTime time)
        {
            var accountListId = rdr.ReadInt32();
            var add = rdr.ReadBoolean();
            var objectId = rdr.ReadInt32();

            if (client.Player == null || client?.Player?.World is TestWorld)
                return;

            if (!(client.Player.World.GetEntity(objectId) is Player targetPlayer) || targetPlayer.Client.Account == null)
            {
                client.Player.SendError("Player not found.");
                return;
            }

            if (accountListId == LockAction)
            {
                client.GameServer.Database.LockAccount(client.Account, targetPlayer.Client.Account, add);
                return;
            }

            if (accountListId == IgnoreAction)
            {
                client.GameServer.Database.IgnoreAccount(client.Account, targetPlayer.Client.Account, add);
                return;
            }
        }
    }
}
