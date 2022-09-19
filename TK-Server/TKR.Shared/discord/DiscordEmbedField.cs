using Newtonsoft.Json;

namespace TKR.Shared.discord
{
    public struct DiscordEmbedField
    {
        [JsonProperty("inline")] public bool InLine;
        [JsonProperty("name")] public string Name;
        [JsonProperty("value")] public string Value;
    }
}
