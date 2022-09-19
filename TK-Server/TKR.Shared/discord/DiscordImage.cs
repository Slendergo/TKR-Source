using Newtonsoft.Json;

namespace TKR.Shared.discord
{
    public struct DiscordImage
    {
        [JsonProperty("url")] public string Url;
    }
}
