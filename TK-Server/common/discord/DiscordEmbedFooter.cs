using Newtonsoft.Json;

namespace common.discord
{
    public struct DiscordEmbedFooter
    {
        [JsonProperty("icon_url")] public string Icon;
        [JsonProperty("text")] public string Text;
    }
}
