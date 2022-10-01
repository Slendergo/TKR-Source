using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class MapInfo : OutgoingMessage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Difficulty { get; set; }
        public uint Seed { get; set; }
        public int Background { get; set; }
        public bool AllowPlayerTeleport { get; set; }
        public bool ShowDisplays { get; set; }
        public string Music { get; set; }
        public bool DisableShooting { get; set; }
        public bool DisableAbilities { get; set; }


        public override MessageId MessageId => MessageId.MAPINFO;

        public override void Write(NWriter wtr)
        {
            wtr.Write(Width);
            wtr.Write(Height);
            wtr.WriteUTF(Name);
            wtr.WriteUTF(DisplayName);
            wtr.Write(Seed);
            wtr.Write(Background);
            wtr.Write(Difficulty);
            wtr.Write(AllowPlayerTeleport);
            wtr.Write(ShowDisplays);
            wtr.WriteUTF(Music);
            wtr.Write(DisableShooting);
            wtr.Write(DisableAbilities);
        }
    }
}
