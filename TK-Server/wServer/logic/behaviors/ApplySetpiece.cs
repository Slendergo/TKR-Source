using System;
using wServer.core;
using wServer.core.objects;
using wServer.core.setpieces;

namespace wServer.logic.behaviors
{
    public class ApplySetpiece : Behavior
    {
        private readonly string name;

        public ApplySetpiece(string name) => this.name = name;

        protected override void OnStateEntry(Entity host, TickData time, ref object state)
        {
            var piece = (ISetPiece)Activator.CreateInstance(Type.GetType("wServer.core.setpieces." + name, true, true));
            piece.RenderSetPiece(host.Owner, new IntPoint((int)host.X, (int)host.Y));
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
