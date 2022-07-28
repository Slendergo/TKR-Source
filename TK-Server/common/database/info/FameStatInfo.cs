using common.database.model;
using System;

namespace common.database.info
{
    public struct FameStatInfo
    {
        public string Description;
        public Func<FameStats, CharacterModel, int, bool> Condition;
        public Func<double, int> Bonus;
    }
}
