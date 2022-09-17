using Anna.Request;
using common;
using common.database;
using common.utils;
using System.Collections.Specialized;

namespace server.account
{
    internal class verify : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            if (Program.Config.serverInfo.requireSecret && query["secret"] != "69420")
            {
                System.Console.WriteLine(query["guid"] + " Tried to login without secret");
                Write(context, "<Error>Internal Server Error</Error>");
                return;
            }

            var status = _db.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == DbLoginStatus.OK)
                Write(context, Account.FromDb(acc).ToXml().ToString());
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
