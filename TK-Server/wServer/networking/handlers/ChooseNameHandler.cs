using common.database;
using NLog;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class ChooseNameHandler : PacketHandlerBase<ChooseName>
    {
        private static readonly Logger NameChangeLog = LogManager.GetCurrentClassLogger();

        public override PacketId ID => PacketId.CHOOSENAME;

        protected override void HandlePacket(Client client, ChooseName packet) => Handle(client, packet);

        private bool IsValid(string text)
        {
            var nonDup = new Regex(@"([a-zA-z]{2,})\1{1,}");
            var alpha = new Regex(@"^[A-Za-z]{1,10}$");

            return !(nonDup.Matches(text).Count > 0) && alpha.Matches(text).Count > 0;
        }

        private void Handle(Client client, ChooseName packet)
        {
            if (client.Player == null || IsTest(client))
                return;

            client.CoreServerManager.Database.ReloadAccount(client.Account);

            var name = packet.Name;

            if (name.Length < 1 || name.Length > 10 || !name.All(char.IsLetter) || !IsValid(name) || Database.GuestNames.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                client.SendPacket(new NameResult()
                {
                    Success = false,
                    ErrorText = "Invalid name"
                });
            else
            {
                string lockToken = null;

                var key = Database.NAME_LOCK;

                try
                {
                    while ((lockToken = client.CoreServerManager.Database.AcquireLock(key)) == null) ;

                    if (client.CoreServerManager.Database.Conn.HashExists("names", name.ToUpperInvariant()))
                    {
                        client.SendPacket(new NameResult()
                        {
                            Success = false,
                            ErrorText = "Duplicated name"
                        });
                        return;
                    }

                    if (client.Account.NameChosen && client.Account.Credits < 100)
                        client.SendPacket(new NameResult()
                        {
                            Success = false,
                            ErrorText = "Not enough gold"
                        });
                    else
                    {
                        // remove fame is purchasing name change
                        if (client.Account.NameChosen)
                            client.CoreServerManager.Database.UpdateCredit(client.Account, -100);

                        // rename
                        var oldName = client.Account.Name;
                        while (!client.CoreServerManager.Database.RenameIGN(client.Account, name, lockToken)) ;
                        NameChangeLog.Info($"{oldName} changed their name to {name}");

                        // update player
                        UpdatePlayer(client.Player);
                        client.SendPacket(new NameResult()
                        {
                            Success = true,
                            ErrorText = ""
                        });
                    }
                }
                finally
                {
                    if (lockToken != null)
                        client.CoreServerManager.Database.ReleaseLock(key, lockToken);
                }
            }
        }

        private void UpdatePlayer(Player player)
        {
            player.Credits = player.Client.Account.Credits;
            player.CurrentFame = player.Client.Account.Fame;
            player.Name = player.Client.Account.Name;
            player.NameChosen = true;
        }
    }
}
