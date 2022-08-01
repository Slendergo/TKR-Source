using System;
using System.Diagnostics;

namespace wServer.utils
{
    public sealed class TimedProfiler : IDisposable
    {
        private string Message { get; }
        private Stopwatch Stopwatch { get; }

        public TimedProfiler(string message)
        {
            Message = message;
            Stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            var time = Stopwatch.Elapsed;
            var ms = Stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{Message} - Elapsed: {time} ({ms}ms)");
        }
    }
}
