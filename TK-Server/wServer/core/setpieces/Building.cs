using System;
using wServer.core.worlds;

namespace wServer.core.setpieces
{
    internal class Building : ISetPiece
    {
        private static readonly string Floor = "Brown Lines";
        private static readonly string Wall = "Wooden Wall";

        private Random rand = new Random();

        public int Size => 21;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var w = rand.Next(19, 22);
            var h = rand.Next(19, 22);
            var t = new int[w, h];

            for (var x = 0; x < w; x++)                     //Perimeter
            {
                t[x, 0] = 1;
                t[x, h - 1] = 1;
            }

            for (var y = 0; y < h; y++)
            {
                t[0, y] = 1;
                t[w - 1, y] = 1;
            }

            var midPtH = h / 2 + rand.Next(-2, 3);          //Mid hori wall
            var sepH = rand.Next(2, 4);

            if (rand.Next() % 2 == 0)
            {
                for (var x = sepH; x < w; x++)
                    t[x, midPtH] = 1;
            }
            else
            {
                for (var x = 0; x < w - sepH; x++)
                    t[x, midPtH] = 1;
            }

            int begin, end;

            if (rand.Next() % 2 == 0)
            {
                begin = 0;
                end = midPtH;
            }
            else
                begin = midPtH; end = h;

            var midPtV = w / 2 + rand.Next(-2, 3);          //Mid vert wall
            var sepW = rand.Next(2, 4);

            if (rand.Next() % 2 == 0)
            {
                for (var y = begin + sepW; y < end; y++)
                    t[midPtV, y] = 1;
            }
            else
            {
                for (var y = begin; y < end - sepW; y++)
                    t[midPtV, y] = 1;
            }
            for (var x = 0; x < w; x++)                     //Flooring
                for (var y = 0; y < h; y++)
                    if (t[x, y] == 0)
                        t[x, y] = 2;

            for (var x = 0; x < w; x++)                     //Corruption
                for (var y = 0; y < h; y++)
                    if (rand.Next() % 2 == 0)
                        t[x, y] = 0;

            var rotation = rand.Next(0, 4);                 //Rotation

            for (var i = 0; i < rotation; i++)
                t = SetPieces.rotateCW(t);

            w = t.GetLength(0); h = t.GetLength(1);

            var dat = world.Manager.Resources.GameData;

            for (var x = 0; x < w; x++)                     //Rendering
                for (var y = 0; y < h; y++)
                {
                    if (t[x, y] == 1)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y];
                        tile.ObjType = dat.IdToObjectType[Wall];
                        tile.ObjDesc = dat.ObjectDescs[tile.ObjType];

                        if (tile.ObjId == 0)
                            tile.ObjId = world.GetNextEntityId();

                        tile.UpdateCount++;
                    }
                    else if (t[x, y] == 2)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y];
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = 0;
                        tile.UpdateCount++;
                    }
                }
        }
    }
}
