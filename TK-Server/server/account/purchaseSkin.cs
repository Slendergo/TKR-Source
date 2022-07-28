using Anna.Request;
using common;
using common.database;
using common.utils;
using System.Collections.Specialized;

namespace server.account
{
    internal class purchaseSkin : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = _db.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == DbLoginStatus.OK)
            {
                // perhaps the checks should be moved into the purchas skin routine...
                var skinType = (ushort)Utils.GetInt(query["skinType"]);
                var skinDesc = Program.Resources.GameData.Skins[skinType];
                var classStats = Program.Database.ReadClassStats(acc);

                if (skinDesc.UnlockLevel > classStats[skinDesc.PlayerClassType].BestLevel || skinDesc.Cost > acc.Credits)
                {
                    Write(context, "<Error>Failed to purchase skin</Error>");
                    return;
                }

                Program.Database.PurchaseSkin(acc, skinType, skinDesc.Cost);
                Write(context, "<Success />");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
