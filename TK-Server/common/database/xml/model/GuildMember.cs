using common.database.model;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct GuildMember
    {
        private int _guildFame;
        private int _lastSeen;
        private string _name;
        private int _rank;

        public static GuildMember Serialize(AccountModel model) => new GuildMember()
        {
            _name = model.Name,
            _rank = (byte)model.GuildRank,
            _guildFame = model.GuildFame,
            _lastSeen = model.LastSeen
        };

        public XElement ToXml() => new XElement("Member",
            new XElement("Name", _name),
            new XElement("Rank", _rank),
            new XElement("Fame", _guildFame),
            new XElement("LastSeen", _lastSeen)
        );
    }
}
