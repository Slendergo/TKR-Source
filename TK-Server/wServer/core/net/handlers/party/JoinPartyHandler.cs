using common;
using common.database;
using wServer.core;
using wServer.core.net.handlers;
using wServer.core.worlds.logic;
using wServer.networking;

namespace wServer.core.net
{
    internal class JoinPartyHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.JOIN_PARTY;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var partyLeader = rdr.ReadUTF();
            var partyId = rdr.ReadInt32();

            if (client == null || client?.Player?.World is TestWorld || client.Player == null || client.Player.World == null)
                return;

            var db = client.Account.Database;

            client.Account.Reload("partyId");

            var party = DbPartySystem.Get(db, partyId);

            if (party == null)
            {
                client.Player.SendError("Party doesn't exists.");
                return;
            }

            if (client.Account.PartyId != 0)
            {
                client.Player.SendError("You're already in a Party.");
                client.Account.Reload("partyId");
                return;
            }

            if (partyLeader != null)
                if (party.PartyLeader.Item1 != partyLeader)
                {
                    client.Player.SendError("Unexpected error.");
                    return;
                }

            client.GameServer.Database.AddMemberToParty(db, client.Account.Name, client.Account.AccountId, party.PartyId);
            client.Account.PartyId = party.PartyId;
            client.Account.FlushAsync();
            client.Account.Reload("partyId");
            client.GameServer.ChatManager.Party(client.Player, client.Player.Name + " has joined the Party!");
            client.Player?.SendInfo("Joined Successfully!");
        }
    }
}
