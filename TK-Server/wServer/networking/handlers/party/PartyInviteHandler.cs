using CA.Extensions.Concurrent;
using common.database;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers.party
{
    internal class PartyInviteHandler : PacketHandlerBase<PartyInvite>
    {
        public override PacketId ID => PacketId.PARTY_INVITE;

        protected override void HandlePacket(Client client, PartyInvite packet)
        {
            if (client == null || IsTest(client) || client.Player == null || client.Player.World == null)
                return;

            //client.Player.SendInfo("This feature is temporarily disabled until further notice.");
            Handle(client, packet);
        }

        private void Handle(Client client, PartyInvite packet)
        {
            var db = client.Account.Database;

            client.Account.Reload("partyId");

            var party = DbPartySystem.Get(db, client.Account.PartyId);

            if (party != null && !party.LeaderIsVip(Program.CoreServerManager.Database) || !client.Player.CanUseThisFeature(core.objects.Player.GenericRank.VIP))
            {
                client.Player.SendError("<Error> VIPs cannot be the Leader of a Party.");
                return;
            }

            if (party == null)
            {
                var nextId = DbPartySystem.NextId(db);

                party = new DbPartySystem(db, nextId)
                {
                    PartyId = nextId,
                    PartyLeader = (client.Account.Name, client.Account.AccountId),
                    PartyMembers = new List<DbMemberData>(DbPartySystem.ReturnSize(client.Account.Rank))
                };
                party.Flush();

                client.Account.PartyId = party.PartyId;
                client.Account.FlushAsync();
                client.Account.Reload("partyId");
                client.Player?.SendInfo("Party Created Successfully!");
            }

            var target = client.CoreServerManager.ConnectionManager.Clients
                .KeyWhereAsParallel(_ => _ != null && _.Account != null && _.Account.Name.Equals(packet.Name))
                .FirstOrDefault();
            if (target == null || target.Account == null || target.Player == null)
            {
                client.Player?.SendError("Player not found.");
                return;
            }

            if (!target.Player.NameChosen)
            {
                client.Player.SendError("Player needs to choose a name first.");
                return;
            }

            target.Account.Reload("partyId");

            if (target.Account.PartyId == client.Account.PartyId)
            {
                client.Player.SendError("He's already in your Party!");
                client.Account.Reload("partyId");
                return;
            }

            if (target.Account.PartyId != 0)
            {
                client.Player.SendError("He is already in a Party!");
                client.Account.Reload("partyId");
                return;
            }

            target.SendPacket(new InvitedToParty()
            {
                Name = client.Account.Name,
                PartyId = client.Account.PartyId
            });

            client.Player.SendInfo($"Invited {target.Account.Name} to your Party!");
        }
    }
}
