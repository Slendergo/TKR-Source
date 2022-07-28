using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class WallsHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var assets = new Dictionary<string, IEnumerable<XElement>>()
            {
                ["Wall"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Wall"),
                ["DoubleWall"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "DoubleWall"),
                ["ConnectedWall"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "ConnectedWall"),
                ["CaveWall"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "CaveWall")
            };
            var amount = 0;

            AssetFormatter.ExportAssets("Walls", "Objects", assets, ref amount);

            Log.Info($"\t{amount} wall{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
