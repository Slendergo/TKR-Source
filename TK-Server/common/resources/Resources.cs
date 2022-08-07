using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace common.resources
{
    public class Resources : IDisposable
    {
        public XmlData GameData;
        public string ResourcePath;
        public AppSettings Settings;
        public Dictionary<string, byte[]> WebFiles = new Dictionary<string, byte[]>();
        public IList<string> MusicNames;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Resources(string resourcePath, bool? wServer = false, Action<float, float, string, bool> progress = null, bool exportXml = false)
        {
            if (wServer.HasValue) Log.Info("Loading resources...");

            ResourcePath = resourcePath;
            GameData = new XmlData(resourcePath + "/xml", !wServer.HasValue, progress, exportXml);
            
            if (!wServer.HasValue) return;

            Settings = new AppSettings(resourcePath + "/data/init.xml");

            if (!wServer.Value) 
                webFiles(resourcePath + "/web");
            else 
                GameData.LoadMaps($"{resourcePath}/worlds");

            music(resourcePath);
        }

        private void music(string baseDir)
        {
            List<string> music;

            MusicNames =
                new ReadOnlyCollection<string>(
                    music = new List<string>());

            music.AddRange(Directory
                .EnumerateFiles(baseDir + "/web/music", "*.mp3", SearchOption.AllDirectories)
                .Select(Path.GetFileNameWithoutExtension));
        }

        public IDictionary<string, byte[]> Languages { get; private set; }

        public void Dispose()
        {
            ResourcePath = null;
            GameData = null;
            WebFiles = null;
            Languages = null;
            Settings = null;

            GC.SuppressFinalize(this);
        }

        private void webFiles(string dir)
        {
            Log.Info("Loading web data...");

            var files = Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var webPath = file.Substring(dir.Length, file.Length - dir.Length)
                    .Replace("\\", "/");

                WebFiles[webPath] = File.ReadAllBytes(file);
            }
        }
    }
}
