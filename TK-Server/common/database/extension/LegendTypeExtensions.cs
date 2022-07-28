using System;
using System.Text;

namespace common.database.extension
{
    public static class LegendTypeExtensions
    {
        public static string ToField(this LegendType type)
        {
            var sb = new StringBuilder();

            switch (type)
            {
                default: throw new NotSupportedException($"{typeof(LegendTypeExtensions).Name} doesn't implements support for following type: {type}");
                case LegendType.Week: sb.Append("week"); break;
                case LegendType.Month: sb.Append("month"); break;
                case LegendType.All: sb.Append("all"); break;
            }

            return sb.ToString();
        }
    }
}
