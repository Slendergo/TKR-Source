using System.Threading.Tasks;

namespace tk.assetformatter.handler
{
    public sealed class UnknownHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["unknown"];

            AssetFormatter.ExportAsset("Objects", nodes.Elements(), "Unknown", out var amount);

            Log.Info($"\t{amount} unknown{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
