using dungeonGen.templates;
using RotMG.Common.Rasterizer;

namespace dungeonGen.definitions
{
    public class DungeonGraph
    {
        private const int _pad = 4;

        public DungeonGraph(DungeonTemplate template, Room[] rooms)
        {
            Template = template;

            var dx = int.MaxValue;
            var dy = int.MaxValue;
            var mx = int.MinValue;
            var my = int.MinValue;

            for (int i = 0; i < rooms.Length; i++)
            {
                var bounds = rooms[i].Bounds;

                if (bounds.X < dx)
                    dx = bounds.X;
                if (bounds.Y < dy)
                    dy = bounds.Y;

                if (bounds.MaxX > mx)
                    mx = bounds.MaxX;
                if (bounds.MaxY > my)
                    my = bounds.MaxY;
            }

            Width = mx - dx + _pad * 2;
            Height = my - dy + _pad * 2;

            for (int i = 0; i < rooms.Length; i++)
            {
                var room = rooms[i];
                var pos = room.Pos;

                room.Pos = new Point(pos.X - dx + _pad, pos.Y - dy + _pad);

                foreach (var edge in room.Edges)
                {
                    if (edge.RoomA != room)
                        continue;

                    if (edge.Linkage.Direction == Direction.South || edge.Linkage.Direction == Direction.North)
                        edge.Linkage = new Link(edge.Linkage.Direction, edge.Linkage.Offset - dx + _pad);
                    else if (edge.Linkage.Direction == Direction.East || edge.Linkage.Direction == Direction.West)
                        edge.Linkage = new Link(edge.Linkage.Direction, edge.Linkage.Offset - dy + _pad);
                }
            }

            Rooms = rooms;
        }

        public int Height { get; private set; }
        public Room[] Rooms { get; private set; }
        public DungeonTemplate Template { get; private set; }

        public int Width { get; private set; }
    }
}
