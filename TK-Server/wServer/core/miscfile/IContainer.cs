using common;

namespace wServer.core
{
    public interface IContainer
    {
        RInventory DbLink { get; }
        Inventory Inventory { get; }
        int[] SlotTypes { get; }
    }
}
