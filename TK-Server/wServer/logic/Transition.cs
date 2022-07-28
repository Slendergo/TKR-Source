using System;
using System.Collections.Generic;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic
{
    public abstract class Transition : IStateChildren
    {
        protected readonly string[] TargetStates;
        protected int SelectedState;

        [ThreadStatic]
        private static Random _rand;

        public Transition(params string[] targetStates)
        {
            TargetStates = targetStates;
        }

        public State[] TargetState { get; private set; }

        protected static Random Random
        {
            get { return _rand ?? (_rand = new Random()); }
        }

        public bool Tick(Entity host, TickData time)
        {
            if (host == null) return false;

            host.StateStorage.TryGetValue(this, out object state);

            var ret = TickCore(host, time, ref state);

            if (ret)
                host.SwitchTo(TargetState[SelectedState]);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
            return ret;
        }

        internal void Resolve(IDictionary<string, State> states)
        {
            var numStates = TargetStates.Length;
            TargetState = new State[numStates];
            for (var i = 0; i < numStates; i++)
                TargetState[i] = states[TargetStates[i]];
        }

        protected abstract bool TickCore(Entity host, TickData time, ref object state);
    }
}
