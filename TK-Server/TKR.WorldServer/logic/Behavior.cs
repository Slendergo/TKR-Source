using TKR.Shared;
using TKR.Shared.resources;
using NLog;
using System;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.logic
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

        public void OnStateEntry(Entity host, TickTime time)
        {
            if (host == null || host.World == null)
                return;

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            OnStateEntry(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        public void OnStateExit(Entity host, TickTime time)
        {
            if (host == null || host.World == null)
                return;

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            OnStateExit(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        public void Tick(Entity host, TickTime time)
        {
            if (host == null || host.World == null)
                return;

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            TickCore(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        public bool TickOrdered(Entity host, TickTime time)
        {
            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            var ret = TickCoreOrdered(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;

            return ret;
        }

        public virtual void OnDeath(Entity host, ref TickTime time) { }

        protected internal virtual void Resolve(State parent) { }

        protected virtual void OnStateEntry(Entity host, TickTime time, ref object state) { }

        protected virtual void OnStateExit(Entity host, TickTime time, ref object state) { }

        protected virtual void TickCore(Entity host, TickTime time, ref object state) { }

        protected virtual bool TickCoreOrdered(Entity host, TickTime time, ref object state) => true;

        protected static Position PointAt(Entity host, float angle, float radius) => new Position((float)(host.X + Math.Cos(angle) * radius), (float)(host.Y + Math.Sin(angle) * radius));
        protected static float ClampSpeed(float value, float min, float max) => value < min ? min : value > max ? max : value;
        protected static float GetSpeedMultiplier(Entity host, float spd) => host.HasConditionEffect(ConditionEffectIndex.Slowed) ? spd * 0.5f : host.HasConditionEffect(ConditionEffectIndex.Speedy) ? spd * 1.5f : spd;
    }
}
