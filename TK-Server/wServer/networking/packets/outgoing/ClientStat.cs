﻿using common;

namespace wServer.networking.packets.outgoing
{
    public class ClientStat : OutgoingMessage
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override MessageId MessageId => MessageId.CLIENTSTAT;

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Value);
        }
    }
}
