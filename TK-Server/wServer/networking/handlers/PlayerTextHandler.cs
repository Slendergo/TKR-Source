using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    public class PlayerMessage
    {
        public Player Player { get; private set; }
        public string Message { get; private set; }
        public long Time { get; private set; }

        public PlayerMessage(Player player, TickData time, string msg)
        {
            Player = player;
            Message = msg;
            Time = time.TotalElapsedMs;
        }
    }

    internal class PlayerTextHandler : PacketHandlerBase<PlayerText>
    {
        public override PacketId ID => PacketId.PLAYERTEXT;

        protected override void HandlePacket(Client client, PlayerText packet)
        {
            if (client?.Player?.Owner == null || packet.Text.Length > 512)
                return;
            client.Player.AddPendingAction(t => Handle(client.Player, t, packet.Text));
        }

        private void Handle(Player player, TickData time, string text)
        {
            if (player?.Owner == null || text.Length > 512)
                return;

            var manager = player.CoreServerManager;

            // check for commands before other checks
            if (text[0] == '/')
            {
                manager.CommandManager.Execute(player, time, text);
            }
            else
            {
                if (!player.NameChosen)
                {
                    player.SendError("Please choose a name before chatting.");
                    return;
                }

                if (player.Muted)
                {
                    player.SendError("Muted. You can not talk at this time.");
                    return;
                }

                if (player.CompareAndCheckSpam(text, time.TotalElapsedMs))
                {
                    return;
                }
                if (player.Stars < 2 && player.Rank < 10)
                {
                    player.SendHelp("To use this feature you need 2 stars or D-1 rank.");
                    return;
                }

                // save message for mob behaviors
                player.Owner.ChatReceived(player, text);

                manager.ChatManager.Say(player, text);
            }
        }
    }
}
