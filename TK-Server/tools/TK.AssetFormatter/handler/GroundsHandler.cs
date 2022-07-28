using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class GroundsHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["grounds"];
            var assets = new Dictionary<string, IList<XElement>>();

            foreach (var node in nodes.Elements())
            {
                var id = node.GetAttribute<string>("id");

                try
                {
                    string key;

                    if (node.HasElement("RandomTexture"))
                    {
                        var randomTexture = node.Element("RandomTexture");

                        key = randomTexture.Element("Texture").GetValue<string>("File");
                    }
                    else if (node.HasElement("Texture"))
                        key = node.Element("Texture").GetValue<string>("File");
                    else
                        throw new NullReferenceException();

                    if (assets.ContainsKey(key))
                        assets[key].Add(node);
                    else
                        assets[key] = new List<XElement>() { node };
                }
                catch (NullReferenceException) { Log.Warn($"\t[{GetType().Name}]: Unable to parse -> ID '{id}'!"); }
                catch (Exception e) { Log.Error($"\t[{GetType().Name}]: Error!", e); }
            }

            var amount = 0;

            AssetFormatter.ExportAssets("Grounds", "GroundTypes", assets, ref amount);

            Log.Info($"\t{amount} ground{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
