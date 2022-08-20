using Anna.Request;
using System.Collections.Specialized;

namespace server.account
{
    internal class setName : RequestHandler
    {
        public override async void HandleRequest(RequestContext context, NameValueCollection query)
        {
            Write(context, "<Error>Unknown Error</Error>");
        }
    }
}
