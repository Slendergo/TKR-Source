using Anna.Request;
using common.resources;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace server.app
{
    internal class init : RequestHandler
    {
        private static byte[] _data;

        public override void InitHandler(Resources resources)
        {
            var root = XElement.Parse(File.ReadAllText(resources.ResourcePath + "/data/init.xml"));
            _data = Encoding.UTF8.GetBytes(root.ToString());
        }

        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            if (_data == null)
                InitHandler(Program.Resources);

            Write(context, _data);
        }
    }
}
