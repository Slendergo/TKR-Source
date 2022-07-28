using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class Hermit : ISetPiece
    {
        public int Size => 32;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["Hermit"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
