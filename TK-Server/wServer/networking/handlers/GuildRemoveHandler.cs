using CA.Extensions.Concurrent;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class GuildRemoveHandler : PacketHandlerBase<GuildRemove>
    {
        public override PacketId ID => PacketId.GUILDREMOVE;

        protected override void HandlePacket(Client client, GuildRemove packet) => Handle(client, packet.Name);

        private void Handle(Client source, string name)
        {
            if (source.Player == null || IsTest(source))
                return;

            var srcPlayer = source.Player;
            var manager = source.CoreServerManager;

            // if resigning
            if (source.Account.Name.Equals(name))
            {
                // chat needs to be done before removal so we can use
                // srcPlayer as a source for guild info
                manager.ChatManager.Guild(srcPlayer, srcPlayer.Name + " has left the guild.");

                if (!manager.Database.RemoveFromGuild(source.Account))
                {
                    srcPlayer.SendError("Guild not found.");
                    return;
                }

                srcPlayer.Guild = "";
                srcPlayer.GuildRank = 0;

                return;
            }

            // get target account id
            var targetAccId = source.CoreServerManager.Database.ResolveId(name);
            if (targetAccId == 0)
            {
                source.Player.SendError("Player not found");
                return;
            }

            // find target player (if connected)
            var targetClient = source.CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _.Account != null && _.Account.AccountId == targetAccId)
                .FirstOrDefault();

            // try to remove connected member
            if (targetClient != null)
            {
                if (source.Account.GuildRank >= 20 &&
                    source.Account.GuildId == targetClient.Account.GuildId &&
                    source.Account.GuildRank > targetClient.Account.GuildRank)
                {
                    var targetPlayer = targetClient.Player;

                    if (!manager.Database.RemoveFromGuild(targetClient.Account))
                    {
                        srcPlayer.SendError("Guild not found.");
                        return;
                    }

                    targetPlayer.Guild = "";
                    targetPlayer.GuildRank = 0;

                    manager.ChatManager.Guild(srcPlayer, targetPlayer.Name + " has been kicked from the guild by " + srcPlayer.Name);
                    targetPlayer.SendInfo("You have been kicked from the guild.");
                    return;
                }

                srcPlayer.SendError("Can't remove member. Insufficient privileges.");
                return;
            }

            // try to remove member via database
            var targetAccount = manager.Database.GetAccount(targetAccId);

            if (source.Account.GuildRank >= 20 &&
                source.Account.GuildId == targetAccount.GuildId &&
                source.Account.GuildRank > targetAccount.GuildRank)
            {
                if (!manager.Database.RemoveFromGuild(targetAccount))
                {
                    srcPlayer.SendError("Guild not found.");
                    return;
                }

                manager.ChatManager.Guild(srcPlayer, targetAccount.Name + " has been kicked from the guild by " + srcPlayer.Name);
                manager.ChatManager.SendInfo(targetAccId, "You have been kicked from the guild.");
                return;
            }

            srcPlayer.SendError("Can't remove member. Insufficient privileges.");
        }
    }
}
