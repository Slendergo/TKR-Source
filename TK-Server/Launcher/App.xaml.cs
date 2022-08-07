using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LauncherInfo LauncherInfo { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var path = $"{GetLauncherPath()}\\LauncherInfo.json";
            var fileInfo = new FileInfo(path);
            fileInfo.Directory.Create();
            if (File.Exists(path))
                LauncherInfo = JsonSerializer.Deserialize<LauncherInfo>(File.ReadAllText(path));
        }

        public static void SetLauncherInformation(LauncherInfo launcherInfo)
        {
            LauncherInfo = launcherInfo;

            var path = $"{GetLauncherPath()}\\LauncherInfo.json";
            File.WriteAllText(path, JsonSerializer.Serialize(launcherInfo));
        }

        public static string GetLauncherPath()
        {
            return Path.Combine(GetApplicationData(), "TK-Client\\Local Store\\Launcher");
        }

        public static string GetApplicationData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
    }
}
