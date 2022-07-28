using RotMG.Common;
using System.Collections.Generic;

namespace dungeonGen.definitions
{
    public class DungeonObject
    {
        public KeyValuePair<string, string>[] Attributes = Empty<KeyValuePair<string, string>>.Array;
        public ObjectType ObjectType;
    }
}
