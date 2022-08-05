using common;
using wServer.core;
using wServer.core.net.handlers;
using wServer.core.objects;
using wServer.core.worlds.logic;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class EditAccountListHandler : IMessageHandler
    {
        private const int LockAction = 0;
        private const int IgnoreAction = 1;

        public override PacketId MessageId => PacketId.EDITACCOUNTLIST;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
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
