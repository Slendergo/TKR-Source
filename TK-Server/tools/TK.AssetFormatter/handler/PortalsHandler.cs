using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class PortalsHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var assets = new Dictionary<string, IEnumerable<XElement>>()
            {
                ["Portal"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Portal"),
                ["ArenaPortal"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "ArenaPortal"),
                ["GuildHallPortal"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "GuildHallPortal")
            };
            var amount = 0;

            AssetFormatter.ExportAssets("Portals", "Objects", assets, ref amount);

            Log.Info($"\t{amount} portal{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
