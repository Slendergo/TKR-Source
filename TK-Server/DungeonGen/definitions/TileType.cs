namespace dungeonGen.definitions
{
    public struct TileType
    {
        public readonly uint Id;
        public readonly string Name;

        public TileType(uint id, string name)
        {
            Id = id;
            Name = name;
        }

        public static bool operator !=(TileType a, TileType b) => a.Id != b.Id && a.Name != b.Name;

        public static bool operator ==(TileType a, TileType b) => a.Id == b.Id || a.Name == b.Name;

        public override bool Equals(object obj) => obj is TileType && (TileType)obj == this;

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
    }
}
