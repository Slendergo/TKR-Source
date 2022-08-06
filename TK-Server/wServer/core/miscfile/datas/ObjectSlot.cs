using common;

namespace wServer
{
    public struct ObjectSlot
    {
        public int ObjectId;
        public int ObjectType;
        public byte SlotId;

        public static ObjectSlot Read(NReader rdr) => new ObjectSlot
        {
            ObjectId = rdr.ReadInt32(),
            SlotId = rdr.ReadByte(),
            ObjectType = rdr.ReadInt16()
        };

        public override string ToString() => string.Format("{{ObjectId: {0}, SlotId: {1}, ObjectType: {2}}}", ObjectId, SlotId, ObjectType);

        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.Write(SlotId);
            wtr.Write((short)ObjectType);
        }
    }
}
