using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class MiscellaneousHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var assets = new Dictionary<string, IEnumerable<XElement>>()
            {
                ["Forge"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Forge"),
                ["SkillTree"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "SkillTree"),
                ["BountyBoard"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "BountyBoard"),
                ["Merchant"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Merchant"),
                ["GuildMerchant"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "GuildMerchant"),
                ["Sign"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Sign"),
                ["MoneyChanger"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "MoneyChanger"),
                ["CharacterChanger"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "CharacterChanger"),
                ["Stalagmite"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Stalagmite"),
                ["NameChanger"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "NameChanger"),
                ["GuildRegister"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "GuildRegister"),
                ["GuildChronicle"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "GuildChronicle"),
                ["GuildBoard"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "GuildBoard"),
                ["ReskinVendor"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "ReskinVendor"),
                ["ArenaGuard"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "ArenaGuard"),
                ["SpiderWeb"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "SpiderWeb"),
            };
            var amount = 0;

            AssetFormatter.ExportAssets("Miscellaneous", "Objects", assets, ref amount);

            Log.Info($"\t{amount} miscellaneous");

            return Task.CompletedTask;
        }
    }
}
