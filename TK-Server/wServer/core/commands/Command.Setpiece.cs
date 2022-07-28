using System;
using System.Linq;
using wServer.core.objects;
using wServer.core.setpieces;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Setpiece : Command
        {
            public Setpiece() : base("setpiece", permLevel: 100)
            { }

            protected override bool Process(Player player, TickData time, string setPiece)
            {
                if ((player.Owner is Nexus) || (player.Owner is Marketplace) || player.Owner.Id == -10 || player.Owner is GuildHall)
                {
                    player.SendError("No");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(setPiece))
                {
                    var type = typeof(ISetPiece);
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);
                    var msg = types.Aggregate(
                        "Valid SetPieces: ", (c, p) => c + p.Name + ", ");
                    player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                    return false;
                }

                if (!player.Owner.Name.Equals("Nexus"))
                {
                    try
                    {
                        ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType("wServer.core.setpieces." + setPiece, true, true));
                        piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
                        return true;
                    }
                    catch (Exception)
                    {
                        player.SendError("Invalid SetPiece.");
                        return false;
                    }
                }
                else
                {
                    player.SendInfo("/setpiece not allowed in Nexus.");
                    return false;
                }
            }
        }
    }
}
