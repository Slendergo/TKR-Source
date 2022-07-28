using dungeonGen.definitions;
using RotMG.Common;
using RotMG.Common.Rasterizer;
using System.Collections.Generic;

namespace dungeonGen
{
    public class RoomCollision
    {
        private const int GridScale = 3;
        private const int GridSize = 1 << GridScale;

        private readonly Dictionary<RoomKey, HashSet<Room>> rooms = new Dictionary<RoomKey, HashSet<Room>>();

        public void Add(Room rm)
        {
            var bounds = rm.Bounds;
            var x = bounds.X;
            var y = bounds.Y;

            for (; y <= bounds.MaxY + GridSize; y += GridSize)
                for (x = bounds.X; x <= bounds.MaxX + 20; x += GridSize)
                    Add(x, y, rm);
        }

        public bool HitTest(Room rm)
        {
            var bounds = new Rect(rm.Bounds.X - 1, rm.Bounds.Y - 1, rm.Bounds.MaxX + 1, rm.Bounds.MaxY + 1);
            var x = bounds.X;
            var y = bounds.Y;

            for (; y <= bounds.MaxY + GridSize; y += GridSize)
                for (x = bounds.X; x <= bounds.MaxX + GridSize; x += GridSize)
                    if (HitTest(x, y, bounds))
                        return true;

            return false;
        }

        public void Remove(Room rm)
        {
            var bounds = rm.Bounds;
            var x = bounds.X;
            var y = bounds.Y;

            for (; y <= bounds.MaxY + GridSize; y += GridSize)
                for (x = bounds.X; x <= bounds.MaxX + 20; x += GridSize)
                    Remove(x, y, rm);
        }

        private void Add(int x, int y, Room rm)
        {
            var key = new RoomKey(x, y);
            var roomList = rooms.GetValueOrCreate(key, k => new HashSet<Room>());
            roomList.Add(rm);
        }

        private bool HitTest(int x, int y, Rect bounds)
        {
            var key = new RoomKey(x, y);
            var roomList = rooms.GetValueOrDefault(key, (HashSet<Room>)null);

            if (roomList != null)
                foreach (var room in roomList)
                    if (!room.Bounds.Intersection(bounds).IsEmpty)
                        return true;

            return false;
        }

        private void Remove(int x, int y, Room rm)
        {
            var key = new RoomKey(x, y);

            if (rooms.TryGetValue(key, out HashSet<Room> roomList))
                roomList.Remove(rm);
        }

        private struct RoomKey
        {
            public readonly int XKey;
            public readonly int YKey;

            public RoomKey(int x, int y)
            {
                XKey = x >> GridScale;
                YKey = y >> GridScale;
            }

            public override int GetHashCode() => XKey * 7 + YKey;
        }
    }
}
