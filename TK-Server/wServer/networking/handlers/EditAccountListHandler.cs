using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class EditAccountListHandler : PacketHandlerBase<EditAccountList>
    {
        private const int LockAction = 0;
        private const int IgnoreAction = 1;

        public override PacketId ID => PacketId.EDITACCOUNTLIST;

        protected override void HandlePacket(Client client, EditAccountList packet) => Handle(client, packet.AccountListId, packet.ObjectId, packet.Add);

        private void Handle(Client client, int action, int objId, bool add)
        {
            if (client.Player == null || IsTest(client))
                return;

            if (!(client.Player.Owner.GetEntity(objId) is Player targetPlayer) || targetPlayer.Client.Account == null)
            {
                client.Player.SendError("Player not found.");
                return;
            }

            if (action == LockAction)
            {
                client.CoreServerManager.Database.LockAccount(client.Account, targetPlayer.Client.Account, add);
                return;
            }

            if (action == IgnoreAction)
            {
                client.CoreServerManager.Database.IgnoreAccount(client.Account, targetPlayer.Client.Account, add);
                return;
            }
        }
    }
}
