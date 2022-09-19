using Newtonsoft.Json;

namespace TKR.Shared.discord
{
    public struct DiscordEmbedAuthor
    {
        [JsonProperty("icon_url")] public string Icon;
        [JsonProperty("name")] public string Name;
    }
}
