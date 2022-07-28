using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class KageKami : ISetPiece
    {
        public int Size => 65;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["KageKami"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
