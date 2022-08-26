using common;
using common.discord;
using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.core.objects;
using wServer.core.setpieces;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core
{
    public enum KindgomState
    {
        Idle,
        Closing,
        Emptying,
        Closed,
        DoNothing
    }

    public sealed class KingdomManager
    {
        public int _EventCount = 0;
        public RealmWorld World;

        private static readonly Tuple<string, TauntData>[] CriticalEnemies = new Tuple<string, TauntData>[]
        {
            Tuple.Create("Strange Magician", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Strange Magician"
            }),
            Tuple.Create("Fire Elemental", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Fire Elemental"
            }),
            Tuple.Create("Kage Kami", new TauntData()
        {
            Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Kage Kami"
            }),
            Tuple.Create("Julius Caesar", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Julius Caesar"
            }),
            Tuple.Create("Fluffy", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Fluffy"
            }),
            Tuple.Create("Candy Gnome", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Candy Gnome"
            }),
            Tuple.Create("Wind Elemental", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Wind Elemental"
            }),
            Tuple.Create("Earth Elemental", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Earth Elemental"
            }),
            Tuple.Create("Water Elemental", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Water Elemental"
            }),
            Tuple.Create("Tiki Tiki", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Tiki Tiki"
            }),
            Tuple.Create("Lucky Ent God", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Lucky Ent God"
            }),
            Tuple.Create("Lucky Djinn", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Lucky Djinn"
            }),
            Tuple.Create("King Slime", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "King Slime"
            }),
            /* NOTE:
             * This encounter event is temporarily disabled until properly patch.
             *
             * The behavior of "Mushroom" is causing console spam, might be caused
             * by invalid paramenters on projectile cast on a specific phase.
             */
            Tuple.Create("Mushroom", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Mushroom"
            }),

            /*Tuple.Create("Spectral Sentry", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Spectral Sentry"
            }),*/

            Tuple.Create("Skull Shrine", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                NumberOfEnemies = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Skull Shrine"
            }),
            Tuple.Create("Cube God", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                NumberOfEnemies = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Cube God"
            }),
            Tuple.Create("Pentaract", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                NumberOfEnemies = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Pentaract"
            }),
            Tuple.Create("Grand Sphinx", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                NumberOfEnemies = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Grand Sphinx"
            }),
            Tuple.Create("Lord of the Lost Lands", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                NumberOfEnemies = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Lord of the Lost Lands"
            }),
            Tuple.Create("Hermit God", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                NumberOfEnemies = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Hermit God"
            }),
            Tuple.Create("Ghost Ship", new TauntData()
            {
                Spawn = new string[] {
                    ""
                },
                Final = new string[] {
                    ""
                },
                Killed = new string[] {
                    ""
                },
                NameOfDeath = "Ghost Ship"
            })
        };

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<TerrainType, Tuple<int, Tuple<string, double>[]>> RegionMobs = new Dictionary<TerrainType, Tuple<int, Tuple<string, double>[]>>()
        {
            { TerrainType.ShoreSand, Tuple.Create(
                100, new []
                {
                    Tuple.Create("Pirate", 0.3),
                    Tuple.Create("Piratess", 0.1),
                    Tuple.Create("Snake", 0.2),
                    Tuple.Create("Scorpion Queen", 0.4),
                })
            },
            { TerrainType.ShorePlains, Tuple.Create(
                150, new []
                {
                    Tuple.Create("Bandit Leader", 0.4),
                    Tuple.Create("Red Gelatinous Cube", 0.2),
                    Tuple.Create("Purple Gelatinous Cube", 0.2),
                    Tuple.Create("Green Gelatinous Cube", 0.2),
                })
            },
            { TerrainType.LowPlains, Tuple.Create(
                200, new []
                {
                    Tuple.Create("Hobbit Mage", 0.5),
                    Tuple.Create("Undead Hobbit Mage", 0.4),
                    Tuple.Create("Sumo Master", 0.1),
                })
            },
            { TerrainType.LowForest, Tuple.Create(
                200, new []
                {
                    Tuple.Create("Elf Wizard", 0.2),
                    Tuple.Create("Goblin Mage", 0.2),
                    Tuple.Create("Easily Enraged Bunny", 0.3),
                    Tuple.Create("Forest Nymph", 0.3),
                })
            },
            { TerrainType.LowSand, Tuple.Create(
                200, new []
                {
                    Tuple.Create("Sandsman King", 0.4),
                    Tuple.Create("Giant Crab", 0.2),
                    Tuple.Create("Sand Devil", 0.4),
                })
            },
            { TerrainType.MidPlains, Tuple.Create(
                150, new []
                {
                    Tuple.Create("Fire Sprite", 0.1),
                    Tuple.Create("Ice Sprite", 0.1),
                    Tuple.Create("Magic Sprite", 0.1),
                    Tuple.Create("Pink Blob", 0.07),
                    Tuple.Create("Gray Blob", 0.07),
                    Tuple.Create("Earth Golem", 0.04),
                    Tuple.Create("Paper Golem", 0.04),
                    Tuple.Create("Big Green Slime", 0.08),
                    Tuple.Create("Swarm", 0.05),
                    Tuple.Create("Wasp Queen", 0.2),
                    Tuple.Create("Shambling Sludge", 0.03),
                    Tuple.Create("Orc King", 0.06),
                    Tuple.Create("Candy Gnome", 0.02)
                })
            },
            { TerrainType.MidForest, Tuple.Create(
                150, new []
                {
                    Tuple.Create("Dwarf King", 0.3),
                    Tuple.Create("Metal Golem", 0.05),
                    Tuple.Create("Clockwork Golem", 0.05),
                    Tuple.Create("Werelion", 0.1),
                    Tuple.Create("Horned Drake", 0.3),
                    Tuple.Create("Red Spider", 0.1),
                    Tuple.Create("Black Bat", 0.1)
                })
            },
            { TerrainType.MidSand, Tuple.Create(
                300, new []
                {
                    Tuple.Create("Desert Werewolf", 0.25),
                    Tuple.Create("Fire Golem", 0.1),
                    Tuple.Create("Darkness Golem", 0.1),
                    Tuple.Create("Sand Phantom", 0.2),
                    Tuple.Create("Nomadic Shaman", 0.25),
                    Tuple.Create("Great Lizard", 0.1),
                })
            },
            { TerrainType.HighPlains, Tuple.Create(
                300, new []
                {
                    Tuple.Create("Shield Orc Key", 0.2),
                    Tuple.Create("Urgle", 0.2),
                    Tuple.Create("Undead Dwarf God", 0.6)
                })
            },
            { TerrainType.HighForest, Tuple.Create(
                300, new []
                {
                    Tuple.Create("Ogre King", 0.4),
                    Tuple.Create("Dragon Egg", 0.1),
                    Tuple.Create("Lizard God", 0.5),
                    Tuple.Create("Beer God", 0.1)
                })
            },
            { TerrainType.HighSand, Tuple.Create(
                250, new []
                {
                    Tuple.Create("Minotaur", 0.4),
                    Tuple.Create("Flayer God", 0.4),
                    Tuple.Create("Flamer King", 0.2)
                })
            },
            { TerrainType.Mountains, Tuple.Create(
                125, new []
                {
                    Tuple.Create("White Demon", 0.09),
                    Tuple.Create("Sprite God", 0.09),
                    Tuple.Create("Medusa", 0.09),
                    Tuple.Create("Ent God", 0.09),
                    Tuple.Create("Beholder", 0.09),
                    Tuple.Create("Flying Brain", 0.09),
                    Tuple.Create("Slime God", 0.09),
                    Tuple.Create("Ghost God", 0.09),
                    Tuple.Create("Rock Bot", 0.01),
                    Tuple.Create("Djinn", 0.09),
                    Tuple.Create("Leviathan", 0.09),
                    Tuple.Create("Arena Headless Horseman", 0.09)
                })
            },
        };

        private readonly List<Tuple<string, ISetPiece>> _events = new List<Tuple<string, ISetPiece>>()
        {
            Tuple.Create("Tiki Tiki", (ISetPiece) null), // null means use the entity name isntead of make a setpiece class
            Tuple.Create("King Slime", (ISetPiece) null),
            Tuple.Create("Fluffy", (ISetPiece) new Fluffy()),
            Tuple.Create("Candy Gnome", (ISetPiece) new CandyGnome()),
            Tuple.Create("Strange Magician", (ISetPiece) new StrangeMagician()),
            Tuple.Create("Water Elemental", (ISetPiece) new WaterElemental()),
            Tuple.Create("Earth Elemental", (ISetPiece) new EarthElemental()),
            Tuple.Create("Mushroom", (ISetPiece) new Mushroom()),
            Tuple.Create("Wind Elemental", (ISetPiece) new WindElemental()),
            Tuple.Create("Fire Elemental", (ISetPiece) new FireElemental()),
            Tuple.Create("Julius Caesar", (ISetPiece) new JuliusCaesar()),
            Tuple.Create("Cube God", (ISetPiece) null),
            Tuple.Create("Pentaract", (ISetPiece) new Pentaract()),
            Tuple.Create("Lord of the Lost Lands", (ISetPiece) null),
            Tuple.Create("Ghost Ship", (ISetPiece) new GhostShip()),
            Tuple.Create("Grand Sphinx", (ISetPiece) new Sphinx()),
            Tuple.Create("Hermit God", (ISetPiece) new Hermit()),
            Tuple.Create("Skull Shrine", (ISetPiece) new SkullShrine()),
            Tuple.Create("Lucky Ent God", (ISetPiece) null),
            Tuple.Create("Lucky Djinn", (ISetPiece) null),
        };

        private const float MAX_GUILD_LOOT_BOOST = 0.2f;

        private int[] EnemyCounts = new int[12];
        private int[] EnemyMaxCounts = new int[12];
        private long LastEnsurePopulationTime;
        private long LastAnnouncementTime;
        private long LastQuestTime;

        public KindgomState CurrentState;
        public bool DisableSpawning;

        public KingdomManager(RealmWorld world)
        {
            World = world;
            CurrentState = KindgomState.Idle;
        }

        public void Update(ref TickTime time)
        {
            switch (CurrentState)
            {
                case KindgomState.Idle:
                    {
                        if (time.TotalElapsedMs - LastQuestTime >= 10000)
                        {
                            EnsureQuest();
                            LastQuestTime = time.TotalElapsedMs;
                        }

                        if (time.TotalElapsedMs - LastAnnouncementTime >= 20000)
                        {
                            HandleAnnouncements();
                            LastAnnouncementTime = time.TotalElapsedMs;
                        }

                        if (time.TotalElapsedMs - LastEnsurePopulationTime >= 60000)
                        {
                            EnsurePopulation();
                            LastEnsurePopulationTime = time.TotalElapsedMs;
                        }
                    }
                    break;
                case KindgomState.Closing:
                    {
                        DisableSpawning = true;
                        World.GameServer.WorldManager.Nexus.PortalMonitor.RemovePortal(World.Id);

                        BroadcastMsg("RAAHH MY TROOPS HAVE FAILED ME!");
                        BroadcastMsg("THIS KINDOM SHALL NOT FALL!!");

                        CurrentState = KindgomState.Emptying;
                    }
                    break;
                case KindgomState.Emptying:
                    {
                        CurrentState = KindgomState.DoNothing;
                        foreach (var e in World.Enemies.Values)
                        {
                            if (e.ObjectDesc.ObjectId.Contains("Oryx Guardian TaskMaster") || e.ObjectDesc.ObjectId.Contains("Talisman King's Golden Guardian"))
                                continue;
                            World.LeaveWorld(e);
                        }

                        World.Timers.Add(new WorldTimer(30000, (w, t) => CurrentState = KindgomState.Closed));
                    }
                    break;
                case KindgomState.Closed:
                    {
                        BroadcastMsg("ENOUGH WAITING!");
                        BroadcastMsg("YOU SHALL MEET YOUR DOOM BY MY HAND!!!");
                        BroadcastMsg("GUARDIANS AWAKEN AND KILL THESE FOOLS!!!");

                        foreach (var e in World.Enemies.Values)
                        {
                            if (e.ObjectDesc.ObjectId.Contains("Oryx Guardian TaskMaster"))
                                continue;

                            if (e.ObjectDesc.ObjectId.Contains("Talisman King's Golden Guardian"))
                                e.Death(ref time);
                        }

                        MovePeopleNaerby(time);

                        CurrentState = KindgomState.DoNothing;
                    }
                    break;
                case KindgomState.DoNothing:
                    break;
            }
        }

        public void EnsureQuest()
        {
            if (HasQuestAlready || DisableSpawning)
                return;

            var events = _events;
            var evt = events[World.Random.Next(0, events.Count)];
            var gameData = World.GameServer.Resources.GameData;

            if (gameData.ObjectDescs[gameData.IdToObjectType[evt.Item1]].PerRealmMax == 1)
                events.Remove(evt);

            SpawnEvent(evt.Item1, evt.Item2);
            HasQuestAlready = true;
        }

        public void AnnounceMVP(Enemy eventDead, string name)
        {
            var hitters = eventDead.DamageCounter.GetHitters();
            var mvp = hitters.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
            if (mvp == null)
                return;

            var playerCount = hitters.Count;
            var dmgPercentage = (float)Math.Round(100.0 * (hitters[mvp] / (double)eventDead.DamageCounter.TotalDamage), 0);
            if (eventDead.Name.Equals("Pentaract"))
                dmgPercentage = (float)Math.Round(dmgPercentage / 5, 0);

            var sb = new StringBuilder();
            sb.Append($"MVP goes to {mvp.Name} for doing {dmgPercentage}% damage to {name}");

            if (playerCount > 1)
            {
                var playerAssist = playerCount - 1;

                if (playerAssist == 1)
                    sb.Append(" one other person helping");
                else
                    sb.Append($" with {playerAssist} people helping");
            }
            else
                sb.Append(" solo");

            sb.Append("!");

            World.GameServer.ChatManager.AnnounceRealm(sb.ToString(), "The Talisman King");

            var account = World.GameServer.Database.GetAccount(mvp.AccountId);
            var guild = World.GameServer.Database.GetGuild(account.GuildId);
            var points = World.Random.Next(1, 10);

            if (guild != null)
            {
                if (!mvp.IsAdmin)
                {
                    mvp.SendInfo("You are awarded " + points + " guild points for doing the most damage to boss!");

                    guild.GuildPoints += points;
                    guild.FlushAsync();

                    if (guild.GuildPoints > 5000 && guild.GuildLootBoost < MAX_GUILD_LOOT_BOOST)
                    {
                        guild.GuildPoints -= 5000;
                        var pguild = mvp.Client.Account.GuildId;
                        guild.GuildLootBoost += .01f;
                        guild.FlushAsync();

                        var worlds = mvp.GameServer.WorldManager.GetWorlds();
                        foreach (var world in worlds)
                            world.ForeachPlayer(_ =>
                            {
                                if (_.Client.Account.GuildId == pguild)
                                    _.SendInfo($"Congratulations! Your guild's loot boost increased to {guild.GuildLootBoost:P} (max: {MAX_GUILD_LOOT_BOOST:P}).");
                            });
                    }
                }
            }
            else
                mvp.SendInfo("Sorry, points cannot be rewarded. Please join a guild first.");
        }

        public int CountingEvents(string eventDead)
        {
            HasQuestAlready = false;

            _EventCount++;

            if (_EventCount >= 30 && !DisableSpawning)
            {
                DisableSpawning = true;
                CurrentState = KindgomState.Closing;
                World.GameServer.ChatManager.AnnounceRealm("(" + _EventCount + "/30) " + eventDead + " has been defeated!", World.DisplayName);
            }
            else if (_EventCount <= 30 && !World.Closed)
                World.GameServer.ChatManager.AnnounceRealm("(" + _EventCount + "/30) " + eventDead + " has been defeated!", World.DisplayName);

            return _EventCount;
        }

        public void Init()
        {
            SpawnEvent("Talisman King", new TalismanKing(), 1024, 1024);

            var events = _events;
            var evt = events[World.Random.Next(0, events.Count)];
            var gameData = World.GameServer.Resources.GameData;

            if (gameData.ObjectDescs[gameData.IdToObjectType[evt.Item1]].PerRealmMax == 1)
                events.Remove(evt);

            var w = World.Map.Width;
            var h = World.Map.Height;
            var stats = new int[12];

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    var tile = World.Map[x, y];
                    if (tile.Terrain != TerrainType.None)
                        stats[(int)tile.Terrain - 1]++;
                }

            foreach (var i in RegionMobs)
            {
                var terrain = i.Key;
                var idx = (int)terrain - 1;
                var enemyCount = stats[idx] / i.Value.Item1;
                EnemyMaxCounts[idx] = enemyCount;
                EnemyCounts[idx] = 0;

                for (var j = 0; j < enemyCount; j++)
                {
                    var objType = GetRandomObjType(i.Value.Item2);

                    if (objType == 0)
                        continue;

                    EnemyCounts[idx] += Spawn(World.GameServer.Resources.GameData.ObjectDescs[objType], terrain, w, h);

                    if (EnemyCounts[idx] >= enemyCount)
                        break;
                }
            }

            World.Timers.Add(new WorldTimer(15000, (world, t) => SpawnEvent(evt.Item1, evt.Item2)));
        }

        public void OnEnemyKilled(Enemy enemy, Player killer)
        {
            foreach (var dat in CriticalEnemies)
                if (enemy.ObjectDesc.ObjectId == dat.Item1)
                {
                    CountingEvents(dat.Item2.NameOfDeath);
                    AnnounceMVP(enemy, dat.Item2.NameOfDeath);
                    break;
                }
        }

        private bool HasQuestAlready;

        public void OnPlayerEntered(Player player)
        {
            player.SendInfo("Welcome to Talisman's Kingdom!");
            player.SendEnemy("The Talisman King", "You are a pest to my kingdom!");
            player.SendInfo("Use [WASDQE] to move; click to shoot!");
            player.SendInfo("Type \"/help\" for more help");
        }

        private static double GetNormal(Random rand)
        {
            // Use Box-Muller algorithm
            var u1 = GetUniform(rand);
            var u2 = GetUniform(rand);
            var r = Math.Sqrt(-2.0 * Math.Log(u1));
            var theta = 2.0 * Math.PI * u2;

            return r * Math.Sin(theta);
        }

        private static double GetNormal(Random rand, double mean, double standardDeviation) => mean + standardDeviation * GetNormal(rand);

        private static double GetUniform(Random rand) => ((uint)(rand.NextDouble() * uint.MaxValue) + 1.0) * 2.328306435454494e-10;

        private void BroadcastMsg(string message) => World.GameServer.ChatManager.TalismanKing(World, message);

        private void EnsurePopulation()
        {
            RecalculateEnemyCount();

            var state = new int[12];
            var diff = new int[12];
            var c = 0;

            for (var i = 0; i < state.Length; i++)
            {
                if (EnemyCounts[i] > EnemyMaxCounts[i] * 1.5) //Kill some
                {
                    state[i] = 1;
                    diff[i] = EnemyCounts[i] - EnemyMaxCounts[i];
                    c++;
                    continue;
                }

                if (EnemyCounts[i] < EnemyMaxCounts[i] * 0.75) //Add some
                {
                    state[i] = 2;
                    diff[i] = EnemyMaxCounts[i] - EnemyCounts[i];
                    continue;
                }

                state[i] = 0;
            }

            foreach (var i in World.Enemies) //Kill
            {
                var idx = (int)i.Value.Terrain - 1;

                if (idx == -1 || state[idx] == 0 || i.Value.GetNearestEntity(10, true) != null || diff[idx] == 0)
                    continue;

                if (state[idx] == 1)
                {
                    World.LeaveWorld(i.Value);
                    diff[idx]--;
                    if (diff[idx] == 0)
                        c--;
                }

                if (c == 0)
                    break;
            }

            var w = World.Map.Width;
            var h = World.Map.Height;

            for (var i = 0; i < state.Length; i++) //Add
            {
                if (state[i] != 2)
                    continue;

                var x = diff[i];
                var t = (TerrainType)(i + 1);

                for (var j = 0; j < x;)
                {
                    var objType = GetRandomObjType(RegionMobs[t].Item2);

                    if (objType == 0)
                        continue;

                    j += Spawn(World.GameServer.Resources.GameData.ObjectDescs[objType], t, w, h);
                }
            }

            RecalculateEnemyCount();
        }

        private ushort GetRandomObjType(IEnumerable<Tuple<string, double>> dat)
        {
            var p = World.Random.NextDouble();

            double n = 0;
            ushort objType = 0;

            foreach (var k in dat)
            {
                n += k.Item2;

                if (n > p)
                {
                    objType = World.GameServer.Resources.GameData.IdToObjectType[k.Item1];
                    break;
                }
            }

            return objType;
        }

        private void HandleAnnouncements()
        {
            if (World.Closed)
                return;

            var taunt = CriticalEnemies[World.Random.Next(0, CriticalEnemies.Length)];
            var count = 0;

            foreach (var i in World.Enemies)
            {
                var desc = i.Value.ObjectDesc;

                if (desc == null || desc.ObjectId != taunt.Item1)
                    continue;

                count++;
            }

            if (count == 0)
                return;

            if ((count == 1 && taunt.Item2.Final != null) || (taunt.Item2.Final != null && taunt.Item2.NumberOfEnemies == null))
            {
                var arr = taunt.Item2.Final;
                var msg = arr[World.Random.Next(0, arr.Length)];

                BroadcastMsg(msg);
            }
            else
            {
                var arr = taunt.Item2.NumberOfEnemies;

                if (arr == null)
                    return;

                var msg = arr[World.Random.Next(0, arr.Length)];
                msg = msg.Replace("{COUNT}", count.ToString());

                BroadcastMsg(msg);
            }
        }

        private void RecalculateEnemyCount()
        {
            for (var i = 0; i < EnemyCounts.Length; i++)
                EnemyCounts[i] = 0;

            foreach (var i in World.Enemies)
            {
                if (i.Value.Terrain == TerrainType.None)
                    continue;

                EnemyCounts[(int)i.Value.Terrain - 1]++;
            }
        }

        private int Spawn(ObjectDesc desc, TerrainType terrain, int w, int h)
        {
            Entity entity;

            var ret = 0;
            var pt = new IntPoint();

            if (desc.Spawn != null)
            {
                var num = (int)GetNormal(World.Random, desc.Spawn.Mean, desc.Spawn.StdDev);

                if (num > desc.Spawn.Max)
                    num = desc.Spawn.Max;
                else if (num < desc.Spawn.Min)
                    num = desc.Spawn.Min;

                do
                {
                    pt.X = World.Random.Next(0, w);
                    pt.Y = World.Random.Next(0, h);
                } while (World.Map[pt.X, pt.Y].Terrain != terrain || !World.IsPassable(pt.X, pt.Y) || World.AnyPlayerNearby(pt.X, pt.Y));

                for (var k = 0; k < num; k++)
                {
                    entity = Entity.Resolve(World.GameServer, desc.ObjectType);
                    entity.Move(pt.X + (float)(World.Random.NextDouble() * 2 - 1) * 5, pt.Y + (float)(World.Random.NextDouble() * 2 - 1) * 5);
                    (entity as Enemy).Terrain = terrain;
                    World.EnterWorld(entity);
                    ret++;
                }

                return ret;
            }

            do
            {
                pt.X = World.Random.Next(0, w);
                pt.Y = World.Random.Next(0, h);
            }
            while (World.Map[pt.X, pt.Y].Terrain != terrain || !World.IsPassable(pt.X, pt.Y) || World.AnyPlayerNearby(pt.X, pt.Y));

            entity = Entity.Resolve(World.GameServer, desc.ObjectType);
            entity.Move(pt.X, pt.Y);
            (entity as Enemy).Terrain = terrain;
            World.EnterWorld(entity);
            ret++;
            return ret;
        }

        private void MovePeopleNaerby(TickTime time)
        {
            var regions = World.Map.Regions.Where(t => t.Value == TileRegion.Defender).ToArray();
            foreach (var player in World.Players.Values)
            {
                var pos = regions[World.Random.Next(regions.Length)];
                player.TeleportPosition(time, pos.Key.X, pos.Key.Y, true);
            }
        }

        private void SpawnEvent(string name, ISetPiece setpiece, int x = 0, int y = 0)
        {
            if (DisableSpawning)
                return;

            var pt = new IntPoint(x, y);
            while (World.Map[pt.X, pt.Y].Terrain < TerrainType.Mountains || World.Map[pt.X, pt.Y].Terrain > TerrainType.MidForest || !World.IsPassable(pt.X, pt.Y, true) || World.AnyPlayerNearby(pt.X, pt.Y))
            {
                pt.X = World.Random.Next(0, World.Map.Width);
                pt.Y = World.Random.Next(0, World.Map.Height);
            }

            var sp = setpiece ?? new NamedEntitySetPiece(name);

            pt.X -= (sp.Size - 1) / 2;
            pt.Y -= (sp.Size - 1) / 2;
            sp.RenderSetPiece(World, pt);
            World.ForeachPlayer(_ => _.CheckForEncounter());

            var taunt = $"{name} has been spawned!";

            BroadcastMsg(taunt);
        }

        private struct TauntData
        {
            public string[] Final;
            public string[] Killed;
            public string NameOfDeath;
            public string[] NumberOfEnemies;
            public string[] Spawn;
        }
    }
}
