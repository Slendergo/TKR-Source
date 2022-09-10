using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.packets.outgoing;
using wServer.utils;

namespace wServer.core.objects.player.state
{
    public sealed class MoveRecord
    {
        public int Time;
        public float X;
        public float Y;

        public MoveRecord(int time, float x, float y)
        {
            Time = time;
            X = x;
            Y = y;
        }
    }
    //class FixedQueue<T>
    //{
    //    public T[] values;
    //    private int size = 0;
    //    private int from = 0;
    //    private int to = 0;

    //    public FixedQueue<T>(int size) 
    //    {
    //        values = new T[size];
    //        size = size;
    //    }

    //    public function add(elem: T)
    //    {
    //        this.to++;
    //        if (this.to == this.size)
    //            this.to = 0;

    //        this.values[this.to] = elem;
    //    }

    //    public T Pop(int idx)
    //{
    //        var elem = values[from];
    //        from++;
    //        if (from == size)
    //            from = 0;
    //        return elem;
    //    }

    //    public void Clear()
    //    {
    //        this.from = this.to = 0;
    //    }
    //}

    public sealed class ClientState
    {
        private readonly Player Player;

        private ConcurrentQueue<int> PendingMoves = new ConcurrentQueue<int>();
        
        private int MoveRecordsWindow = 0;
        private MoveRecord[] MoveRecords = new MoveRecord[110];
        private List<AoeData> Aoes = new List<AoeData>();

        public ClientState(Player player)
        {
            Player = player;
        }

        public void Update(ref TickTime time)
        {
            //check the sent aoes have all been received
            Player.PlayerUpdate.SendUpdate();
            Player.PlayerUpdate.SendNewTick(time.ElapsedMsDelta, Aoes);
            
            // no longer need
            Aoes.Clear();
        }

        public void OnUpdateAck()
        {
            // update adds a list of pending changes
            // this includes tiles,
            // add
            // remove

            // upon updateack received we push them to active client state
        }

        public void OnShootAck(int time)
        {
            //upon shoreceived we get the first send pending shoot
            //if the time is -1 it means client entity doesnt exist or is dead
            //so it would need to check the owner of projectile is dead or not
            //if it is dc cus its not a valid state (modified)
            //otherwise valid projectile and we add that to active projectile state
        }

        private Queue<AoeData> PendingAoe = new Queue<AoeData>();

        private int SentAoes;
        private int ReceivedAoes;

        public void AwaitAoe(AoeData aoe)
        {
            SentAoes++;
            PendingAoe.Enqueue(aoe);
            Aoes.Add(aoe);
        }

        public void OnAoeAck(int time, float x, float y)
        {
            if(x == 0 || y == 0)
            {
                Console.WriteLine("AoeAck: 0, 0");
            }

            if (PendingAoe.Count == 0)
            {
                Disconnect("Received AoeAck without sending Aoe");
                return;
            }

            ReceivedAoes++;

            var aoe = PendingAoe.Dequeue();

            MoveRecord record = null;
            var minDelta = int.MaxValue;
            var start = (MoveRecordsWindow + 1) * 11 - 1;
            var i = 0;
            var flip = false;
            while (minDelta >= 50)
            {
                var currentRecord = MoveRecords[start + i];
                if (currentRecord != null)
                {
                    var delta = time - currentRecord.Time;
                    if (delta < minDelta)
                    {
                        record = currentRecord;
                        minDelta = delta;
                    }
                }

                i--;
                if (flip && i <= 0)
                    break;
                if (start + i < 0)
                {
                    i = MoveRecords.Length - start - 1;
                    flip = true;
                }
            }

            Position pos;
            if (minDelta == 0)
                pos = new Position(x, y);
            else
                pos = new Position(record.X, record.Y);

            var maxDistance = Player.Stats.GetSpeed() * (minDelta / 1000.0f);
            var dist = pos.DistTo(x, y);

            if (dist > 2.0 + maxDistance)
            {
                Console.WriteLine($"Distance: {dist} > {2.0 + maxDistance} [{maxDistance}]");
                Disconnect("Record too far from the aoeack x, y");
                return;
            }

            var hit = pos.DistTo(aoe.Pos.X, aoe.Pos.Y) < aoe.Radius;
            if (hit)
            {
                if (Player.HasConditionEffect(ConditionEffectIndex.Invulnerable) || Player.HasConditionEffect(ConditionEffectIndex.Invincible) || Player.HasConditionEffect(ConditionEffectIndex.Hidden))
                    return;

                Player.ApplyConditionEffect(aoe.Effect, (int)(aoe.Duration * 1000.0));
                
                var dmg = (int)Player.Stats.GetDefenseDamage(aoe.Damage, false);

                Player.HP -= dmg;

                var damage = new Damage()
                {
                    TargetId = Player.Id,
                    Effects = aoe.Effect,
                    DamageAmount = dmg,
                    Kill = Player.HP <= dmg,
                };
                Player.World.BroadcastIfVisibleExclude(damage, Player, Player);

                if (Player.HP <= 0)
                {
                    _ = Player.World.GameServer.Resources.GameData.ObjectDescs.TryGetValue(aoe.OrigType, out var desc);
                    Player.Death(desc?.ObjectId ?? "Unknown");
                }
            }
        }

