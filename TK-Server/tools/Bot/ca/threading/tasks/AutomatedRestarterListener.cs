using System;

namespace tk.bot.ca.threading.tasks
{
    /// <summary>
    /// Represents <see cref="AutomatedRestarter"/> listener entry pattern.
    /// </summary>
    public struct AutomatedRestarterListener
    {
        /// <summary>
        /// The <see cref="Action"/> when this listener gonna trigger.
        /// </summary>
        public Action Handler;

        /// <summary>
        /// The <see cref="TimeSpan"/> when this listener gonna trigger.
        /// </summary>
        public TimeSpan Timeout;

        /// <summary>
        /// Varify is <see cref="AutomatedRestarterListener"/> entries are valid.
        /// </summary>
        /// <returns></returns>
        public bool IsInvalid => Timeout == null || Handler == null;
    }
}
