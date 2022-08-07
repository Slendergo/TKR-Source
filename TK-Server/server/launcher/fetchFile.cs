using Anna.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace server.launcher
{
    internal class fetchFile : RequestHandler
    {
        public sealed class LauncherInfo
        {
            public string Version { get; set; }
            public List<LauncherChecksum> CheckSum { get; set; }
        }

        public sealed class LauncherChecksum
        {
            public string CheckSum { get; set; }
            public string Path { get; set; }
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var checksum = query["checksum"];

            var launcherInfoPath = $"{Environment.CurrentDirectory}\\Deployment\\LauncherInfo.json";
            if (!File.Exists(launcherInfoPath))
            {
                Write(context, "<Error>Unexpected Error</Error>");
                return;
            }

            var launcherInfo = JsonConvert.DeserializeObject<LauncherInfo>(File.ReadAllText(launcherInfoPath));

            foreach(var check in launcherInfo.CheckSum)
                if(check.CheckSum == checksum)
                {
                    var path = $"{Environment.CurrentDirectory}\\Deployment\\{check.Path}";
                    Console.WriteLine(path);
                    Write(context, File.ReadAllBytes(path));
                    return;
                }

            Write(context, "<Error>Unexpected Error</Error>");
        }
    }
}
