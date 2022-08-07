using System.Security.Cryptography;

namespace server
{
    public sealed class LauncherInfo
    {
        public string Version { get; set; }

        public string LauncherPath { get; set; }
        public List<LauncherChecksum> CheckSum { get; set; } = new List<LauncherChecksum>();
    }

    public sealed class LauncherChecksum
    {
        public string CheckSum { get; set; }
        public string Path { get; set; }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            //var appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            if (!Directory.Exists("Deployment"))
                Directory.CreateDirectory("Deployment");

            Console.Write("Enter version: ");
            var version = Console.ReadLine();

            var launcherInfo = new LauncherInfo();
            launcherInfo.Version = version;
            launcherInfo.LauncherPath = "TK-Client\\Local Store\\Launcher\\";

            var files = Directory.GetFiles($"{Environment.CurrentDirectory}\\Deployment", "*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                if(Path.GetFileName(file) == "LauncherInfo.json")
                    continue;

                launcherInfo.CheckSum.Add(new LauncherChecksum()
                {
                    CheckSum = GetMD5HashFromFile(file),
                    Path = file.Replace($"{Environment.CurrentDirectory}\\Deployment\\", "")
                });
            }

            using var f = File.CreateText($"{Environment.CurrentDirectory}\\Deployment\\LauncherInfo.json");
            f.Write(System.Text.Json.JsonSerializer.Serialize(launcherInfo));
        }

        private static string GetMD5HashFromFile(string fileName)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(fileName);

            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
        }
    }
}