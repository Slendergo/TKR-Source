using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class DyesHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Dye");

            AssetFormatter.ExportAsset("Objects", nodes, "Dyes", out var amount);

            Log.Info($"\t{amount} dye{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
