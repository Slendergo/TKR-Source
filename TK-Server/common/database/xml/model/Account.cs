using common.database.model;
using common.resources;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct Account
    {
        public int AccountId { get; private set; }
        public bool Admin { get; private set; }
        public int Credits { get; private set; }
        public bool FirstDeath { get; private set; }
        public GuildIdentity Guild { get; private set; }
        public string Name { get; set; }
        public bool NameChosen { get; private set; }
        public int NextCharSlotCurrency { get; private set; }
        public int NextCharSlotPrice { get; private set; }
        public ushort[] Skins { get; private set; }
        public Stats Stats { get; private set; }
        public Vault Vault { get; private set; }

        public static async Task<Account> SerializeAsync(RedisDb redis, Resources resources, AccountModel model)
        {
            var vault = new VaultModel();
            var stats = new ClassStatsModel();
            var guild = new GuildModel();

            await vault.InitAsync(redis.Db, model.Id.ToString());
            await stats.InitAsync(redis.Db, model.Id.ToString());
            await guild.InitAsync(redis.Db, model.GuildId.ToString());

            return new Account()
            {
                AccountId = model.Id,
                Name = model.Name,
                NameChosen = model.IsNameChosen,
                Admin = model.IsAdmin,
                FirstDeath = model.IsFirstDeath,
                Credits = model.Gold,
                NextCharSlotPrice = resources.Settings.NewAccounts.SlotCost,
                NextCharSlotCurrency = (int)resources.Settings.NewAccounts.SlotCurrency,
                Vault = Vault.Serialize(model, vault),
                Stats = Stats.Serialize(resources, model, stats),
                Guild = GuildIdentity.Serialize(model, guild),
                Skins = model.Skins ?? new ushort[0]
            };
        }

        public XElement ToXml() => new XElement("Account",
            new XElement("AccountId", AccountId),
            new XElement("Name", Name),
            NameChosen ? new XElement("NameChosen", "") : null,
            Admin ? new XElement("Admin", "") : null,
            FirstDeath ? new XElement("isFirstDeath", "") : null,
            new XElement("Credits", Credits),
            new XElement("NextCharSlotPrice", NextCharSlotPrice),
            new XElement("NextCharSlotCurrency", NextCharSlotCurrency),
            Vault.ToXml(),
            Stats.ToXml(),
            Guild.ToXml()
        );
    }
}
