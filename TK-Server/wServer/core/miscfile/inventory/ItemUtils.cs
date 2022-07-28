using common.resources;
using wServer.core;
using wServer.core.objects;
using wServer.core.objects.containers;

namespace wServer
{
    public static class ItemUtils
    {
        public static bool AuditItem(this IContainer container, Item item, int slot)
        {
            if (container is GiftChest && item != null || container is SpecialChest && item != null)
                return false;

            return item == null || container.SlotTypes[slot] == 0 || item.SlotType == container.SlotTypes[slot];
        }
    }
}
