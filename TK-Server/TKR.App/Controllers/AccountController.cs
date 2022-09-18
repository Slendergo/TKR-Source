using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TKR.App.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger) => _logger = logger;

        [HttpPost("verify")]
        public void AccountVerify([FromForm] string guid, [FromForm] string password)
        {

        }

        [HttpPost("register")]
        public void AccountRegister([FromForm] string guid, [FromForm] string newGuid, [FromForm] string newPassword, [FromForm] string name)
        {

        }
    }
}