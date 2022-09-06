using Anna.Request;
using common;
using common.isc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;

namespace server.app
{
    internal class serverList : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var servers = Program.GetServerList().Select(_ => _.ToXml()).ToList();
            if (servers.Any())
            {
                var response = new XElement("Servers", new XElement("Servers", servers)).ToString();
                Write(context, response);
                return;
            }

            Write(context, "<Error>No Servers</Error>");
        }
    }
}