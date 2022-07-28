using Newtonsoft.Json;

namespace common.discord
{
    public struct DiscordThumbnail
    {
        [JsonProperty("url")] public string Url;
    }
}
