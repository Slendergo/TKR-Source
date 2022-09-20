using System.Xml.Linq;
using TKR.Shared;
using TKR.WorldServer.core.objects.inventory;

namespace TKR.WorldServer.core.objects
{
    public interface IContainer
    {
        RInventory DbLink { get; }
        Inventory Inventory { get; }
        int[] SlotTypes { get; }
    }
}
