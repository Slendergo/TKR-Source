using TKR.Shared;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class UseItemHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.USEITEM;

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

            player.UseItem(time, tickTime, slotObject.ObjectId, slotObject.SlotId, itemUsePos, sellMaxed, useType);
        }
    }
}
