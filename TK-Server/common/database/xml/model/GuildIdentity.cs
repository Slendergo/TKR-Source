using common.database.model;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct GuildIdentity
    {
        private int _id;
        private string _name;
        private int _rank;

        public static GuildIdentity Serialize(AccountModel account, GuildModel guild) => new GuildIdentity()
        {
            _id = guild.Id,
            _name = guild.Name,
            _rank = (byte)account.GuildRank
        };

        public XElement ToXml() => new XElement("Guild",
            new XAttribute("id", _id),
            new XElement("Name", _name),
            new XElement("Rank", _rank)
        );
    }
}
