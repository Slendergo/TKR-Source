using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class GameObjectsHandler : IHandler
    {
        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "GameObject");
            var assets = new Dictionary<string, IList<XElement>>();

            foreach (var node in nodes)
            {
                var id = node.GetAttribute<string>("id");

                try
                {
                    string key;

                    if (node.HasElement("AnimatedTexture"))
                    {
                        var animatedTexture = node.Element("AnimatedTexture");

                        key = animatedTexture.GetValue<string>("File");
                    }
                    else if (node.HasElement("RandomTexture"))
                    {
                        var randomTexture = node.Element("RandomTexture");

                        key = randomTexture.Element("Texture").GetValue<string>("File");
                    }
                    else if (node.HasElement("Texture"))
                        key = node.Element("Texture").GetValue<string>("File");
                    else if (node.HasElement("Model"))
                        key = node.GetValue<string>("Model").Replace(" ", "_");
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

            AssetFormatter.ExportAssets("GameObjects", "Objects", assets, ref amount);

            Log.Info($"\t{amount} game object{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
