using Anna.Request;
using common;
using common.resources;
using System.Collections.Specialized;
using System.Text;

namespace server.app
{
    internal class globalNews : RequestHandler
    {
        private static byte[] _data;

        public override void InitHandler(Resources resources)
        {
            string data = Utils.Read(resources.ResourcePath + "/data/globalNews.json");
            _data = Encoding.UTF8.GetBytes(data);
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            if (_data == null)
                InitHandler(Program.Resources);

            Write(context, _data);
        }
    }
}
