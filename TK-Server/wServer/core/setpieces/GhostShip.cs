using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class GhostShip : ISetPiece
    {
        public override int Size => 40;
        public override string Map => "set_piece/ghost_ship.jm";
    }
}
