using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class EquipmentsHandler : IHandler
    {
        private static readonly Dictionary<int, string> _slotTypes = new Dictionary<int, string>()
        {
            [0] = "Any",
            [1] = "Sword",
            [2] = "Dagger",
            [3] = "Bow",
            [4] = "Tome",
            [5] = "Shield",
            [6] = "HideArmor",
            [7] = "HeavyArmor",
            [8] = "Wand",
            [9] = "Ring",
            [10] = "Miscellaneous",
            [11] = "Spell",
            [12] = "Seal",
            [13] = "Cloak",
            [14] = "Robe",
            [15] = "Quiver",
            [16] = "Helm",
            [17] = "Staff",
            [18] = "Poison",
            [19] = "Skull",
            [20] = "Trap",
            [21] = "Orb",
            [22] = "Prism",
            [23] = "Scepter",
            [24] = "Katana",
            [25] = "Star"
        };

        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["objects"].GetXmlsByNode("Class", "Equipment");
            var assets = new Dictionary<string, IList<XElement>>();

            foreach (var node in nodes)
            {
                var id = node.GetAttribute<string>("id");

                try
                {
                    var slotType = node.GetValue<int>("SlotType");

                    if (!_slotTypes.ContainsKey(slotType))
                        throw new NullReferenceException();

                    if (slotType == 10)
                        slotType = 0;

                    if (assets.ContainsKey(_slotTypes[slotType]))
                        assets[_slotTypes[slotType]].Add(node);
                    else
                        assets[_slotTypes[slotType]] = new List<XElement>() { node };
                }
                catch (NullReferenceException) { Log.Warn($"\t[{GetType().Name}]: Unable to parse -> ID '{id}'!"); }
                catch (Exception e) { Log.Error($"\t[{GetType().Name}]: Error!", e); }
            }

            var assetCategories = new Dictionary<string, IList<XElement>>();

            foreach (var asset in assets)
            {
                for (var i = 0; i < asset.Value.Count; i++)
                {
                    var node = asset.Value[i];
                    var id = node.GetAttribute<string>("id");

                    try
                    {
                        string category;

                        if (node.HasElement("Mythical") || node.HasElement("Revenge"))
                            category = "Mythical";
                        else if (node.HasElement("Legendary"))
                            category = "Legendary";
                        else if (node.HasElement("Tier"))
                            category = "Tiered";
                        else if (asset.Key != _slotTypes[0])
                            category = "Untiered";
                        else
                            category = "Regular";

                        var key = $"{asset.Key}_{category}";

                        if (assetCategories.ContainsKey(key))
                            assetCategories[key].Add(node);
                        else
                            assetCategories[key] = new List<XElement>() { node };
                    }
                    catch (NullReferenceException) { Log.Warn($"\t[{GetType().Name}]: Unable to parse category -> ID '{id}'!"); }
                    catch (Exception e) { Log.Error($"\t[{GetType().Name}]: Category Error!", e); }
                }
            }

            var amount = 0;

            AssetFormatter.ExportAssets("Equipments", "Objects", assetCategories, ref amount);

            Log.Info($"\t{amount} equipment{(amount > 1 ? "s" : "")}");

            return Task.CompletedTask;
        }
    }
}
