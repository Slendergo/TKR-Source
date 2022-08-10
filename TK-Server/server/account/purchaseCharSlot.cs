using Anna.Request;
using common.database;
using common.resources;
using common.utils;
using NLog;
using StackExchange.Redis;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System;


namespace server.account
{
    internal class purchaseCharSlot : RequestHandler
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();


        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = _db.Verify(query["guid"], query["password"], out var acc);
            if (status == DbLoginStatus.OK)
            {
                using (var l = _db.Lock(acc))
                {
                    if (!_db.LockOk(l))
                    {
                        Write(context, "<Error>Account in use</Error>");
                        return;
                    }

                    var currency = Program.Resources.Settings.NewAccounts.SlotCurrency;
                    var price = Program.Resources.Settings.NewAccounts.SlotCost;

                    acc.Reload("fame");
                    acc.Reload("totalFame");

                    if (currency == CurrencyType.Gold && acc.Credits < price || currency == CurrencyType.Fame && acc.Fame < price)
                    {
                        Write(context, "<Error>Insufficient funds</Error>");
                        return;
                    }

                    var trans = _db.Conn.CreateTransaction();
                    var t1 = _db.UpdateCurrency(acc, -price, currency, trans);
                    trans.AddCondition(Condition.HashEqual(acc.Key, "maxCharSlot", acc.MaxCharSlot));
                    trans.HashIncrementAsync(acc.Key, "maxCharSlot");
                    var t2 = trans.ExecuteAsync();

                    Task.WhenAll(t1, t2).ContinueWith(r =>
                    {
                        if (t2.IsCanceled || !t2.Result)
                        {
                            Write(context, "<Error>Internal Server Error</Error>");
                            return;
                        }

                        acc.MaxCharSlot++;
                        Write(context, "<Success />");
                    }).ContinueWith(e =>
                        Log.Error(e.Exception.InnerException.ToString()),
                        TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
