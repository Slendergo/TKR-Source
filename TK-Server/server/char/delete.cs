using Anna.Request;
using common.database;
using common.utils;
using System.Collections.Specialized;

namespace server.@char
{
    internal class delete : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var status = _db.Verify(query["guid"], query["password"], out DbAccount acc);
            if (status == DbLoginStatus.OK)
            {
                using (var l = _db.Lock(acc))
                    if (_db.LockOk(l))
                    {
                        _db.DeleteCharacter(acc, int.Parse(query["charId"]));
                        Write(context, "<Success />");
                    }
                    else
                        Write(context, "<Error>Account in Use</Error>");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
