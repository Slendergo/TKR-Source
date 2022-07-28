using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class LoadHandler : IHandler
    {
        public async Task ExecuteAsync(params string[] args)
        {
            var input = args[0];

            if (string.IsNullOrWhiteSpace(input))
            {
                Log.Error("Empty directory path!");
                return;
            }

            if (!Directory.Exists(input))
            {
                Log.Error($"There is no directory for following path:\n\t{input}");
                return;
            }

            var xmls = Directory.GetFiles(input, "*.xml", SearchOption.AllDirectories);

            if (xmls.Length == 0)
            {
                Log.Error($"No XML found into path:\n\t{input}");
                return;
            }

            Log.Warn($"Path:");
            Log.Info($"\t{input}");
            Log.Warn($"Loading {xmls.Length} XML{(xmls.Length > 1 ? "s" : "")}:");

            var assets = new List<XElement>();

            for (var i = 0; i < xmls.Length; i++)
            {
                Log.Info($"\tReading XML -> {xmls[i].Replace(input, "..")}");

                var file = File.ReadAllText(xmls[i]);
                var asset = XElement.Parse(file);

                assets.Add(asset);
            }

            var groundWorkers = new Task<XElement[]>[assets.Count];
            var objectWorkers = new Task<XElement[]>[assets.Count];

            for (var i = 0; i < groundWorkers.Length; i++)
            {
                groundWorkers[i] = assets[i].FetchNodesAsync("Ground");
                objectWorkers[i] = assets[i].FetchNodesAsync("Object");
            }

            Log.Warn("Added:");

            var groundTask = await Task.WhenAll(groundWorkers);
            var eligibleGrounds = groundTask.Where(result => result.Length != 0).ToArray();
            var groundNodes = new List<XElement>();

            for (var i = 0; i < eligibleGrounds.Length; i++)
                groundNodes.AddRange(eligibleGrounds[i]);

            var amountGrounds = groundNodes.Count;

            Log.Info($"\t{amountGrounds} ground{(amountGrounds > 1 ? "s" : "")}");

            var objectTask = await Task.WhenAll(objectWorkers);
            var eligibleObjects = objectTask.Where(result => result.Length != 0).ToArray();
            var objectNodes = new List<XElement>();

            for (var i = 0; i < eligibleObjects.Length; i++)
                objectNodes.AddRange(eligibleObjects[i]);

            var amountObjects = objectNodes.Count;

            Log.Info($"\t{amountObjects} object{(amountObjects > 1 ? "s" : "")}");

            var grounds = new XElement("Grounds", groundNodes);
            var objects = new XElement("Objects", objectNodes);

            AssetFormatter.Assets["grounds"] = grounds;
            AssetFormatter.Assets["objects"] = objects;
        }
    }
}
