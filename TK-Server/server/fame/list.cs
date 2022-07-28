using Anna.Request;
using common.database;
using System.Collections.Specialized;

namespace server.fame
{
    internal class list : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            DbChar character = null;
            if (query["accountId"] != null)
            {
                character = _db.LoadCharacter(
                    int.Parse(query["accountId"]),
                    int.Parse(query["charId"])
                );
            }
            var list = FameList.FromDb(_db, query["timespan"], character);
            Write(context, list.ToXml().ToString());
        }
    }
}
