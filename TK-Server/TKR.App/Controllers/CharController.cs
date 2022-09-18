using Microsoft.AspNetCore.Mvc;
using TKR.App.Database;
using TKR.App.Database.Models;

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

            var status = _database.ValidateLogin(guid, password);
            switch (status)
            {
                case LoginModelResult.OK:
                    {
                        var accountModel = _database.GetAccount(guid);
                        if (accountModel != null)
                        {
                            // load the rest



                            return;
                        }
                        Response.CreateError("Account credentials not valid");
                    }
                    break;
                case LoginModelResult.AccountNotExists:
                    Response.CreateXml(RespondGuest());
                    break;
                case LoginModelResult.InvalidCredentials:
                    Response.CreateError("Account credentials not valid");
                    break;
            }
        }

        private static string RespondGuest() => @"<Chars nextCharId=""1"" maxNumChars=""1"">
    <Account>
        <Credits>100</Credits>
        <NextCharSlotPrice>600</NextCharSlotPrice>
        <AccountId>-1</AccountId>
        <Name>Orothi</Name>
        <BeginnerPackageTimeLeft>604800</BeginnerPackageTimeLeft>
        <IsAgeVerified>0</IsAgeVerified>
        <PetYardType>1</PetYardType>
        <isFirstDeath />
        <Stats>
            <BestCharFame>0</BestCharFame>
            <TotalFame>0</TotalFame>
            <Fame>0</Fame>
        </Stats>
    </Account>
    <News/>
    <Servers/>
    <ClassAvailabilityList>
        <ClassAvailability id=""Rogue"">available</ClassAvailability>
        <ClassAvailability id=""Assassin"">available</ClassAvailability>
        <ClassAvailability id=""Huntress"">available</ClassAvailability>
        <ClassAvailability id=""Mystic"">available</ClassAvailability>
        <ClassAvailability id=""Trickster"">available</ClassAvailability>
        <ClassAvailability id=""Sorcerer"">available</ClassAvailability>
        <ClassAvailability id=""Ninja"">unavailable</ClassAvailability>
        <ClassAvailability id=""Archer"">available</ClassAvailability>
        <ClassAvailability id=""Wizard"">available</ClassAvailability>
        <ClassAvailability id=""Priest"">available</ClassAvailability>
        <ClassAvailability id=""Necromancer"">available</ClassAvailability>
        <ClassAvailability id=""Warrior"">available</ClassAvailability>
        <ClassAvailability id=""Knight"">available</ClassAvailability>
        <ClassAvailability id=""Paladin"">available</ClassAvailability>
    </ClassAvailabilityList>
</Chars>";
    }
}