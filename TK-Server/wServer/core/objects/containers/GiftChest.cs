using common;

namespace wServer.core.objects
{
    internal class GiftChest : Container
    {
        public GiftChest(CoreServerManager manager, ushort objType, int? life, bool dying, RInventory dbLink = null) : base(manager, objType, life, dying, dbLink)
        { }

        public GiftChest(CoreServerManager manager, ushort id) : base(manager, id)
        { }
    }
}
