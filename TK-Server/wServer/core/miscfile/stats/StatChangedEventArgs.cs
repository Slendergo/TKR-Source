using System;

namespace wServer.core
{
    public class StatChangedEventArgs : EventArgs
    {
        public StatChangedEventArgs(StatDataType stat, object value, bool updateSelfOnly = false)
        {
            Stat = stat;
            Value = value;
            UpdateSelfOnly = updateSelfOnly;
        }

        public StatDataType Stat { get; private set; }
        public bool UpdateSelfOnly { get; private set; }
        public object Value { get; private set; }
    }
}