        public void AwaitMove(int tickId) => PendingMoves.Enqueue(tickId);

        public void OnMove(int tickId, int time, float newX, float newY, MoveRecord[] moveRecords)
        {
            if (!PendingMoves.TryDequeue(out var serverTickId))
            {
                Disconnect("One too many MovePackets");
                return;
            }

            if (serverTickId != tickId)
            {
                Disconnect("[NewTick -> Move] TickIds don't match");
                return;
            }

            if (tickId > Player.PlayerUpdate.TickId)
            {
                Disconnect("[NewTick -> Move] Invalid tickId");
                return;
            }

            if (SentAoes > ReceivedAoes)
            {
                Disconnect("Didnt receive correct count of Aoe Acks");
                return;
            }

            Player.LastClientTime = time;

            for (var i = 0; i < moveRecords.Length; i++)
                MoveRecords[MoveRecordsWindow * 11 + i] = moveRecords[i];
            MoveRecords[MoveRecordsWindow * 11 + 10] = new MoveRecord(time, newX, newY);
            MoveRecordsWindow++;
            if (MoveRecordsWindow >= 10)
                MoveRecordsWindow = 0;

            if (!Player.World.Map.Contains(newX, newY))
            {
                Player.Client.Disconnect("Out of map bounds");
                return;
            }

            if (!Player.World.IsPassable(newX, newY))
            {
                StaticLogger.Instance.Info($"{Player.Name} no-clipped. {newX}, {newY}");
                Player.Client.Disconnect("No clipping");
                return;
            }

            for (var i = 0; i < moveRecords.Length; i++)
            {
                var record = moveRecords[i];
                if (!Player.World.IsPassable(record.X, record.Y))
                {
                    StaticLogger.Instance.Info($"{Player.Name} move record no-clipped. {newX}, {newY}");
                    Player.Client.Disconnect("No clipping");
                    break;
                }
            }

            if (newX != -1 && newX != Player.X || newY != -1 && newY != Player.Y)
            {
                if (Player.Stars <= 2 && Player.Quest != null && Player.DistTo(newX, newY) > 50 && Player.Quest.DistTo(newX, newY) < 0.25)
                {
                    StaticLogger.Instance.Warn($"{Player.Name} was caught teleporting directly to a quest, uh oh");
                    Player.Client.Disconnect("Unexpected Error Occured");
                    return;
                }

                var dt = time - Player.LastClientTime;

                var dx = Math.Abs(newX - Player.X);
                var dy = Math.Abs(newY - Player.Y);

                var multiplier = Player.World.GameServer.Resources.GameData.Tiles[Player.World.Map[(int)newX, (int)newY].TileId].Speed;

                var serverDistance = Player.Stats.MoveSpeed(multiplier) * dt;
                var clientDistance = Math.Sqrt(Math.Pow(dx, 2)) + Math.Sqrt(Math.Pow(dy, 2));

                var absDis = Math.Abs(serverDistance - clientDistance);
                if (absDis > 3.0)
                {
                    Player.SpeedCountTollerance++;
                    if (Player.SpeedCountTollerance > 4)
                    {
                        foreach (var w in Player.GameServer.WorldManager.GetWorlds())
                            w.ForeachPlayer(other =>
                            {
                                if (other.IsAdmin || other.IsCommunityManager)
                                    other.SendInfo($"Warning: [{Player.Name}] {Player.AccountId}-{Player.Client.Character.CharId} is faster than expected!");
                            });
                        StaticLogger.Instance.Info($"[{Player.Name}] {Player.AccountId}-{Player.Client.Character.CharId} is moving faster than expected!");
                        Player.Client.Disconnect("Speed Delta Too Fast");
                    }
                }
                else
                {
                    Player.SpeedCountTollerance--;
                    if (Player.SpeedCountTollerance < 0)
                        Player.SpeedCountTollerance = 0;
                }

                Player.Move(newX, newY);
                Player.PlayerUpdate.UpdateTiles = true;
            }
        }

        private void Disconnect(string reason) => Player.Client.Disconnect(reason);
    }
}
