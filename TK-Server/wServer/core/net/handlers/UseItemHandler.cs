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
            var Time = rdr.ReadInt32();
            var SlotObject = ObjectSlot.Read(rdr);
            var ItemUsePos = Position.Read(rdr);
            var UseType = rdr.ReadByte();
            var SellMaxed = rdr.ReadByte();

            var player = client.Player;
            if (player?.World == null)
                return;

            player.UseItem(tickTime, SlotObject.ObjectId, SlotObject.SlotId, ItemUsePos, SellMaxed);
        }
    }
}
