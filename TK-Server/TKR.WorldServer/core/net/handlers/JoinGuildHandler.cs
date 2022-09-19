using System;
using TKR.Shared;
using TKR.Shared.database.guild;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    internal class JoinGuildHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.JOINGUILD;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var guildName = rdr.ReadUTF();
            if (client.Player == null || client?.Player?.World is TestWorld)
                return;

            if (client.Player.GuildInvite == null)
            {
                client.Player.SendError("You have not been invited to a guild.");
                return;
            }

            var guild = client.GameServer.Database.GetGuild((int)client.Player.GuildInvite);

            if (guild == null)
            {
                client.Player.SendError("Internal server error.");
                return;
            }

            if (!guild.Name.Equals(guildName, StringComparison.InvariantCultureIgnoreCase))
            {
                client.Player.SendError("You have not been invited to join " + guildName + ".");
                return;
            }

            var result = client.GameServer.Database.AddGuildMember(guild, client.Account);
            if (result != DbAddGuildMemberStatus.OK)
            {
                client.Player.SendError("Could not join guild. (" + result + ")");
                return;
            }

            client.Player.Guild = guild.Name;
            client.Player.GuildRank = 0;
            client.GameServer.ChatManager.Guild(client.Player, client.Player.Name + " has joined the guild!");
        }
    }
}