using common.database;
using wServer.core;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers.party
{
    internal class JoinPartyHandler : PacketHandlerBase<JoinParty>
    {
        public override PacketId ID => PacketId.JOIN_PARTY;

        protected override void HandlePacket(Client client, JoinParty packet, ref TickTime time)
        {
            if (client == null || IsTest(client) || client.Player == null || client.Player.World == null)
                return;

            //client.Player.SendInfo("This feature is temporarily disabled until further notice.");
            Handle(client, packet);
        }

        private void Handle(Client client, JoinParty packet)
        {
            var db = client.Account.Database;

            client.Account.Reload("partyId");

            var party = DbPartySystem.Get(db, packet.PartyId);

            if (party == null)
            {
                client.Player.SendError("Party doesn't exists.");
                return;
            }

            if (!party.LeaderIsVip(Program.CoreServerManager.Database))
            {
                client.Player.SendError("<Error> VIPs cannot be the Leader of a Party.");
                return;
            }

            if (client.Account.PartyId != 0)
            {
                client.Player.SendError("You're already in a Party.");
                client.Account.Reload("partyId");
                return;
            }

            if (packet.PartyLeader != null)
                if (party.PartyLeader.Item1 != packet.PartyLeader)
                {
                    client.Player.SendError("Unexpected error.");
                    return;
                }

            client.CoreServerManager.Database.AddMemberToParty(db, client.Account.Name, client.Account.AccountId, party.PartyId);
            client.Account.PartyId = party.PartyId;
            client.Account.FlushAsync();
            client.Account.Reload("partyId");
            client.CoreServerManager.ChatManager.Party(client.Player, client.Player.Name + " has joined the Party!");
            client.Player?.SendInfo("Joined Successfully!");
        }
    }
}
