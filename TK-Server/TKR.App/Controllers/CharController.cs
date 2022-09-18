using Microsoft.AspNetCore.Mvc;
using TKR.App.Database;

namespace TKR.App.Controllers
{
    [ApiController]
    [Route("char")]
    public class CharController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostingEnvronment;
        private readonly ILogger<CharController> _logger;
        private readonly DatabaseService _database;
        private readonly string _secret;

        public CharController(IWebHostEnvironment webHostingEnvronment, ILogger<CharController> logger, IConfiguration configuration, DatabaseService database)
        {
            _webHostingEnvronment = webHostingEnvronment;
            _logger = logger;
            _database = database;
            _secret = configuration.GetConnectionString("char_list_secrets");
        }

        [HttpPost("list")]
        public void List([FromForm] string guid, [FromForm] string password, [FromForm]string secret)
        {
            if (_webHostingEnvronment.IsProduction())
            {
                if (_secret != null && secret != _secret)
                {
                    Response.InternalServerError("Internal Server Error");
                    return;
                }
            }

            //var status = _database.Verify(query["guid"], query["password"], out var acc);
            //if (status == DbLoginStatus.OK || status == DbLoginStatus.AccountNotExists)
            //{
            //    if (status == DbLoginStatus.AccountNotExists)
            //        acc = _db.CreateGuestAccount(query["guid"]);

            //    var list = CharList.FromDb(_db, acc);
            //    list.Servers = Program.GetServerList();
            //    WriteXml(context, list.ToXml().ToString());
            //}
            //else
            //    Write(context, "<Error>" + status.GetInfo() + "</Error>");

            //var status = _database.ValidateLogin(guid, password);
            //switch (status)
            //{
            //    case LoginModelResult.OK:
            //        {
            //            if(_database.GetAccount(guid, out var accountId))
            //            {
            //                Response.CreateXml(RespondCharacter());
            //                return;
            //            }
            //            Response.CreateError("Account credentials not valid");
            //        }
            //        break;
            //    case LoginModelResult.AccountNotExists:
            //        Response.CreateXml(RespondGuest());
            //        break;
            //    case LoginModelResult.InvalidCredentials:
            //        Response.CreateError("Account credentials not valid");
            //        break;
            //}
        }
    }
}