using Newtonsoft.Json;

namespace TKR.Shared.discord
{
    public struct DiscordThumbnail
    {
        [JsonProperty("url")] public string Url;
    }
}
