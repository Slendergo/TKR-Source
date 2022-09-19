using Newtonsoft.Json;

namespace TKR.Shared.discord
{
    public struct DiscordEmbedBuilder
    {
        [JsonProperty("avatar_url")] public string Avatar;
        [JsonProperty("content")] public string Content;
        [JsonProperty("embeds")] public DiscordEmbed[] Embeds;
        [JsonProperty("username")] public string Username;
    }
}
