using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class StrangeMagician : ISetPiece
    {
        public int Size => 32;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            //var proto = world.Manager.Resources.Worlds["StrangeMagician"];
            //SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
