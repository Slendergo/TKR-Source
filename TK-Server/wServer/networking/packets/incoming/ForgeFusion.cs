using common;

namespace wServer.networking.packets.incoming
{
    public class ForgeFusion : IncomingMessage
    {
        public ForgeItem[] myInventory { get; set; }

        public override PacketId ID => PacketId.FORGEFUSION;

        public override Packet CreateInstance()
        {
            return new ForgeFusion();
        }

        protected override void Read(NReader rdr)
        {
            myInventory = new ForgeItem[rdr.ReadInt16()];
            for (int i = 0; i < myInventory.Length; i++)
            {
                myInventory[i].ObjectType = rdr.ReadUInt16();
                myInventory[i].slotID = rdr.ReadInt32();
                myInventory[i].Included = rdr.ReadBoolean();
            }
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)myInventory.Length);
            foreach (var i in myInventory)
            {
                wtr.Write(i.ObjectType);
                wtr.Write(i.slotID);
                wtr.Write(i.Included);
            }
        }
    }
}
