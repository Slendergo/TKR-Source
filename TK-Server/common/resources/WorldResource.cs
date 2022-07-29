using System.Xml.Linq;

namespace common.resources
{
    public sealed class WorldResource
    {
        public readonly string IdName;
        public readonly string DisplayName;
        public readonly int Width;
        public readonly int Height;
        public readonly int Capacity;
        public readonly bool Instance;
        public readonly bool Persists;
        public readonly byte Difficulty;
        public readonly byte Background;
        public readonly byte VisibilityType;
        public readonly string MapJM;

        public WorldResource(XElement elem)
        {
            IdName = elem.GetAttribute<string>("id");
            DisplayName = elem.GetValue("DisplayName", IdName);
            Width = elem.GetValue<int>("Width");
            Height = elem.GetValue<int>("Height");
            Capacity = elem.GetValue<int>("Capacity");
            Instance = elem.GetValue<bool>("Instance");
            Persists = elem.GetValue<bool>("Persists");
            Difficulty = elem.GetValue<byte>("Difficulty");
            Background = elem.GetValue<byte>("Background");
            VisibilityType = elem.GetValue<byte>("VisibilityType");
            MapJM = elem.GetValue<string>("MapJM");
        }

        public override string ToString()
        {
            var ret = $"IdName: {IdName}\n";
            ret += $"Width: {Width}\n";
            ret += $"Height: {Height}\n";
            ret += $"Capacity: {Capacity}\n";
            ret += $"Instance: {Instance}\n";
            ret += $"Persists: {Persists}\n";
            ret += $"MapJM: {MapJM}";
            return ret;
        }
    }
}
