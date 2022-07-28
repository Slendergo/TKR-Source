using common.resources;
using System.Threading.Tasks;
using wServer.core.worlds.logic;
using wServer.utils;

namespace wServer.core.objects.vendors
{
    internal class ClosedVaultChest : SellableObject
    {
        public ClosedVaultChest(CoreServerManager manager, ushort objType) : base(manager, objType)
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

            var db = CoreServerManager.Database;
            var acc = player.Client.Account;
            var trans = db.Conn.CreateTransaction();

            CoreServerManager.Database.CreateChest(acc, trans);
            switch (player.Rank)
            {
                case 10:
                    Price = 150;
                    break;
                case 20:
                    Price = 125;
                    break;
                case 30:
                    Price = 100;
                    break;
                case 40:
                    Price = 75;
                    break;
                case 50:
                    Price = 25;
                    break;
                case 60:
                    Price = 0;
                    break;
                default:
                    Price = 200;
                    break;
            }
            /* if (player.Rank == 10)
             {
                 Price -= 50;
             }
             if (player.Rank == 20)
             {
                 Price -= 75;
             }
             if (player.Rank == 30)
             {
                 Price -= 100;
             }
             if (player.Rank == 40)
             {
                 Price -= 125;
             }
             if (player.Rank == 50)
             {
                 Price -= 175;
             }
             if (player.Rank == 60)
             {
                 Price -= 200;
             }*/

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
                (Owner as Vault)?.AddChest(this);
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
