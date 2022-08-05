using common;

namespace wServer.core.objects
{
    internal class GiftChest : Container
    {
        public GiftChest(GameServer manager, ushort objType, int? life, bool dying, RInventory dbLink = null) : base(manager, objType, life, dying, dbLink)
        { }

        public GiftChest(GameServer manager, ushort id) : base(manager, id)
        { }
    }
}
