using System;
using System.Diagnostics;

namespace dungeonGen.definitions
{
    public class Edge
    {
        private Edge()
        { }

        public Link Linkage { get; set; }
        public Room RoomA { get; private set; }
        public Room RoomB { get; private set; }

        public static void Link(Room a, Room b, Link link)
        {
            Debug.Assert(a != b);

            var edge = new Edge
            {
                RoomA = a,
                RoomB = b,
                Linkage = link
            };

            a.Edges.Add(edge);
            b.Edges.Add(edge);
        }

        public static void UnLink(Room a, Room b)
        {
            Edge edge = null;

            foreach (var ed in a.Edges)
                if (ed.RoomA == b || ed.RoomB == b)
                {
                    edge = ed;
                    break;
                }

            if (edge == null)
                throw new ArgumentException();

            a.Edges.Remove(edge);
            b.Edges.Remove(edge);
        }
    }
}
