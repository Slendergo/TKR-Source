using common.resources;
using System.Threading.Tasks;
using wServer.core.worlds.logic;
using wServer.utils;

namespace wServer.core.objects.vendors
{
    internal class ClosedVaultChest : SellableObject
    {
        public ClosedVaultChest(GameServer manager, ushort objType) : base(manager, objType)
        {
            Price = 200;
            Currency = CurrencyType.Fame;
            
        }

        public override void Buy(Player player)
        {
            var result = ValidateCustomer(player, null);

            if (result != BuyResult.Ok)
            {
                SendFailed(player, result);
                return;
            }

            var db = GameServer.Database;
            var acc = player.Client.Account;
            var trans = db.Conn.CreateTransaction();

            GameServer.Database.CreateChest(acc, trans);

            var t1 = db.UpdateCurrency(acc, -Price, Currency, trans);
            var t2 = trans.ExecuteAsync();

            Task.WhenAll(t1, t2).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    SendFailed(player, BuyResult.TransactionFailed);
                    return;
                }

                acc.Reload("vaultCount");
                player.CurrentFame = acc.Fame;
                (World as VaultWorld)?.AddChest(this);
                acc.Reload();
                player.Client.SendPacket(new networking.packets.outgoing.BuyResult()
                {
                    Result = 0,
                    ResultString = "Vault chest purchased!"
                });
            }).ContinueWith(e => SLogger.Instance.Error(e.Exception.InnerException.ToString()), TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
