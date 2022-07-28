namespace common.database
{
    public class DbCharInv : RInventory
    {
        public DbCharInv(DbAccount acc, int charId, bool isAsync = false)
        {
            Field = "items";

            Init(acc.Database, "char." + acc.AccountId + "." + charId, Field, isAsync);
        }
    }
}
