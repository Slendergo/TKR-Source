using Microsoft.AspNetCore.Mvc;
using TKR.App.Database;

namespace TKR.App.Controllers
{
    [ApiController]
    [Route("app")]
    public class AppController : ControllerBase
    {
        private readonly ILogger<AppController> _logger;

        public AppController(ILogger<AppController> logger)
        {
            _logger = logger;
        }


        [HttpPost("init")]
        public void Init() => Response.CreateBytes(ReadFile($"{Program.ResourcePath}/app/init.xml"));

        [HttpPost("globalNews")]
        public void GlobalNews() => Response.CreateBytes(ReadFile($"{Program.ResourcePath}/app/globalNews.json"));

        private static byte[] ReadFile(string path) => System.IO.File.ReadAllBytes(path);
    }
}