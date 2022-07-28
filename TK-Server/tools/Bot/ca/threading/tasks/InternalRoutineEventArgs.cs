using System;

namespace tk.bot.ca.threading.tasks
{
    /// <summary>
    /// Contains event arguments from <see cref="InternalRoutine.OnDeltaVariation"/> event.
    /// </summary>
    public class InternalRoutineEventArgs : EventArgs
    {
#pragma warning disable

        public InternalRoutineEventArgs(
            int delta,
            int ticksPerSecond,
            int timeout
            ) : base()

#pragma warning restore

        {
            Delta = delta;
            TicksPerSecond = ticksPerSecond;
            Timeout = timeout;
        }

        /// <summary>
        /// Delta variation of <see cref="InternalRoutine.routine"/>.
        /// </summary>
        public int Delta { get; }

        /// <summary>
        /// Ticks per second for <see cref="InternalRoutine.routine"/> invoke on <see cref="TaskScheduler"/>.
        /// </summary>
        public int TicksPerSecond { get; }

        /// <summary>
        /// Timeout in milliseconds for <see cref="InternalRoutine.routine"/> invoke on <see cref="TaskScheduler"/>.
        /// </summary>
        public int Timeout { get; }
    }
}
