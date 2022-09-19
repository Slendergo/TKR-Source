using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Xml.Linq;

namespace TKR.App.Controllers
{
    [ApiController]
    [Route("app")]
    public class AppController : ControllerBase
    {
        private readonly CoreService _core;

        public AppController(CoreService core)
        {
            _core = core;
        }

        [HttpPost("getLanguageStrings")]
        public void GetLanguageStrings()
        {
            Response.CreateError("Invalid Endpoint");
        }

        [HttpPost("serverList")]
        public void ServerList()
        {
            var servers = _core.GetServerList().Select(_ => _.ToXml()).ToList();
            if (servers.Any())
            {
                var response = new XElement("Servers", new XElement("Servers", servers)).ToString();
                Response.CreateXml(response.ToString());
                return;
            }
            Response.CreateError("No Servers");
        }

        [HttpPost("init")]
        public void Init() => Response.CreateBytes(ReadFile($"{_core.Resources.ResourcePath}/data/init.xml"));

        [HttpPost("globalNews")]
        public void GlobalNews() => Response.CreateBytes(ReadFile($"{_core.Resources.ResourcePath}/data/globalNews.json"));

        private static byte[] ReadFile(string path) => System.IO.File.ReadAllBytes(path);
    }
}