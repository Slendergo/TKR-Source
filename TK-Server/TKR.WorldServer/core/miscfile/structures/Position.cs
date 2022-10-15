using TKR.Shared;
using System;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.core.miscfile.structures
{
    public struct Position
    {
        public float X;
        public float Y;
        public int Hash => (int)X << 16 | (int)Y;
        public IntPoint IntPoint => new IntPoint((int)X, (int)Y);

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Position Read(NetworkReader rdr) => new Position
        {
            X = rdr.ReadSingle(),
            Y = rdr.ReadSingle()
        };

        public override string ToString() => string.Format("{{X: {0}, Y: {1}}}", X, Y);

        public void Write(NetworkWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
        }

        public double DistTo(Entity host) => Math.Sqrt((host.X - X) * (host.X - X) + (host.Y - Y) * (host.Y - Y));
        public double DistTo(ref Position position) => Math.Sqrt((position.X - X) * (position.X - X) + (position.Y - Y) * (position.Y - Y));
        public double DistTo(double x, double y) => Math.Sqrt((x - X) * (x - X) + (y - Y) * (y - Y));

        public float Distance(Position from, Position to)
        {
            float v1 = from.X - to.X, v2 = from.Y - to.Y;
            return (float)Math.Sqrt(v1 * v1 + v2 * v2);
        }
    }
}
