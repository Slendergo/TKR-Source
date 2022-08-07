using Anna.Request;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace server.launcher
{
    internal class getVersion : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var version = new XElement("LauncherInfo", new XElement("Version", Program.Config.serverSettings.version));
            Write(context, version.ToString());
        }
    }
}
