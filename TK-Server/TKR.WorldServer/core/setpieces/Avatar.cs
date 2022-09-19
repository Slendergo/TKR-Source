using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.setpieces
{
    internal class Avatar : ISetPiece
    {
        public override int Size => 32;
        public override string Map => "set_piece/avatar.jm";
    }
}
