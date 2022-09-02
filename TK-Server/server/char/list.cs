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
        internal static ServerConfig Config;
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {

            if (query["secret"] != "69420")
            {
                System.Console.WriteLine(query["guid"] + " Tried to get charList without secret");
                Write(context, "<Error>Internal Server Error</Error>");
            }
            var status = _db.Verify(query["guid"], query["password"], out var acc);

            if (status == DbLoginStatus.OK || status == DbLoginStatus.AccountNotExists)
            {
                if (status == DbLoginStatus.AccountNotExists)
                    acc = _db.CreateGuestAccount(query["guid"]);

                var list = CharList.FromDb(_db, acc);
                list.Servers = GetServerList();
                WriteXml(context, list.ToXml().ToString());
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }

        internal static List<ServerItem> GetServerList()
        {
            var ret = new List<ServerItem>();
            foreach (var server in Program.ISManager.GetServerList().ToList())
            {
                if (server.type != ServerType.World)
                    continue;

                ret.Add(new ServerItem()
                {
                    Name = server.name,
                    Lat = server.latitude,
                    Long = server.longitude,
                    Port = server.port,
                    DNS = server.address,
                    Usage = server.players / (double)server.maxPlayers,
                    AdminOnly = server.adminOnly,
                    UsageText = server.IsJustStarted() ? "- NEW!" : $"{server.players}/{server.maxPlayers}"
                });
            }
            ret = ret.OrderBy(_ => _.Port).ToList();
            return ret;
        }
    }
}
