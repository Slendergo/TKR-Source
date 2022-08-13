﻿using common;

namespace wServer.networking.packets.outgoing
{
    internal class InvitedToGuild : OutgoingMessage
    {
        public string Name;
        public string GuildName;

        public override MessageId MessageId => MessageId.INVITEDTOGUILD;

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.WriteUTF(GuildName);
        }
    }
}
