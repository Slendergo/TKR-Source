using Anna.Request;
using common;
using common.database;
using common.utils;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace server.account
{
    internal class register : RequestHandler
    {
        private bool IsValidName(string text)
        {
            var nonDup = new Regex(@"([a-zA-z]{2,})\1{1,}");
            var alpha = new Regex(@"^[A-Za-z]{1,10}$");
            return !(nonDup.Matches(text).Count > 0) && alpha.Matches(text).Count > 0;
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {

            if (Program.Config.serverInfo.requireSecret && query["secret"] != "69420")
            {
                System.Console.WriteLine(query["newGUID"] + " Tried to register without secret");
                Write(context, "<Error>Internal Server Error</Error>");
                return;
            }

            if (Program.Resources.Settings.NewAccounts.TestingRegister)
            {
                Write(context, "<Error>Testers Only</Error>");
                return;
            }

            var name = query["name"];
            if (Database.GuestNames.Contains(name, StringComparer.InvariantCultureIgnoreCase) || !IsValidName(name))
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

            if (!Utils.IsValidEmail(query["newGUID"]))
                Write(context, "<Error>Invalid email</Error>");
            else
            {
                var key = Database.REG_LOCK;
                var lockToken = "";
                try
                {
                    while ((lockToken = _db.AcquireLock(key)) == null) ;

                    string nameLockToken = null;

                    var nameKey = Database.NAME_LOCK;

                    try
                    {
                        while ((nameLockToken = _db.AcquireLock(nameKey)) == null) ;

                        if (_db.Conn.HashExists("names", name.ToUpperInvariant()))
                        {
                            Write(context, "<Error>Duplicated name</Error>");
                            return;
                        }

                        var s = _db.Register(query["newGUID"], query["newPassword"], false, out var acc);
                        if (s == DbRegisterStatus.OK)
                        {
                            while (!_db.RenameIGN(acc, name, nameLockToken)) ;

                            Write(context, "<Success />");
                        }
                        else
                            Write(context, "<Error>" + s.GetInfo() + "</Error>");
                        }
                    finally
                    {
                        if (nameLockToken != null)
                            _db.ReleaseLock(nameKey, nameLockToken);
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
