using TKR.Shared;
using System;
using System.Collections.Generic;
using TKR.WorldServer.core.miscfile.datas;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class NewTick : OutgoingMessage
    {
        public int TickId { get; set; }
        public int TickTime { get; set; }
        public List<ObjectStats> Statuses { get; set; }

        public override MessageId MessageId => MessageId.NEWTICK;

        public NewTick()
        {
            Statuses = new List<ObjectStats>();
        }

        public override void Write(NWriter wtr)
        {
            wtr.Write(TickId);
            wtr.Write(TickTime);
            wtr.Write((short)Statuses.Count);
            foreach (var status in Statuses)
                status.Write(wtr);
        }
    }
}
