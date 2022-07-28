using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class NPCsHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var assets = new Dictionary<string, IEnumerable<XElement>>()
            {
                ["Stat"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "StatNPC"),
                ["Market"] = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "MarketNPC")
            };
            var amount = 0;

            AssetFormatter.ExportAssets("NPCs", "Objects", assets, ref amount);

            Log.Info($"\t{amount} npc{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
