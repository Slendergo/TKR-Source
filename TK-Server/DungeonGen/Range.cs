using System;

namespace dungeonGen
{
    public struct Range
    {
        public static readonly Range Zero = new Range(0, 0);

        public readonly int Begin;
        public readonly int End;

        public Range(int begin, int end)
        {
            if (end < begin)
                end = begin;

            Begin = begin;
            End = end;
        }

        public bool IsEmpty => Begin == End;

        public Range Intersection(Range range) => new Range(Math.Max(Begin, range.Begin), Math.Min(End, range.End));

        public int Random(Random rand) => rand.Next(Begin, End + 1);

        public override string ToString() => string.Format("[{0}, {1}]", Begin, End);
    }
}
