using common.database.model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct Guild
    {
        private int _currentFame;
        private string _hallType;
        private int _id;
        private List<GuildMember> _members;
        private string _name;
        private int _totalFame;

        public static async Task<Guild> SerializeAsync(RedisDb redis, GuildModel model)
        {
            var accounts = new AccountModel[model.Members.Length];

            for (var i = 0; i < accounts.Length; i++)
            {
                var account = new AccountModel();

                await account.InitAsync(redis.Db, model.Members[i].ToString());

                accounts[i] = account;
            }

            var members = (
                from account in accounts
                where account.EntryExist
                orderby
                    (byte)account.GuildRank descending,
                    account.GuildFame descending,
                    account.Name ascending
                select GuildMember.Serialize(account)
            ).ToList();

            return new Guild()
            {
                _id = model.Id,
                _name = model.Name,
                _currentFame = model.Fame,
                _totalFame = model.FameHistory,
                _hallType = "Guild Hall " + model.Level,
                _members = members
            };
        }

        public XElement ToXml()
        {
            var guild = new XElement("Guild");
            guild.Add(new XAttribute("id", _id));
            guild.Add(new XAttribute("name", _name));
            guild.Add(new XElement("TotalFame", _totalFame));
            guild.Add(new XElement("CurrentFame", _currentFame));
            guild.Add(new XElement("HallType", _hallType));

            foreach (var member in _members)
                guild.Add(member.ToXml());

            return guild;
        }
    }
}
