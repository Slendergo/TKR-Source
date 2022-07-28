using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tk.assetformatter.handler
{
    public sealed class DisplayClassesHandler : IHandler
    {
        private static string[] _handledTypes => new[]
        {
            "Character", "GameObject", "Equipment", "Player", "Portal", "Forge",
            "SkillTree", "MarketNPC", "StatNPC", "BountyBoard", "GuildLeaderboard",
            "Projectile", "Container", "ClosedVaultChest", "OneWayContainer", "ClosedGiftChest",
            "Dye", "Wall", "DoubleWall", "ArenaPortal", "Merchant", "GuildHallPortal", "GuildMerchant",
            "ConnectedWall", "Sign", "MoneyChanger", "CharacterChanger", "Stalagmite", "CaveWall",
            "NameChanger", "GuildRegister", "GuildChronicle", "GuildBoard", "ReskinVendor", "ArenaGuard",
            "SpiderWeb", "Skin"
        };

        public Task ExecuteAsync(params string[] args)
        {
            var nodes = AssetFormatter.Assets["objects"];
            var types = new List<string>();

            foreach (var node in nodes.Elements())
            {
                var type = node.GetValue<string>("Class");

                if (string.IsNullOrWhiteSpace(type) || !_handledTypes.Contains(type))
                {
                    var id = node.GetAttribute<string>("id");

                    Log.Error($"\tUnhandled class type -> ID '{id}' class: {(string.IsNullOrWhiteSpace(type) ? "Unregistered" : type)}");

                    type = "Unknown";

                    if (AssetFormatter.Assets.ContainsKey("unknown"))
                        AssetFormatter.Assets["unknown"].Add(node);
                    else
                        AssetFormatter.Assets["unknown"] = new XElement("Objects", node);
                }

                if (!types.Contains(type))
                    types.Add(type);
            }

            Log.Warn($"Found {types.Count} type{(types.Count > 1 ? "s" : "")}:");

            for (var i = 0; i < types.Count; i++)
                Log.Info($"\t{types[i]}");

            return Task.CompletedTask;
        }
    }
}
