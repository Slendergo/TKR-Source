using Anna.Request;
using common.database;
using common.utils;
using System.Collections.Specialized;
using System.Web;

namespace server.guild
{
    internal class setBoard : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = _db.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == DbLoginStatus.OK)
            {
                if (acc.GuildId <= 0 || acc.GuildRank < 20)
                {
                    Write(context, "<Error>No permission</Error>");
                    return;
                }

                var guild = _db.GetGuild(acc.GuildId);
                var text = HttpUtility.UrlDecode(query["board"]);
                if (_db.SetGuildBoard(guild, text))
                {
                    Write(context, text);
                    return;
                }

                Write(context, "<Error>Failed to set board</Error>");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
