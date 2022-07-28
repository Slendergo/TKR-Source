using common.resources;
using System;
using System.Text;

namespace common.database.extension
{
    public static class CurrencyTypeExtensions
    {
        public static string ToField(this CurrencyType currency, bool isTotal = false)
        {
            var sb = new StringBuilder();

            switch (currency)
            {
                default: throw new NotSupportedException($"{typeof(CurrencyTypeExtensions).Name} doesn't implements support for following type: {currency}");
                case CurrencyType.Gold: sb.Append(isTotal ? "totalCredits" : "credits"); break;
                case CurrencyType.Fame:
                case CurrencyType.GuildFame: sb.Append(isTotal ? "totalFame" : "fame"); break;
            }

            return sb.ToString();
        }
    }
}
