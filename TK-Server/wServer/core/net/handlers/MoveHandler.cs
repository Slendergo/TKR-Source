using common;
using System;
using wServer.core;
using wServer.networking;
using wServer.utils;

namespace wServer.core.net.handlers
{
    public class MoveHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.MOVE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var tickId = rdr.ReadInt32();
            var time = rdr.ReadInt32();
            var newX = rdr.ReadSingle();
            var newY = rdr.ReadSingle();
            var moveRecords = new TimedPosition[rdr.ReadInt16()];
            for (var i = 0; i < moveRecords.Length; i++)
                moveRecords[i] = TimedPosition.Read(rdr);


            var player = client.Player;

            if (newX != -1 && newX != player.X || newY != -1 && newY != player.Y)
            {
                if (!player.World.Map.Contains(newX, newY))
                {
                    player.Client.Disconnect("Out of map bounds");
                    return;
                }

                if (player.Stars <= 2 && player.Quest != null && player.Dist(new Position(newX, newY)) > 50 && player.Quest.Dist(new Position(newX, newY)) < 0.25)
                {
                    StaticLogger.Instance.Warn($"{player.Name} was caught teleporting directly to a quest, uh oh");
                    player.Client.Disconnect("Unexpected Error Occured");
                    return;
                }

                var dt = time - player.LastClientTime;

                var dx = Math.Abs(newX - player.X);
                var dy = Math.Abs(newY - player.Y);

                var multiplier = player.World.GameServer.Resources.GameData.Tiles[player.World.Map[(int)newX, (int)newY].TileId].Speed;
                
                var serverDistance = player.Stats.MoveSpeed(multiplier) * dt;
                var clientDistance = Math.Sqrt(Math.Pow(dx, 2)) + Math.Sqrt(Math.Pow(dy, 2));

                var absDis = Math.Abs(serverDistance - clientDistance);
                if(absDis > 3.0)
                {
                    player.SpeedCountTollerance++;
                    if (player.SpeedCountTollerance > 4)
                    {
                        foreach (var w in player.GameServer.WorldManager.GetWorlds())
                            w.ForeachPlayer(other =>
                            {
                                if (other.IsAdmin || other.IsCommunityManager)
                                    other.SendInfo($"Warning: [{player.Name}] {player.AccountId}-{player.Client.Character.CharId} is faster than expected!");
                            });
                        StaticLogger.Instance.Info($"[{player.Name}] {player.AccountId}-{player.Client.Character.CharId} is moving faster than expected!");
                        player.Client.Disconnect("Speed Delta Too Fast");
                    }
                }
                else
                {
                    player.SpeedCountTollerance--;
                    if (player.SpeedCountTollerance < 0)
                        player.SpeedCountTollerance = 0;
                }

                ////// s = d / t

                ////// calculate the distance

                ////// d = s * t;

                ////// time between this move and last move
                //var dt = time - player.LastClientTime;

                //// time in seconds
                //var moveTime = dt / 1000.0;

                //// distance
                //var distance = player.Dist(newX, newY);

                //var multiplier = player.World.GameServer.Resources.GameData.Tiles[player.World.Map[(int)newX, (int)newY].TileId].Speed;

                //// speed
                //var clientSpeed = distance / moveTime;
                //var serverSpeed = player.Stats.MoveSpeed(multiplier) * dt; // player.Stats.GetSpeed() * multiplier;

                //// only check if canTp
                //if (clientSpeed > serverSpeed)
                //{
                //    player.SpeedCountTollerance++;
                //    var delta = clientSpeed - serverSpeed;
                //    if (delta > 0.5) // tollerance
                //    {
                //        Console.WriteLine(clientSpeed + " " + dt + " " + distance + " " + clientSpeed + " " + serverSpeed);

                //        foreach (var other in player.World.Players.Values)
                //            if (other.IsAdmin || other.IsCommunityManager)
                //            {
                //                if (delta > 1.0)
                //                    other.SendInfo($"Warning: [{player.Name}] {player.AccountId}-{player.Client.Character.CharId} is moving exceptionally faster than expected! ({delta} | tolerance: 0.5)");
                //                else
                //                    other.SendInfo($"Warning: [{player.Name}] {player.AccountId}-{player.Client.Character.CharId} is faster than expected! ({delta} | tolerance: 0.5)");
                //            }
                //        StaticLogger.Instance.Info($"[{player.Name}] {player.AccountId}-{player.Client.Character.CharId} is moving faster than expected! {delta} ({delta} | tolerance: 0.5)");

                //        if (player.SpeedCountTollerance > 3)
                //            player.Client.Disconnect("Speed Delta Too Fast");
                //    }
                //    else
                //    {
                //        player.SpeedCountTollerance--;
                //        if(player.SpeedCountTollerance < 0)
                //            player.SpeedCountTollerance = 0;
                //    }
                //}

                player.Move(newX, newY);
                player.PlayerUpdate.UpdateTiles = true;
            }

            if (player.IsNoClipping())
            {
                if (player.NoClipCountTollerance > 2)
                {
                    StaticLogger.Instance.Info($"{player.Name} is walking on an occupied tile. {player.RealX}, {player.RealY} [REACHED MAX TOLERANCE]");
                    player.Client.Disconnect("No clipping");
                    return;
                }
            }
            else
                player.NoClipCountTollerance--;

            player.MoveReceived(tickTime, time, tickId);
        }
    }
}
