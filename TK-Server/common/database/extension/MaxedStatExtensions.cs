using System;
using System.Text;

namespace common.database.extension
{
    public static class MaxedStatExtensions
    {
        public static string ToField(this MaxedStat stat)
        {
            var sb = new StringBuilder("maxed");

            switch (stat)
            {
                default: throw new NotSupportedException($"{typeof(MaxedStatExtensions).Name} doesn't implements support for following type: {stat}");
                case MaxedStat.Life: sb.Append("Life"); break;
                case MaxedStat.Mana: sb.Append("Mana"); break;
                case MaxedStat.Attack: sb.Append("Att"); break;
                case MaxedStat.Defense: sb.Append("Def"); break;
                case MaxedStat.Speed: sb.Append("Spd"); break;
                case MaxedStat.Dexterity: sb.Append("Dex"); break;
                case MaxedStat.Vitality: sb.Append("Vit"); break;
                case MaxedStat.Wisdom: sb.Append("Wis"); break;
            }

            return sb.ToString();
        }
    }
}
