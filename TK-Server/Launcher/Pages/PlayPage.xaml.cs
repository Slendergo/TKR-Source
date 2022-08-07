using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using Launcher;

namespace Launcher.Components
{
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();

            //HttpClient.Timeout = TimeSpan.FromMinutes(1);

            Play.Name = "Checking";
            Update();
        }

        public string ServerVersion;

        private static HttpClient HttpClient = new HttpClient();

        public async void Update()
        {
            ProgressText.Text = "Checking For Update";

            try
            {
                var checksum = await HttpClient.GetStringAsync("http://147.189.169.79:2000/launcher/checkSum");
                var serverLauncherInfo = System.Text.Json.JsonSerializer.Deserialize<LauncherInfo>(checksum);

                if (App.LauncherInfo == null)
                    App.SetLauncherInformation(serverLauncherInfo);

                if (serverLauncherInfo.Version != App.LauncherInfo.Version)
                {
                    Play.Name = "Update";
                    ProgressText.Text = "New Update Found";
                }

                var cancellationToken = new CancellationTokenSource();

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.Version.Text = $"v{App.LauncherInfo.Version}";

                ServerVersion = serverLauncherInfo.Version;

                var changed = new List<LauncherChecksum>();
                var same = new List<LauncherChecksum>();
                var missing = new List<LauncherChecksum>();
                var all = App.LauncherInfo.CheckSum.ToList();

                await Task.Delay(1000);

                foreach (var fileChecksum in serverLauncherInfo.CheckSum)
                {
                    var duplicateEntry = App.LauncherInfo.CheckSum.FirstOrDefault(_ => _.Path == fileChecksum.Path);
                    if (duplicateEntry != null)
                    {
                        var path = $"{App.GetApplicationData()}\\{serverLauncherInfo.LauncherPath}\\{fileChecksum.Path}";
                        if (!File.Exists(path))
                        {
                            missing.Add(fileChecksum);
                            continue;
                        }

                        if (fileChecksum.CheckSum != duplicateEntry.CheckSum)
                        {
                            changed.Add(fileChecksum);
                            Trace.WriteLine($"{fileChecksum.Path} -> {fileChecksum.CheckSum} != {duplicateEntry.CheckSum}");
                        }
                        else
                        {
                            same.Add(fileChecksum);
                            Trace.WriteLine($"{fileChecksum.Path} -> {fileChecksum.CheckSum} == {duplicateEntry.CheckSum}");
                        }
                        continue;
                    }

                    missing.Add(fileChecksum);
                    Trace.WriteLine($"{fileChecksum.Path} -> Is Missing");
                }

                if (serverLauncherInfo.Version != App.LauncherInfo.Version || missing.Count > 0)
                {
                    foreach (var c in changed)
                    {
                        var duplicateEntry = changed.FirstOrDefault(_ => _.Path == c.Path);
                        _ = all.Remove(duplicateEntry);
                    }

                    foreach (var c in same)
                    {
                        var duplicateEntry = all.FirstOrDefault(_ => _.Path == c.Path);
                        _ = all.Remove(duplicateEntry);
                    }

                    foreach (var c in missing)
                    {
                        var duplicateEntry = all.FirstOrDefault(_ => _.Path == c.Path);
                        _ = all.Remove(duplicateEntry);
                    }

                    foreach (var c in all)
                    {
                        var path = $"{App.GetApplicationData()}\\{c.Path}";
                        File.Delete(path);
                    }

                    var progress = new Progress<(string, double)>();
                    progress.ProgressChanged += UpdatingProgress_ProgressChanged;

                    var count = changed.Count + missing.Count;
                    foreach (var fileChecksum in changed)
                    {
                        var path = $"{App.GetApplicationData()}\\{serverLauncherInfo.LauncherPath}\\{fileChecksum.Path}";

                        var contents = await DownloadFileAsync($"http://147.189.169.79:2000/launcher/fetchFile?checksum={fileChecksum.CheckSum}", progress, cancellationToken.Token);

                        File.Delete(path);
                        File.WriteAllBytes(path, contents);
                    }

                    foreach (var fileChecksum in missing)
                    {
                        var path = $"{App.GetApplicationData()}\\{serverLauncherInfo.LauncherPath}\\{fileChecksum.Path}";

                        var contents = await DownloadFileAsync($"http://147.189.169.79:2000/launcher/fetchFile?checksum={fileChecksum.CheckSum}", progress, cancellationToken.Token);

                        var file = new FileInfo(path);
                        file.Directory.Create();
                        File.WriteAllBytes(path, contents);
                    }

                    mainWindow.Version.Text = $"v{serverLauncherInfo.Version}";
                    App.SetLauncherInformation(serverLauncherInfo);

                    ProgressText.Text = $"Ready to play!";
                    Play.Name = "Play";
                    Play.IsEnabled = true;
                }
                else
                {
                    ProgressText.Text = $"No update found!";
                    await Task.Delay(1000);
                    ProgressText.Text = $"Ready to play!";
                    Play.Name = "Play";
                    Play.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void UpdatingProgress_ProgressChanged(object? sender, (string text, double value) e)
        {
            ProgressBar.Value = e.value;
            ProgressText.Text = $"Updating to v{ServerVersion}: {e.text}";
        }
        
        public async Task<byte[]> DownloadFileAsync(string url, IProgress<(string, double)> progress, CancellationToken token)
        {
            var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
            var canReportProgress = total != -1 && progress != null;

            var speed = CheckInternetSpeed();

            var segments = new List<byte[]>();
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var totalRead = 0L;
                var buffer = new byte[(int)speed];
                var isMoreToRead = true;

                do
                {
                    token.ThrowIfCancellationRequested();

                    var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                    if (read == 0)
                        isMoreToRead = false;
                    else
                    {
                        var data = new byte[read];
                        buffer.ToList().CopyTo(0, data, 0, read);
                        
                        segments.Add(data);

                        totalRead += read;

                        if (canReportProgress)
                            progress.Report((SizeSuffix(totalRead) + " / " + SizeSuffix(total), (totalRead * 1d) / (total * 1d) * 100));
                        await Task.Delay(1);
                    }
                }
                while (isMoreToRead);
            }
            return segments.SelectMany(a => a).ToArray();
        }

        public double CheckInternetSpeed()
        {
            var wc = new WebClient();
            var dt1 = DateTime.Now;
            var data = wc.DownloadData("http://google.com");
            var dt2 = DateTime.Now;
            return Math.Round(data.Length / (dt2 - dt1).TotalSeconds, 2);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var path = $"{App.GetLauncherPath()}\\TK-Client.exe";

            var mainWindow = Application.Current.MainWindow;

            mainWindow.WindowState = WindowState.Minimized;
            var process = Process.Start(path);
            process.WaitForExit();
            mainWindow.WindowState = WindowState.Normal;
        }

        private static readonly string[] SizeSuffixes = { "b", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private static string SizeSuffix(long value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException("decimalPlaces");

            if (value < 0)
                return "-" + SizeSuffix(-value, decimalPlaces);

            if (value == 0)
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
