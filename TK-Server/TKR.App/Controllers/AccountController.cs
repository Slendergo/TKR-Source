using Microsoft.AspNetCore.Mvc;
using TKR.App.Database;
using TKR.App.Database.Models;

namespace TKR.App.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly RedisDatabase _database;

        public AccountController(ILogger<AccountController> logger, RedisDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpPost("verify")]
        public void AccountVerify([FromForm] string guid, [FromForm] string password)
        {
            var status = _database.ValidateLogin(guid, password);
            switch (status)
            {
                case LoginModelResult.OK:
                    Response.CreateSuccess("Unknown Error");
                    break;
                case LoginModelResult.AccountNotExists:
                    Response.CreateError("Unknown Error");
                    break;
                case LoginModelResult.InvalidCredentials:
                    Response.CreateError("Unknown Error");
                    break;
            }
        }

        [HttpPost("register")]
        public void AccountRegister([FromForm] string guid, [FromForm] string newGuid, [FromForm] string newPassword, [FromForm] string name)
        {
            _logger.LogInformation(guid + " " + newGuid + " " + newPassword + " " + name);
            Response.CreateError("Unknown Error");
        }
    }
}