using common;
using common.database;
using wServer.core;
using wServer.core.net.handlers;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    internal class CreateGuildHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.CREATEGUILD;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var name = rdr.ReadUTF(); 
            
            if (client.Player == null || client?.Player?.World is TestWorld)
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
