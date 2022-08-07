using System.Collections.Generic;

namespace Launcher
{
    public sealed class LauncherInfo
    {
        public string Version { get; set; }
        public string LauncherPath { get; set; }
        public List<LauncherChecksum> CheckSum { get; set; }
    }

    public sealed class LauncherChecksum
    {
        public string CheckSum { get; set; }
        public string Path { get; set; }
    }
}
