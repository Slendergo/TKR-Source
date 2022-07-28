using Newtonsoft.Json;

namespace common.discord
{
    public struct DiscordEmbed
    {
        [JsonProperty("author")] public DiscordEmbedAuthor Author;
        [JsonProperty("color")] public int Color;
        [JsonProperty("description")] public string Description;
        [JsonProperty("fields")] public DiscordEmbedField[] Fields;
        [JsonProperty("footer")] public DiscordEmbedFooter Footer;
        [JsonProperty("image")] public DiscordImage Image;
        [JsonProperty("thumbnail")] public DiscordThumbnail Thumbnail;
        [JsonProperty("title")] public string Title;
    }
}
