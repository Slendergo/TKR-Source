using TKR.Shared.discord.model;

namespace TKR.Shared.discord
{
    public class DiscordIntegration
    {
        public string botName { get; set; }
        public string botToken { get; set; } = "";
        public ClassModel[] classes { get; set; } = new ClassModel[0];
        public string lgBagImage { get; set; } = "";
        public string lgImage { get; set; } = "";
        public string mtBagImage { get; set; } = "";
        public string ripIco { get; set; } = "";
        public string mtImage { get; set; } = "";
        public StarModel[] stars { get; set; } = new StarModel[0];
        public string webhookBotImage { get; set; } = "";
        public string webhookLogoImage { get; set; } = "";
        public string webhookLootEvent { get; set; } = "";
        public string webhookOryx1Image { get; set; } = "";
        public string webhookOryx2Image { get; set; } = "";
        public string webhookRealmEvent { get; set; } = "";
        public string webhookDeathEvent { get; set; } = "";
        public string webhookResourcesURL { get; set; } = "";
    }
}
