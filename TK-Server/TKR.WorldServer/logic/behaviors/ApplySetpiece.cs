using System;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.setpieces;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.behaviors
{
    public class ApplySetpiece : Behavior
    {
        private readonly string name;

        public ApplySetpiece(string name) => this.name = name;

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            var piece = (ISetPiece)Activator.CreateInstance(Type.GetType("TKR.WorldServer.core.setpieces." + name, true, true));
            piece.RenderSetPiece(host.World, new IntPoint((int)host.X, (int)host.Y));
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
