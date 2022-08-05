using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;

namespace common.database
{
    public enum PartySizes
    {
        NPlayer = 3,
        D1 = 4,
        D2 = 5,
        D3 = 6,
        D4 = 7,
        D5 = 8,
        D6 = 9
    }

    public class DbPartySystem
    {
        private IDatabase _db;

        public DbPartySystem()
        { }

        public DbPartySystem(IDatabase db, int id)
        {
            _db = db;

            PartyId = id;

            var json = (string)db.HashGet("party", id);

            if (json != null)
                JsonConvert.PopulateObject(json, this);
        }

        public int PartyId { get; set; }
        public (string, int) PartyLeader { get; set; }
        public List<DbMemberData> PartyMembers { get; set; }
        public int WorldId { get; set; }

        public static DbPartySystem Get(IDatabase db, int partyId)
        {
            if (partyId == 0)
                return null;

            var allParties = db.HashGet("party", partyId);

            if (allParties.IsNullOrEmpty)
                return null;

            var l = JsonConvert.DeserializeObject<DbPartySystem>(allParties);

            if (l.PartyId == partyId)
                return l;
            else
                return null;
        }

        public static int NextId(IDatabase db)
        {
            int id = 0;

            do id++;
            while ((string)db.HashGet("party", id) != null);

            return id;
        }

        public static int ReturnSize(int rank)
        {
            switch (rank)
            {
                case 0:
                    return (int)PartySizes.NPlayer;

                case 10:
                    return (int)PartySizes.D1;

                case 20:
                    return (int)PartySizes.D2;

                case 30:
                    return (int)PartySizes.D3;

                case 40:
                    return (int)PartySizes.D4;

                case 50:
                    return (int)PartySizes.D5;

                case 60:
                    return (int)PartySizes.D6;

                default:
                    return (int)PartySizes.NPlayer;
            }
        }

        public void Flush() => _db.HashSet("party", PartyId, JsonConvert.SerializeObject(this));

        public bool LeaderIsVip(Database db)
        {
            var leaderAcc = db.GetAccount(PartyLeader.Item2);
            if (leaderAcc == null)
                return false;

            if (leaderAcc.Rank >= 60)
            {
                var members = new HashSet<DbAccount>();
                foreach (var member in PartyMembers)
                    members.Add(db.GetAccount(member.accid));
                db.RemoveParty(leaderAcc, members, PartyId);
                return false;
            }

            return true;
        }

        public int ReturnWorldId() => WorldId;
    }
}
