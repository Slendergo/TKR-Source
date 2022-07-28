using Anna.Request;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.@char
{
    class getServerXmls : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            //send the bytes
            WriteSnd(context, Program.Resources.GameData.ZippedXMLS);
        }
    }
}
