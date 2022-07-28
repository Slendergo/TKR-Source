using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class SkinsHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Skin");

            AssetFormatter.ExportAsset("Objects", nodes, "Skins", out var amount);

            Log.Info($"\t{amount} skin{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
