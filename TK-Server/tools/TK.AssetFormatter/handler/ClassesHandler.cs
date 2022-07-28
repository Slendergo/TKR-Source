using System.Threading.Tasks;

namespace tk.assetformatter.handler
{
    public sealed class ClassesHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Player");

            AssetFormatter.ExportAsset("Objects", nodes, "Classes", out var amount);

            Log.Info($"\t{amount} class{(amount > 1 ? "es" : "")}");

            return Task.CompletedTask;
        }
    }
}
