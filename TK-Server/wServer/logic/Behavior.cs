using common;
using NLog;
using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic
{
    public abstract class Behavior : IStateChildren
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [ThreadStatic]
        private static Random rand;

        protected static Random Random => rand ?? (rand = new Random());

        public static ushort GetObjType(string id)
        {
            if (!BehaviorDb.InitGameData.IdToObjectType.TryGetValue(id, out ushort ret))
            {
                ret = BehaviorDb.InitGameData.IdToObjectType["Pirate"];
                Log.Warn($"Object type '{id}' not found. Using Pirate ({ret.To4Hex()}).");
            }
            return ret;
        }

        public void OnStateEntry(Entity host, TickData time)
        {
            if (host == null || host.Owner == null)
                return;

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            OnStateEntry(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        public void OnStateExit(Entity host, TickData time)
        {
            if (host == null || host.Owner == null)
                return;

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            OnStateExit(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        public void Tick(Entity host, TickData time)
        {
            if (host == null || host.Owner == null)
                return;

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            TickCore(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        protected internal virtual void Resolve(State parent)
        { }

        protected virtual void OnStateEntry(Entity host, TickData time, ref object state)
        { }

        protected virtual void OnStateExit(Entity host, TickData time, ref object state)
        { }

        protected abstract void TickCore(Entity host, TickData time, ref object state);
    }
}
