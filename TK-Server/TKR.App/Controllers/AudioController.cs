using Microsoft.AspNetCore.Mvc;

namespace TKR.App.Controllers
{
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly ILogger<AudioController> _logger;
        public AudioController(ILogger<AudioController> logger) => _logger = logger;

        [Route("sfx/{type}")]
        [Route("music/{type}")]
        public void GetSFX(string type)
        {
            var path = $"{Program.ResourcePath}/{ControllerContext.ActionDescriptor.AttributeRouteInfo.Template.Replace("{type}", type)}";
            if (System.IO.File.Exists(path))
                Response.CreateBytes(System.IO.File.ReadAllBytes(path));
        }
    }
}