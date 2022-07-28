using Newtonsoft.Json;

namespace common.discord
{
    public struct DiscordImage
    {
        [JsonProperty("url")] public string Url;
    }
}
