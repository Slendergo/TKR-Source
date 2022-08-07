using Anna.Request;
using System;
using System.Collections.Specialized;
using System.IO;

namespace server.launcher
{
    internal class checkSum : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var launcherInfoPath = $"{Environment.CurrentDirectory}\\Deployment\\LauncherInfo.json";
            if (!File.Exists(launcherInfoPath))
            {
                Write(context, "<Error>Unexpected Error</Error>");
                return;
            }

            var launcherInfo = File.ReadAllText(launcherInfoPath);
            Write(context, launcherInfo);
        }
    }
}
