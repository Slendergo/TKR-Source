using common.database.model;
using System.Linq;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct Vault
    {
        private ushort[][] chests;

        public ushort[] this[int index] => chests[index];

        public static Vault Serialize(AccountModel account, VaultModel vault) => new Vault()
        {
            chests = Enumerable.Range(0, account.VaultAmount - 1).Select(x => vault[x] ?? Enumerable.Repeat((ushort)0xffff, 8).ToArray()).ToArray()
        };

        public XElement ToXml() => new XElement("Vault",
            chests.Select(x => new XElement("Chest", x.Select(i => (short)i).Take(8).ToArray().ToCommaSepString()))
        );
    }
}
