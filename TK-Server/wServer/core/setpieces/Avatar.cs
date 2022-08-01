using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class Avatar : ISetPiece
    {
        public override int Size => 32;
        public override string Map => "set_piece/avatar.jm";
    }
}
