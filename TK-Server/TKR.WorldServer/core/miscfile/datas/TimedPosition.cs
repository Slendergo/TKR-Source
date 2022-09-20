using TKR.Shared;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.core.miscfile.datas
{
    public struct TimedPosition
    {
        public Position Position;
        public int Time;

        public static TimedPosition Read(NReader rdr) => new TimedPosition
        {
            Time = rdr.ReadInt32(),
            Position = Position.Read(rdr)
        };

        public override string ToString() => string.Format("{{Time: {0}, Position: {1}}}", Time, Position);
    }
}
