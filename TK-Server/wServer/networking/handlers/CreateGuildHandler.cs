using common.database;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class CreateGuildHandler : PacketHandlerBase<CreateGuild>
    {
        public override PacketId ID => PacketId.CREATEGUILD;

        protected override void HandlePacket(Client client, CreateGuild packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet.Name));
            Handle(client, packet.Name);
        }

        private void Handle(Client client, string name)
        {
            if (client.Player == null || IsTest(client))
                return;

            var acc = client.Account;

            acc.Reload("fame");
            acc.Reload("totalFame");

            if (acc.Fame < 1000)
            {
                SendError(client, "Insufficient funds");
                return;
            }

            if (!acc.NameChosen)
            {
                SendError(client, "Must pick a character name\nbefore creating a guild");
                return;
            }

            if (acc.GuildId > 0)
            {
                SendError(client, "Already in a guild");
                return;
            }

            var guildResult = client.CoreServerManager.Database.CreateGuild(name, out DbGuild guild);

            if (guildResult != DbGuildCreateStatus.OK)
            {
                SendError(client, guildResult.ToString());
                return;
            }

            var addResult = client.CoreServerManager.Database.AddGuildMember(guild, acc, true);

            if (addResult != DbAddGuildMemberStatus.OK)
            {
                SendError(client, addResult.ToString());
                return;
            }

            client.CoreServerManager.Database.UpdateFame(acc, -1000);
            client.Player.CurrentFame = acc.Fame;
            client.Player.Guild = guild.Name;
            client.Player.GuildRank = 40;

            SendSuccess(client);
        }

        private void SendError(Client client, string message = null) => client.SendPacket(new GuildResult()
        {
            Success = false,
            ErrorText = "Guild Creation Error: " + message
        });

        private void SendSuccess(Client client) => client.SendPacket(new GuildResult()
        {
            Success = true,
            ErrorText = "Success!"
        });
    }
}
