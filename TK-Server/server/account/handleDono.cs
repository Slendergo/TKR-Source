using Anna.Request;
using common.database;
using common.utils;
using System.Collections.Specialized;
using wServer.core;
using wServer.core.objects;

namespace server.account
{
    internal class handleDono : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var secret = "2e97e994e19e630da90092ca4ffcd9ee";
            if (query["secret"] == secret)
            {
                var acc = _db.GetAccount(int.Parse(query["accountId"]));
                System.Console.WriteLine("post accId: " + query["accountId"]);
                System.Console.WriteLine("acc accId: " + acc.AccountId);
                var rank = new DbRank(acc.Database, acc.AccountId);

                rank.NewAmountDonated += int.Parse(query["amountDonated"]);

                rank.Flush();
                Write(context, "<Success />");
            }
            else
                Write(context, "<Error>Internal Server Error.</Error>");
        }
    }
}
