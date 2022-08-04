using wServer.networking.packets;

namespace wServer.networking
{
    public readonly struct InboundBuffer
    {
        public readonly Client Client;
        public readonly PacketId MessageId;
        public readonly byte[] Payload;

        public InboundBuffer(Client client, PacketId id, byte[] payload)
        {
            Client = client;
            MessageId = id;
            Payload = payload;
        }
    }
}
