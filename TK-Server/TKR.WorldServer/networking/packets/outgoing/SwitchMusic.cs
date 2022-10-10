using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class SwitchMusic : OutgoingMessage
    {
        public string Music { get; set; }

        public override MessageId MessageId => MessageId.SWITCH_MUSIC;

        public override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Music);
        }
    }
}