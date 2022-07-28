using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace tk.pipelines.client
{
    public static class SocketExtensions
    {
        public static async Task SendAsync(this Socket socket, NetworkStream stream, string data, Action<string, Exception> logger)
        {
            var name = typeof(SocketExtensions).Name;
            var buffer = Encoding.UTF8.GetBytes(data);

            try { await stream.WriteAsync(buffer, 0, buffer.Length); }
            catch (Exception e) { logger.Invoke($"{name}: Error when handling data -> '{data}'", e); }
        }
    }
}
