using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class Death : OutgoingMessage
    {
        public int AccountId { get; set; }
        public int CharId { get; set; }
        public string KilledBy { get; set; }

        public override MessageId MessageId => MessageId.DEATH;

        public override void Write(NWriter wtr)
        {
            wtr.Write(AccountId);
            wtr.Write(CharId);
            wtr.WriteUTF(KilledBy);
        }
    }
}
