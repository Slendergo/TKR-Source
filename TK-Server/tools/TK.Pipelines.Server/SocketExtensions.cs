using CA.Threading.Tasks;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tk.pipelines.server
{
    public static class SocketExtensions
    {
        public static void HandleConnection(this Socket socket, Action<string> logger)
        {
            var name = typeof(SocketExtensions).Name;

            logger.Invoke($"{name}: {socket.RemoteEndPoint} connected!");

            var stream = new NetworkStream(socket);
            var reader = PipeReader.Create(stream);
            var cts = new CancellationTokenSource();
            var routine = new InternalRoutine(200, async () =>
            {
                if (!socket.Connected && cts.IsCancellationRequested)
                    return;

                while (true)
                {
                    if (!socket.Connected)
                    {
                        cts.Cancel();
                        break;
                    }

                    try
                    {
                        var result = await reader.ReadAsync(cts.Token);
                        var buffer = result.Buffer;
                        var data = Encoding.UTF8.GetString(buffer.First.ToArray());

                        logger.Invoke($"{name}: Processing {buffer.Length} Byte{(buffer.Length > 1 ? "s" : "")}:");
                        logger.Invoke($"{name}: data -> '{data}'");

                        if (result.IsCompleted)
                            return;

                        reader.AdvanceTo(buffer.Start, buffer.End);
                    }
                    catch { break; }
                }
            });
            routine.AttachToParent(cts.Token);
            routine.OnFinished += async (s, e) =>
            {
                await reader.CompleteAsync();
                await stream.DisposeAsync();

                logger.Invoke($"{name}: {socket.RemoteEndPoint} disconnected!");

                socket.Dispose();
            };

            logger.Invoke($"{name}: {socket.RemoteEndPoint} handling connection");

            routine.Start();
        }
    }
}
