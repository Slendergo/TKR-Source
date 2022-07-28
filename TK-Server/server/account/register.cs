using Anna.Request;
using common;
using common.database;
using common.utils;
using System.Collections.Specialized;

namespace server.account
{
    internal class register : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            if (!Utils.IsValidEmail(query["newGUID"]))
                Write(context, "<Error>Invalid email</Error>");
            else
            {
                var key = Database.REG_LOCK;
                var lockToken = "";
                try
                {
                    while ((lockToken = _db.AcquireLock(key)) == null) ;

                    var status = _db.Verify(query["guid"], "", out DbAccount acc);
                    if (status == DbLoginStatus.OK)
                    {
                        //what? can register in game? kill the account lock
                        if (!_db.RenameUUID(acc, query["newGUID"], lockToken))
                        {
                            Write(context, "<Error>Duplicate Email</Error>");
                            return;
                        }
                        _db.ChangePassword(acc.UUID, query["newPassword"]);
                        _db.Guest(acc, false);
                        Write(context, "<Success />");
                    }
                    else
                    {
                        var s = _db.Register(query["newGUID"], query["newPassword"], false, out acc);
                        if (s == RegisterStatus.OK)
                            Write(context, "<Success />");
                        else
                            Write(context, "<Error>" + s.GetInfo() + "</Error>");
                    }
                }
                finally
                {
                    if (lockToken != null)
                        _db.ReleaseLock(key, lockToken);
                }
            }
        }
    }
}
