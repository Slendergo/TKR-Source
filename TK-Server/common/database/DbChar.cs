using System;

namespace common.database
{
    public class DbChar : RedisObject
    {
        public DbChar(DbAccount acc, int charId, bool isAsync = false)
        {
            Account = acc;
            CharId = charId;

            Init(acc.Database, "char." + acc.AccountId + "." + charId, null, isAsync);
        }

        public DbAccount Account { get; private set; }

        public int CharId { get; private set; }
        public DateTime CreateTime { get => GetValue<DateTime>("createTime"); set => SetValue("createTime", value); }
        public bool Dead { get => GetValue<bool>("dead"); set => SetValue("dead", value); }
        public int Experience { get => GetValue<int>("exp"); set => SetValue("exp", value); }
        public int Fame { get => GetValue<int>("fame"); set => SetValue("fame", value); }
        public byte[] FameStats { get => GetValue<byte[]>("fameStats"); set => SetValue("fameStats", value); }
        public int FinalFame { get => GetValue<int>("finalFame"); set => SetValue("finalFame", value); }
        public bool HasBackpack { get => GetValue<bool>("hasBackpack"); set => SetValue("hasBackpack", value); }
        public int HealthStackCount { get => GetValue<int>("hpPotCount"); set => SetValue("hpPotCount", value); }
        public int HP { get => GetValue<int>("hp"); set => SetValue("hp", value); }
        public ushort[] Items { get => GetValue<ushort[]>("items"); set => SetValue("items", value); }
        public ItemData[] Datas { get => GetValue<ItemData[]>("datas"); set => SetValue("datas", value); }
        public DateTime LastSeen { get => GetValue<DateTime>("lastSeen"); set => SetValue("lastSeen", value); }
        public int LDBoostTime { get => GetValue<int>("ldBoost"); set => SetValue("ldBoost", value); }
        public int Level { get => GetValue<int>("level"); set => SetValue("level", value); }
        public int MagicStackCount { get => GetValue<int>("mpPotCount"); set => SetValue("mpPotCount", value); }
        public bool MaxedAtt { get => GetValue<bool>("maxedAtt"); set => SetValue("maxedAtt", value); }
        public bool MaxedDef { get => GetValue<bool>("maxedDef"); set => SetValue("maxedDef", value); }
        public bool MaxedDex { get => GetValue<bool>("maxedDex"); set => SetValue("maxedDex", value); }
        public bool MaxedLife { get => GetValue<bool>("maxedLife"); set => SetValue("maxedLife", value); }
        public bool MaxedMana { get => GetValue<bool>("maxedMana"); set => SetValue("maxedMana", value); }
        public bool MaxedSpd { get => GetValue<bool>("maxedSpd"); set => SetValue("maxedSpd", value); }
        public bool MaxedVit { get => GetValue<bool>("maxedVit"); set => SetValue("maxedVit", value); }
        public bool MaxedWis { get => GetValue<bool>("maxedWis"); set => SetValue("maxedWis", value); }
        public bool SuperMaxedAtt { get => GetValue<bool>("superMaxedAtt"); set => SetValue("superMaxedAtt", value); }
        public bool SuperMaxedDef { get => GetValue<bool>("superMaxedDef"); set => SetValue("superMaxedDef", value); }
        public bool SuperMaxedDex { get => GetValue<bool>("superMaxedDex"); set => SetValue("superMaxedDex", value); }
        public bool SuperMaxedLife { get => GetValue<bool>("superMaxedLife"); set => SetValue("superMaxedLife", value); }
        public bool SuperMaxedMana { get => GetValue<bool>("superMaxedMana"); set => SetValue("superMaxedMana", value); }
        public bool SuperMaxedSpd { get => GetValue<bool>("superMaxedSpd"); set => SetValue("superMaxedSpd", value); }
        public bool SuperMaxedVit { get => GetValue<bool>("superMaxedVit"); set => SetValue("superMaxedVit", value); }
        public bool SuperMaxedWis { get => GetValue<bool>("superMaxedWis"); set => SetValue("superMaxedWis", value); }
        public int MP { get => GetValue<int>("mp"); set => SetValue("mp", value); }
        public ushort ObjectType { get => GetValue<ushort>("charType"); set => SetValue("charType", value); }
        public int PetId { get => GetValue<int>("petId"); set => SetValue("petId", value); }
        public int Points { get => GetValue<int>("points"); set => SetValue("points", value); }
        public int Skin { get => GetValue<int>("skin"); set => SetValue("skin", value); }

        public int Node1TickMaj { get => GetValue<int>("node1TickMaj"); set => SetValue("node1TickMaj", value); }
        public int Node1TickMin { get => GetValue<int>("node1TickMin"); set => SetValue("node1TickMin", value); }
        public int Node1Med { get => GetValue<int>("node1Med"); set => SetValue("node1Med", value); }
        public int Node1Big { get => GetValue<int>("node1Big"); set => SetValue("node1Big", value); }

        public int Node2TickMaj { get => GetValue<int>("node2TickMaj"); set => SetValue("node2TickMaj", value); }
        public int Node2TickMin { get => GetValue<int>("node2TickMin"); set => SetValue("node2TickMin", value); }
        public int Node2Med { get => GetValue<int>("node2Med"); set => SetValue("node2Med", value); }
        public int Node2Big { get => GetValue<int>("node2Big"); set => SetValue("node2Big", value); }

        public int Node3TickMaj { get => GetValue<int>("node3TickMaj"); set => SetValue("node3TickMaj", value); }
        public int Node3TickMin { get => GetValue<int>("node3TickMin"); set => SetValue("node3TickMin", value); }
        public int Node3Med { get => GetValue<int>("node3Med"); set => SetValue("node3Med", value); }
        public int Node3Big { get => GetValue<int>("node3Big"); set => SetValue("node3Big", value); }

        public int Node4TickMaj { get => GetValue<int>("node4TickMaj"); set => SetValue("node4TickMaj", value); }
        public int Node4TickMin { get => GetValue<int>("node4TickMin"); set => SetValue("node4TickMin", value); }
        public int Node4Med { get => GetValue<int>("node4Med"); set => SetValue("node4Med", value); }
        public int Node4Big { get => GetValue<int>("node4Big"); set => SetValue("node4Big", value); }

        public int Node5TickMaj { get => GetValue<int>("node5TickMaj"); set => SetValue("node5TickMaj", value); }
        public int Node5TickMin { get => GetValue<int>("node5TickMin"); set => SetValue("node5TickMin", value); }
        public int Node5Med { get => GetValue<int>("node5Med"); set => SetValue("node5Med", value); }
        public int Node5Big { get => GetValue<int>("node5Big"); set => SetValue("node5Big", value); }

        public int[] Stats { get => GetValue<int[]>("stats"); set => SetValue("stats", value); }
        public int Tex1 { get => GetValue<int>("tex1"); set => SetValue("tex1", value); }
        public int Tex2 { get => GetValue<int>("tex2"); set => SetValue("tex2", value); }
        public bool UpgradeEnabled { get => GetValue<bool>("upgradeEnabled"); set => SetValue("upgradeEnabled", value); }
        public int XPBoostTime { get => GetValue<int>("xpBoost"); set => SetValue("xpBoost", value); }
        public int Essence { get => GetValue<int>("essence"); set => SetValue("essence", value); }
        public int EssenceCap { get => GetValue<int>("essenceCap"); set => SetValue("essenceCap", value); }
    }
}
