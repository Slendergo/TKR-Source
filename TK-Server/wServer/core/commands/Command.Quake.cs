using common.resources;
using System;
using System.Linq;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Quake : Command
        {
            public Quake() : base("quake", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string worldName)
            {
                var worldProtoData = player.CoreServerManager.Resources.Worlds.Data;

                if (string.IsNullOrWhiteSpace(worldName))
                {
                    var msg = worldProtoData.Aggregate(
                        "Valid World Names: ", (c, p) => c + ((!p.Value.setpiece) ? (p.Key + ", ") : ""));
                    player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                    return false;
                }

                if (player.Owner is Nexus)
                {
                    player.SendError("Cannot use /quake in Nexus.");
                    return false;
                }

                var worldNameProper =
                    player.CoreServerManager.Resources.Worlds.Data.FirstOrDefault(
                        p => p.Key.Equals(worldName, StringComparison.InvariantCultureIgnoreCase)).Key;

                ProtoWorld proto;
                if (worldNameProper == null || (proto = worldProtoData[worldNameProper]).setpiece)
                {
                    player.SendError("Invalid world.");
                    return false;
                }

                World world;
                if (proto.persist)
                    world = player.CoreServerManager.WorldManager.Worlds[proto.id];
                else
                {
                    DynamicWorld.TryGetWorld(proto, player.Client, out world);
                    world = player.CoreServerManager.WorldManager.AddWorld(world ?? new World(proto));
                }

                player.Owner.QuakeToWorld(world);
                return true;
            }
        }
    }
}
