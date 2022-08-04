using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class UseItemHandler : IMessageHandler
    {
        public int Time { get; set; }
        public ObjectSlot SlotObject { get; set; }
        public Position ItemUsePos { get; set; }
        public byte UseType { get; set; }
        public byte SellMaxed { get; set; }

        public override PacketId MessageId => PacketId.USEITEM;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            Time = rdr.ReadInt32();
            SlotObject = ObjectSlot.Read(rdr);
            ItemUsePos = Position.Read(rdr);
            UseType = rdr.ReadByte();
            SellMaxed = rdr.ReadByte();

            var player = client.Player;
            if (player?.World == null)
                return;

            player.UseItem(tickTime, SlotObject.ObjectId, SlotObject.SlotId, ItemUsePos, SellMaxed);
        }
    }
}
