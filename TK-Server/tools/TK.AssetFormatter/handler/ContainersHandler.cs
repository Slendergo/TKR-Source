using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class ContainersHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var assets = new Dictionary<string, IEnumerable<XElement>>()
            {
                ["Container"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Container"),
                ["OneWayContainer"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "OneWayContainer"),
                ["ClosedVaultChest"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "ClosedVaultChest"),
                ["ClosedGiftChests"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "ClosedGiftChests")
            };
            var amount = 0;

            AssetFormatter.ExportAssets("Containers", "Objects", assets, ref amount);

            Log.Info($"\t{amount} container{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
