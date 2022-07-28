using Anna.Request;
using common.database;
using common.utils;
using NLog;
using System.Collections.Specialized;

namespace server.account
{
    internal class changePassword : RequestHandler
    {
        private static readonly Logger PassLog = LogManager.GetCurrentClassLogger();

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = _db.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == DbLoginStatus.OK)
            {
                _db.ChangePassword(query["guid"], query["newPassword"]);
                Write(context, "<Success />");
                PassLog.Info($"Password changed. IP: {context.Request.ClientIP()}, Account: {acc.Name} ({acc.AccountId})");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
