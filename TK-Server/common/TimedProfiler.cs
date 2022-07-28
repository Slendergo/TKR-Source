using System;
using System.Diagnostics;

namespace common
{
    /// <summary>
    /// Display total elapsed time of a procedure interval
    /// until <see cref="TimedProfiler"/> is disposed.
    /// </summary>
    public sealed class TimedProfiler : IDisposable
    {
        private readonly Action<string> _action;
        private readonly string _message;
        private readonly Stopwatch _stopwatch;

        public TimedProfiler(string message) : this(message, null)
        { }

        public TimedProfiler(string message, Action<string> action)
        {
            _message = message;
            _action = action;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            var time = _stopwatch.Elapsed;
            var ms = _stopwatch.ElapsedMilliseconds;
            var info = $"{_message} - Elapsed: {time} ({ms}ms)";

            if (_action != null)
                _action.Invoke(info);
            else
                Console.WriteLine(info);
        }
    }
}
