using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal interface ISetPiece
    {
        int Size { get; }

        void RenderSetPiece(World world, IntPoint pos);
    }
}
