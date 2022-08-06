using common;
using System;

namespace wServer
{
    public struct Position
    {
        public float X;
        public float Y;

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Position Read(NReader rdr) => new Position
        {
            X = rdr.ReadSingle(),
            Y = rdr.ReadSingle()
        };

        public override string ToString() => string.Format("{{X: {0}, Y: {1}}}", X, Y);

        public void Write(NWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
        }

        public float Distance(Position from, Position to)
        {
            float v1 = from.X - to.X, v2 = from.Y - to.Y;
            return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }
    }
}
