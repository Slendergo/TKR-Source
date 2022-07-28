using wServer.core.objects;
using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class AbyssDeath : ISetPiece
    {
        private byte[,] SetPiece = new byte[,]
        {
            {1, 1, 1},
            {1, 2, 1},
            {1, 1, 1},
        };

        public int Size => 3;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var dat = world.Manager.Resources.GameData;
            var p = new IntPoint
            {
                X = pos.X - (Size / 2),
                Y = pos.Y - (Size / 2)
            };

            for (var x = 0; x < Size; x++)
                for (var y = 0; y < Size; y++)
                {
                    if (SetPiece[y, x] == 1)
                    {
                        var tile = world.Map[x + p.X, y + p.Y].Clone();
                        tile.TileId = dat.IdToTileType["Red Quad"];
                        tile.ObjType = 0;
                        world.Map[x + p.X, y + p.Y] = tile;
                    }

                    if (SetPiece[y, x] == 2)
                    {
                        var tile = world.Map[x + p.X, y + p.Y].Clone();
                        tile.TileId = dat.IdToTileType["Red Quad"];
                        tile.ObjType = 0;
                        world.Map[x + p.X, y + p.Y] = tile;

                        var en = Entity.Resolve(world.Manager, "Realm Portal");
                        en.Move(x + p.X + 0.5f, y + p.Y + 0.5f);
                        world.EnterWorld(en);
                    }
                }
        }
    }
}
