using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class FireElemental : ISetPiece
    {
        public int Size => 32;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["FireElemental"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
