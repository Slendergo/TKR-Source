using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class UpdateAckHandler : PacketHandlerBase<UpdateAck>
    {
        public override PacketId ID => PacketId.UPDATEACK;

        protected override void HandlePacket(Client client, UpdateAck packet)
        {
            //client.AwaitingUpdate = false;
        }
    }
}
