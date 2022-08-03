using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Launcher.Components
{
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();

            Download();
        }

        public string Version = "1.0";

        private static HttpClient HttpClient = new HttpClient();

        public async void Download()
        {
            var progress = new Progress<(string, double)>();

            progress.ProgressChanged += Progress_ProgressChanged;

            var cancellationToken = new CancellationTokenSource();
            await DownloadFileAsync("http://www.dotpdn.com/files/Paint.NET.3.5.11.Install.zip", progress, cancellationToken.Token);
        }

        private void Progress_ProgressChanged(object? sender, (string text, double value) e)
        {
            ProgressBar.Value = e.value;
            ProgressText.Text = $"Downloading: {e.text}";
            if(ProgressBar.Value == ProgressBar.Maximum)
                ProgressText.Text = "Downloaded";
        }
        
        public async Task DownloadFileAsync(string url, IProgress<(string, double)> progress, CancellationToken token)
        {
            var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
            var canReportProgress = total != -1 && progress != null;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var totalRead = 0L;
                var buffer = new byte[4096];
                var isMoreToRead = true;

                do
                {
                    token.ThrowIfCancellationRequested();

                    var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                    if (read == 0)
                    {
                        isMoreToRead = false;
                    }
                    else
                    {
                        var data = new byte[read];
                        buffer.ToList().CopyTo(0, data, 0, read);

                        // TODO: put here the code to write the file to disk

                        totalRead += read;

                        if (canReportProgress)
                        {
                            progress.Report((SizeSuffix(totalRead) + " / " + SizeSuffix(total), (totalRead * 1d) / (total * 1d) * 100));
                        }
                    }
                } while (isMoreToRead);
            }
        }

        static readonly string[] SizeSuffixes = { "b", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(long value, int decimalPlaces = 2)
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
