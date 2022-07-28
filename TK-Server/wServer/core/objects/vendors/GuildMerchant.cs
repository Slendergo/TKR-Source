using common.resources;
using System;

namespace wServer.core.objects.vendors
{
    internal class GuildMerchant : SellableObject
    {
        private readonly int _upgradeLevel;
        private int[] _hallLevels = new int[] { 1, 2, 3 };
        private int[] _hallPrices = new int[] { 10000, 100000, 250000 };
        private int[] _hallTypes = new int[] { 0x0736, 0x0737, 0x0738 };

        public GuildMerchant(CoreServerManager manager, ushort objType) : base(manager, objType)
        {
            Currency = CurrencyType.Fame;
            Price = Int32.MaxValue; // just in case for some reason _hallType isn't found

            for (int i = 0; i < _hallTypes.Length; i++)
            {
                if (objType != _hallTypes[i])
                    continue;

                Price = _hallPrices[i];

                _upgradeLevel = _hallLevels[i];
            }
        }

        public override void Buy(Player player)
        {
            var account = player.CoreServerManager.Database.GetAccount(player.AccountId);
            var guild = player.CoreServerManager.Database.GetGuild(account.GuildId);

            if (guild.IsNull || account.GuildRank < 30)
            {
                player.SendError("Verification failed.");
                return;
            }

            if (guild.Fame < Price)
            {
                player.Client.SendPacket(new networking.packets.outgoing.BuyResult
                {
                    ResultString = "Not enough Guild Fame!",
                    Result = 9
                });
                return;
            }

            // change guild level
            if (!player.CoreServerManager.Database.ChangeGuildLevel(guild, _upgradeLevel))
            {
                player.SendError("Internal server error.");
                return;
            }

            player.CoreServerManager.Database.UpdateGuildFame(guild, -Price);
            guild.GuildLootBoost = guild.GuildLootBoost + .15f;
            guild.FlushAsync();

            player.Client.SendPacket(new networking.packets.outgoing.BuyResult
            {
                ResultString = "Upgrade successful! Please leave the Guild Hall to have it upgraded.",
                Result = 0
            });
        }
    }
}
