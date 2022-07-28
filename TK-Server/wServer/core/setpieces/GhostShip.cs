using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class GhostShip : ISetPiece
    {
        public int Size => 40;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["GhostShip"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
