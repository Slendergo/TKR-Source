using Anna.Request;
using common.database;
using common.utils;
using System.Collections.Specialized;

namespace server.guild
{
    internal class listMembers : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = _db.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == DbLoginStatus.OK)
            {
                if (acc.GuildId <= 0)
                {
                    Write(context, "<Error>Not in guild</Error>");
                    return;
                }

                var guild = _db.GetGuild(acc.GuildId);
                WriteXml(context, Guild.FromDb(_db, guild).ToXml().ToString());
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
