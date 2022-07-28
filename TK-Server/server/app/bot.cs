using Anna.Request;
using common.isc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;

namespace server.app
{
    internal class bot : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var token = query["token"];
            var cmd = query["cmd"];

            if (!string.IsNullOrWhiteSpace(token) && Program.Config.discordIntegration.botToken.Equals(token))
            {
                var response = string.Empty;

                switch (cmd)
                {
                    default: response = "Command not added on AppEngine"; break;
                    case "list": HandleServerXmls(out response); break;
                    case "restart": HandleRestart(query["user"], query["server"], out response); break;
                    case "announce": HandleAnnounce(query["user"], query["message"], out response); break;
                }

                Write(context, response);
                return;
            }

            Write(context, "Internal server error");
        }

        private IEnumerable<XElement> GetServersXmls()
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
                    Usage = server.IsJustStarted() ? 0 : server.players / (double)server.maxPlayers,
                    AdminOnly = server.adminOnly,
                    UsageText = server.IsJustStarted() ? "0" : $"{server.players}/{server.maxPlayers}"
                });
            }
            ret = ret.OrderBy(_ => _.Port).ToList();

            for (var i = 0; i < ret.Count; i++)
                yield return ret[i].ToXml();
        }

        private void HandleAnnounce(string user, string message, out string response)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                response = "User is empty.";
                return;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                response = "Message is empty.";
                return;
            }

            response = Program.ISManager.AnnounceInstance(user, message);
        }

        private void HandleRestart(string user, string server, out string response)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                response = "User is empty.";
                return;
            }

            if (string.IsNullOrWhiteSpace(server))
            {
                response = "Server name is empty.";
                return;
            }

            response = Program.ISManager.RestartInstance(server, user);
        }

        private void HandleServerXmls(out string response) => response = new XElement("Servers", GetServersXmls()).ToString();
    }
}
