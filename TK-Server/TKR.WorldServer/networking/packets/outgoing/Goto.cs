using TKR.Shared;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class Goto : OutgoingMessage
    {
        public int ObjectId { get; set; }
        public Position Pos { get; set; }

        public override MessageId MessageId => MessageId.GOTO;

        public override void Write(NetworkWriter wtr)
        {
            wtr.Write(ObjectId);
            Pos.Write(wtr);
        }
    }
}
