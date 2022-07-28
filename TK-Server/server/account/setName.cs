using Anna.Request;
using common.database;
using common.utils;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace server.account
{
    internal class setName : RequestHandler
    {
        private const int PRICE = 100;

        private bool IsValid(string text)
        {
            var nonDup = new Regex(@"([a-zA-z]{2,})\1{1,}");
            var alpha = new Regex(@"^[A-Za-z]{1,10}$");
            return !(nonDup.Matches(text).Count > 0) && alpha.Matches(text).Count > 0;
        }

        public override async void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var name = query["name"];
            if (Database.GuestNames.Contains(name, StringComparer.InvariantCultureIgnoreCase) || !IsValid(name))
            {
                Write(context, "<Error>Invalid name</Error>");
                return;
            }

            if (Program.Config.discordIntegration.botName.Equals(name))
            {
                Write(context, "<Error>This is the BOT name</Error>");
                return;
            }

            foreach (string i in Database.BlackListedNames)
                if (name.ToLower() == i.ToLower())
                {
                    Write(context, "<Error>Invalid Name</Error>");
                    return;
                }

            var key = Database.NAME_LOCK;
            var lockToken = "";
            var hasName = _db.Conn.HashExists("names", name.ToUpperInvariant());

            try
            {
                while ((lockToken = _db.AcquireLock(key)) == null)
                    await Task.Delay(1000);

                if (hasName)
                {
                    Write(context, "<Error>Duplicated name</Error>");
                    return;
                }

                var guid = query["guid"];
                var password = query["password"];
                var status = _db.Verify(guid, password, out DbAccount acc);
                if (status == DbLoginStatus.OK)
                {
                    bool isRenamed() => _db.RenameIGN(acc, name, lockToken);
                    using (var l = _db.Lock(acc))
                        if (_db.LockOk(l))
                        {
                            if (!acc.NameChosen)
                            {
                                if (isRenamed())
                                    Write(context, "<Success />");
                                else
                                    Write(context, "<Error>Unable to change name</Error>");
                            }
                            else
                            {
                                if (acc.Credits < PRICE)
                                    Write(context, "<Error>Not enough credits</Error>");
                                else
                                {
                                    if (isRenamed())
                                    {
                                        await _db.UpdateCredit(acc, -PRICE);
                                        Write(context, "<Success />");
                                    }
                                    else
                                        Write(context, "<Error>Unable to change name</Error>");
                                }
                            }
                        }
                        else
                            Write(context, "<Error>Account in use</Error>");
                }
                else
                    Write(context, "<Error>" + status.GetInfo() + "</Error>");
            }
            finally
            {
                if (lockToken != null)
                    _db.ReleaseLock(key, lockToken);
            }
        }
    }
}
