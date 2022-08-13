using common;

namespace wServer.networking.packets.outgoing
{
    public class Goto : OutgoingMessage
    {
        public int ObjectId { get; set; }
        public Position Pos { get; set; }

        public override MessageId MessageId => MessageId.GOTO;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
            Pos.Write(wtr);
        }
    }
}
