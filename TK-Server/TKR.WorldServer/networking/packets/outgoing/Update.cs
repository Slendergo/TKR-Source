using TKR.Shared;
using System.Collections.Generic;
using TKR.WorldServer.core.miscfile.datas;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public struct TileData
    {
        public int X;
        public int Y;
        public int Tile;

        public TileData(int x, int y, int tile)
        {
            X = x;
            Y = y;
            Tile = tile;
        }
    }

    public class Update : OutgoingMessage
    {
        public readonly List<TileData> Tiles;
        public readonly List<ObjectDef> NewObjs;
        public readonly List<int> Drops;

        public override MessageId MessageId => MessageId.UPDATE;

        public Update()
        {
            Tiles = new List<TileData>();
            NewObjs = new List<ObjectDef>();
            Drops = new List<int>();
        }

        public Update(List<TileData> tiles, List<ObjectDef> newObjs, List<int> drops)
        {
            Tiles = tiles;
            NewObjs = newObjs;
            Drops = drops;
        }

        public override void Write(NWriter wtr)
        {
            wtr.Write((short)Tiles.Count);
            foreach (var i in Tiles)
            {
                wtr.Write((short)i.X);
                wtr.Write((short)i.Y);
                wtr.Write(i.Tile);
            }
            wtr.Write((short)NewObjs.Count);
            foreach (var i in NewObjs)
                i.Write(wtr);
            wtr.Write((short)Drops.Count);
            foreach (var i in Drops)
                wtr.Write(i);
        }
    }
}
