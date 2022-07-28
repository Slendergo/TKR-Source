using CA.Extensions.Concurrent;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class UpgradeBaseStatHandler : PacketHandlerBase<UpgradeBaseStat>
    {
        public override PacketId ID => PacketId.UPGRADESTAT;

        protected override void HandlePacket(Client client, UpgradeBaseStat packet) => Handle(client);

        private void Handle(Client client)
        {
            var player = client.Player;
            var acc = client.Account;

            if (player == null || IsTest(client))
                return;

            acc.Reload("fame");
            acc.Reload("totalFame");

            Client dummy;

            switch (acc.SetBaseStat)
            {
                case 0:
                    if (acc.Fame < 10000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 10000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 1:
                    if (acc.Fame < 20000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 20000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 2:
                    if (acc.Fame < 30000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 30000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 3:
                    if (acc.Fame < 40000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 40000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 4:
                    if (acc.Fame < 50000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 50000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 5:
                    if (acc.Fame < 60000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 60000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 6:
                    if (acc.Fame < 70000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 70000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 7:
                    if (acc.Fame < 80000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 80000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 8:
                    if (acc.Fame < 90000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 90000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 9:
                    if (acc.Fame < 100000)
                    {
                        player.SendError("Not enough Fame!");
                        return;
                    }
                    acc.Fame -= 100000;
                    acc.SetBaseStat += 1;
                    player.SendInfo("Your Stat Base was upgraded to +1! \n Restart your Character to update it!");
                    player.Stats.ReCalculateValues();
                    player.Stats.Base.ReCalculateValues();
                    player.Stats.Boost.ReCalculateValues();
                    acc.FlushAsync();
                    dummy = player.CoreServerManager.ConnectionManager.Clients
                        .KeyWhereAsParallel(_ => _.Account.Name.Equals(player.Name))
                        .SingleOrDefault();
                    dummy?.Disconnect("Magician Upgrade");
                    break;

                case 10:
                    player.SendError("You have maxed your Base Stat!");
                    break;
            }
        }
    }
}
