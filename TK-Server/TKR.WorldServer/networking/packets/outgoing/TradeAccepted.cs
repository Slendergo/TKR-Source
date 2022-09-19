using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class TradeAccepted : OutgoingMessage
    {
        public bool[] MyOffer { get; set; }
        public bool[] YourOffer { get; set; }

        public override MessageId MessageId => MessageId.TRADEACCEPTED;


        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)MyOffer.Length);
            foreach (var i in MyOffer)
                wtr.Write(i);
            wtr.Write((short)YourOffer.Length);
            foreach (var i in YourOffer)
                wtr.Write(i);
        }
    }
}
