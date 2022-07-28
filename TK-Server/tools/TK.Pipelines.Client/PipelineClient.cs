using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace tk.pipelines.client
{
    public class PipelineClient
    {
        private static bool _disposed;
        private static ManualResetEvent _mre;

        private static string _name => typeof(PipelineClient).Name;

        private static async Task Main()
        {
            _mre = new ManualResetEvent(false);
            _disposed = false;

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Log.Info($"{_name}: Connecting to port 7777");

            while (!socket.Connected)
            {
                await socket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 7777));

                Thread.Sleep(500);
            }

            Log.Info($"{_name}: Connected!");

            var stream = new NetworkStream(socket);

            Console.CancelKeyPress += delegate
            {
                stream.DisposeAsync();

                _disposed = true;
                _mre.Set();
            };

            while (!_disposed)
            {
                var text = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                if (!socket.Connected)
                {
                    Log.Warn($"{_name}: Disconnected!");
                    break;
                }

                await socket.SendAsync(stream, text, Log.Error);
            }

            _mre.WaitOne();

            Log.Warn($"{_name}: Preparing to shutdown...");
            Thread.Sleep(3000);
            Log.Info($"{_name}: Shutdown completed!");
            Environment.Exit(0);
        }
    }
}
