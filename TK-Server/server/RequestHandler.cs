using Anna.Request;
using Anna.Responses;
using common.database;
using common.resources;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace server
{
    internal static class RequestHandlers
    {
        public static readonly Dictionary<string, RequestHandler> Get = new Dictionary<string, RequestHandler>
        {
            {"/app/bot", new app.bot()}
        };

        public static readonly Dictionary<string, RequestHandler> Post = new Dictionary<string, RequestHandler>
        {
            {"/char/list", new @char.list()},
            {"/char/delete", new @char.delete()},
            {"/char/fame", new @char.fame()},
            {"/char/purchaseClassUnlock", new @char.purchaseClassUnlock()},
            {"/account/verify", new account.verify()},
            {"/account/register", new account.register()},
            {"/account/changePassword", new account.changePassword()},
            {"/account/passwordRecovery", new account.passwordRecovery()},
            {"/account/purchaseCharSlot", new account.purchaseCharSlot()},
            {"/account/purchaseSkin", new account.purchaseSkin()},
            {"/account/setName", new account.setName()},
            {"/fame/list", new fame.list()},
            {"/app/init", new app.init()},
            {"/app/globalNews", new app.globalNews()},
            {"/guild/listMembers", new guild.listMembers()},
            {"/guild/getBoard", new guild.getBoard()},
            {"/guild/setBoard", new guild.setBoard()},
            {"/char/getServerXmls", new @char.getServerXmls()}
        };

        public static void Initialize(Resources resources)
        {
            foreach (var h in Get)
                h.Value.InitHandler(resources);

            InitWebFiles(resources);
        }

        private static void InitWebFiles(Resources resources)
        {
            foreach (var f in resources.WebFiles)
                Get[f.Key] = new StaticFile(f.Value, MimeMapping.GetMimeMapping(f.Key));
        }
    }

    internal abstract class RequestHandler
    {
        protected Database _db => Program.Database;

        public abstract void HandleRequest(RequestContext context, NameValueCollection query);

        public virtual void InitHandler(Resources resources)
        { }

        internal void Write(RequestContext req, string val) => Write(req.Response(val), "text/plain");

        internal void Write(RequestContext req, byte[] val) => Write(req.Response(val), "text/plain");

        internal void Write(Response r, string type)
        {
            r.Headers["Content-Type"] = type;
            r.Send();
        }

        internal void WriteImg(RequestContext req, byte[] val) => Write(req.Response(val), "image/png");

        internal void WriteSnd(RequestContext req, byte[] val) => Write(req.Response(val), "*/*");

        internal void WriteXml(RequestContext req, string val) => Write(req.Response(val), "application/xml");

        internal void WriteXml(RequestContext req, byte[] val) => Write(req.Response(val), "application/xml");
    }
}
