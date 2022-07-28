using CA.Threading.Tasks;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace tk.pipelines.server
{
    public class PipelineServer
    {
        private static CancellationTokenSource _cts;
        private static ManualResetEvent _mre;
        private static InternalRoutine _routine;
        private static Socket _socket;

        private static string _name => typeof(PipelineServer).Name;

        private static void Main()
        {
            _cts = new CancellationTokenSource();
            _mre = new ManualResetEvent(false);

            _routine = new InternalRoutine(1000, async (delta) => await OnReceiveSocketAsync(), (error) => Log.Error(error));
            _routine.AttachToParent(_cts.Token);

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Loopback, 7777));
            _socket.Listen(1024);

            Log.Info($"{_name}: Listening on port 7777");

            _routine.Start();

            Console.CancelKeyPress += (s, e) => _cts.Cancel();

            _mre.WaitOne();

            Log.Warn($"{_name}: Preparing to shutdown...");
            Thread.Sleep(3000);
            Log.Info($"{_name}: Shutdown completed!");
            Environment.Exit(0);
        }

        private static async Task OnReceiveSocketAsync()
        {
            var socket = await _socket.AcceptAsync();
            socket.HandleConnection(Log.Info);
        }
    }
}
