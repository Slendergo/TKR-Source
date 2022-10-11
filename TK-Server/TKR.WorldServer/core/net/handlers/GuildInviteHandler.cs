using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.net.handlers
{
    internal class GuildInviteHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.GUILDINVITE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var name = rdr.ReadUTF();

            if (client.Player == null || client?.Player?.World is TestWorld)
                return;

            if (client.Account.GuildRank < 20)
            {
                client.Player.SendError("Insufficient privileges.");
                return;
            }

            foreach (var entry in client.GameServer.ConnectionManager.Clients)
            {
                var cli = entry.Key;
                if (cli.Player == null || cli.Account == null || !cli.Account.Name.Equals(name))
                    continue;

                if (!cli.Account.NameChosen)
                {
                    client.Player.SendError("Player needs to choose a name first.");
                    return;
                }

                if (cli.Account.GuildId > 0)
                {
                    client.Player.SendError("Player is already in a guild.");
                    return;
                }

                cli.Player.GuildInvite = client.Account.GuildId;
                cli.SendMessage(new InvitedToGuild()
                {
                    Name = client.Account.Name,
                    GuildName = client.Player.Guild
                });
                return;
            }

            client.Player.SendError("Could not find the player to invite.");
        }
    }
}
