namespace dungeonGen.definitions
{
    public struct ObjectType
    {
        public readonly uint Id;
        public readonly string Name;

        public ObjectType(uint id, string name)
        {
            Id = id;
            Name = name;
        }

        public static bool operator !=(ObjectType a, ObjectType b) => a.Id != b.Id && a.Name != b.Name;

        public static bool operator ==(ObjectType a, ObjectType b) => a.Id == b.Id || a.Name == b.Name;

        public override bool Equals(object obj) => obj is ObjectType && (ObjectType)obj == this;

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
    }
}
