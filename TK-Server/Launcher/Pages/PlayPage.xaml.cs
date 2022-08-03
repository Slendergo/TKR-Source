using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launcher.Components
{
    public class JsonMap
    {
        public string Version { get; set; }
        public List<Record> Records { get; set; } = new List<Record>();
    }

    public class Record
    {
        public string Path { get; set; }
        public string CheckSum { get; set; }
    }


    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();

            CheckForUpdates();
        }

        public JsonMap LauncherJsonMap { get; private set; }
        public JsonMap ServerLauncherJsonMap { get; private set; }

        public void CheckForUpdates()
        {
            Task.Factory.StartNew(() =>
            {
                FetchChecksum();
            }).ContinueWith(_ =>
            {
                Trace.WriteLine("Checking for changes");

                var fileToUpdate = new List<string>();
                var fileToRemove = new List<string>();

                // check the current files to the client for removals
                foreach (var file in LauncherJsonMap.Records)
                {
                    var clientFile = ServerLauncherJsonMap.Records.FirstOrDefault(_ => file.Path == _.Path);
                    if (clientFile == null)
                    {
                        Trace.WriteLine($"Remove {file.CheckSum}");
                        fileToRemove.Add(file.Path);
                        continue;
                    }

                    // if it exists check the chechsum for changes
                    if (file.CheckSum != clientFile.CheckSum)
                    {
                        Console.WriteLine($"Update {file.CheckSum} -> {clientFile.CheckSum}");
                        fileToUpdate.Add(file.CheckSum);
                        // file has changed
                    }
                }

                var fileToAdd = new List<string>();
                foreach (var file in ServerLauncherJsonMap.Records)
                {
                    if (fileToUpdate.Contains(file.Path))
                        continue;
                    Trace.WriteLine($"Add {file.CheckSum}");
                    fileToAdd.Add(file.CheckSum);
                }

                Task.Factory.StartNew(() =>
                {
                    var compressedZip = RequestFiles(fileToAdd);

                    using var f = File.Create("Zip.zip");
                    f.Write(compressedZip, 0, compressedZip.Length);
                });
            });
        }

        public byte[] RequestFiles(List<string> files)
        {
            using var client = new HttpClient();

            var json = JsonSerializer.Serialize(files);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("http://127.0.0.1:1999/launcher/requestChanges", content).GetAwaiter().GetResult();
            return response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
        }

        public void FetchChecksum()
        {
            using var client = new HttpClient();

            var values = new Dictionary<string, string>();

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://127.0.0.1:1999/launcher/checkSum", content).GetAwaiter().GetResult();

            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            ServerLauncherJsonMap = JsonSerializer.Deserialize<JsonMap>(responseString);

            if (!File.Exists("CheckSum.json"))
            {
                using var file = File.CreateText("CheckSum.json");
                file.WriteLine(responseString);
                LauncherJsonMap = JsonSerializer.Deserialize<JsonMap>(responseString);
            }
            else
            {
                var json = File.ReadAllText("CheckSum.json");
                LauncherJsonMap = JsonSerializer.Deserialize<JsonMap>(json);
            }
        }
    }
}
