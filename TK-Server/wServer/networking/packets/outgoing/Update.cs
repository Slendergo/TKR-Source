using common;
using System.Collections.Generic;

namespace wServer.networking.packets.outgoing
{
    public class Update : OutgoingMessage
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

        public List<TileData> Tiles { get; set; }
        public List<ObjectDef> NewObjs { get; set; }
        public List<int> Drops { get; set; }

        public override PacketId ID => PacketId.UPDATE;

        public override Packet CreateInstance()
        {
            return new Update();
        }

        public Update()
        {
            Tiles = new List<TileData>();
            NewObjs = new List<ObjectDef>();
            Drops = new List<int>();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)Tiles.Count);
            foreach (var i in Tiles)
            {
                wtr.Write((short)i.X);
                wtr.Write((short)i.Y);
                wtr.Write((ushort)i.Tile);
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
