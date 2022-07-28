using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class EarthElemental : ISetPiece
    {
        public int Size => 32;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["EarthElemental"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
