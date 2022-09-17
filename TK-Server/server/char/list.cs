using Anna.Request;
using common;
using common.database;
using common.isc;
using common.utils;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace server.@char
{
    internal class list : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            if (Program.Config.serverInfo.requireSecret && query["secret"] != "69420")
            {
                System.Console.WriteLine(query["guid"] + " Tried to get charList without secret");
                Write(context, "<Error>Internal Server Error</Error>");
                return;
            }

            var status = _db.Verify(query["guid"], query["password"], out var acc);
            if (status == DbLoginStatus.OK || status == DbLoginStatus.AccountNotExists)
            {
                if (status == DbLoginStatus.AccountNotExists)
                    acc = _db.CreateGuestAccount(query["guid"]);

                var list = CharList.FromDb(_db, acc);
                list.Servers = Program.GetServerList();
                WriteXml(context, list.ToXml().ToString());
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
