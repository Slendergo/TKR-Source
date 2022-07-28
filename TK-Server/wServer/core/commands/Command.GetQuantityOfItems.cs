using System.Xml;
using wServer.core.objects;
using wServer.utils;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class GetQuantityOfItems : Command
        {
            public GetQuantityOfItems() : base("getquantityofitems", permLevel: 110, alias: "gqof")
            { }

            protected override bool Process(Player player, TickData time, string type)
            {
                var slotTypes = new int[26];
                var totalItems = 0;

                using (var xmlRdr = new XmlTextReader(player.CoreServerManager.Resources.ResourcePath + "/xml/CustomItems.xml"))
                    while (xmlRdr.Read())
                        if (xmlRdr.Name.Equals("SlotType"))
                            for (var i = 0; i < slotTypes.Length; i++)
                                if (i == xmlRdr.ReadElementContentAsInt())
                                {
                                    slotTypes[i]++;
                                    totalItems++;
                                }

                type = type.ToLower();

                if (type == "weapons")
                {
                    SLogger.Instance.Warn($"[Swords] = {slotTypes[1]}");
                    SLogger.Instance.Warn($"[Staffs] = {slotTypes[17]}");
                    SLogger.Instance.Warn($"[Katanas] = {slotTypes[24]}");
                    SLogger.Instance.Warn($"[Wands] = {slotTypes[8]}");
                    SLogger.Instance.Warn($"[Bows] = {slotTypes[3]}");
                    SLogger.Instance.Warn($"[Daggers] = {slotTypes[2]}");
                    SLogger.Instance.Warn($"[Total Items] = {totalItems}");
                    SLogger.Instance.Warn("===========================");
                }

                if (type == "armors")
                {
                    SLogger.Instance.Warn($"[Light Armor] = {slotTypes[6]}");
                    SLogger.Instance.Warn($"[Heavy Armor] = {slotTypes[7]}");
                    SLogger.Instance.Warn($"[Robe] = {slotTypes[14]}");
                    SLogger.Instance.Warn($"[Total Items] = {totalItems}");
                    SLogger.Instance.Warn("===========================");
                }

                if (type == "abilities")
                {
                    SLogger.Instance.Warn($"[Tome] = {slotTypes[4]}");
                    SLogger.Instance.Warn($"[Shield] = {slotTypes[5]}");
                    SLogger.Instance.Warn($"[Spell] = {slotTypes[11]}");
                    SLogger.Instance.Warn($"[Seal] = {slotTypes[12]}");
                    SLogger.Instance.Warn($"[Cloak] = {slotTypes[13]}");
                    SLogger.Instance.Warn($"[Quiver] = {slotTypes[15]}");
                    SLogger.Instance.Warn($"[Helm] = {slotTypes[16]}");
                    SLogger.Instance.Warn($"[Poison] = {slotTypes[18]}");
                    SLogger.Instance.Warn($"[Skull] = {slotTypes[19]}");
                    SLogger.Instance.Warn($"[Trap] = {slotTypes[20]}");
                    SLogger.Instance.Warn($"[Orb] = {slotTypes[21]}");
                    SLogger.Instance.Warn($"[Prism] = {slotTypes[22]}");
                    SLogger.Instance.Warn($"[Scepter] = {slotTypes[23]}");
                    SLogger.Instance.Warn($"[Shuriken] = {slotTypes[25]}");
                    SLogger.Instance.Warn($"[Total Items] = {totalItems}");
                    SLogger.Instance.Warn("===========================");
                }

                if (type == "rings")
                {
                    SLogger.Instance.Warn($"[Rings] = {slotTypes[9]}");
                    SLogger.Instance.Warn($"[Total Items] = {totalItems}");
                    SLogger.Instance.Warn("===========================");
                }

                return true;
            }
        }
    }
}
