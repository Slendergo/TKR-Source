using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class UseItemHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.USEITEM;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var slotObject = ObjectSlot.Read(rdr);
            var itemUsePos = Position.Read(rdr);
            var useType = rdr.ReadByte();
            var sellMaxed = rdr.ReadByte();

            var player = client.Player;
            if (player?.World == null)
                return;

            player.UseItem(tickTime, slotObject.ObjectId, slotObject.SlotId, itemUsePos, sellMaxed);
        }
    }
}
