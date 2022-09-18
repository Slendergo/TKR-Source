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
        private readonly DatabaseService _database;

        public AccountController(ILogger<AccountController> logger, DatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpPost("verify")]
        public void Verify([FromForm] string guid, [FromForm] string password)
        {
            var status = _database.ValidateLogin(guid, password);
            switch (status)
            {
                case LoginModelResult.OK:
                    Response.CreateXml("Success");
                    break;
                case LoginModelResult.AccountNotExists:
                case LoginModelResult.InvalidCredentials:
                    Response.CreateError("Account credentials not valid");
                    break;
            }
        }

        [HttpPost("register")]
        public void Register([FromForm] string guid, [FromForm] string newGuid, [FromForm] string newPassword, [FromForm] string name)
        {
            _logger.LogInformation(guid + " " + newGuid + " " + newPassword + " " + name);
            Response.CreateError("Unknown Error");
        }
    }
}