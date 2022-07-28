using common;
using System.Collections.Generic;

namespace EyeOfTheMetis
{
    public struct AccountData
    {
        private readonly int accountId;
        private readonly DbAccount account;
        private readonly List<DbChar> characters;
        private readonly DbVault vault;

        private List<int> linkedAccountIds;

        public AccountData(DbAccount account, List<DbChar> characters, DbVault vault)
        {
            accountId = account.AccountId;

            this.account = account;
            this.characters = characters;
            this.vault = vault;

            linkedAccountIds = new List<int>();
        }

        public void AddToLinkedAccount(int accountId)
            => linkedAccountIds.Add(accountId);
    }
}