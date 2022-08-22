using Bot;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Bot
{
    public struct Payload
    {
        public int Id;
        public byte[] Data;
    }

    public class Bot
    {
        public Socket Socket;
        public ConcurrentQueue<Payload> Messages = new ConcurrentQueue<Payload>();

        public int CharacterId;

        public Bot(string guid, string password, int gameId, int characterId)
        {
            CharacterId = characterId;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Socket.Connect("104.194.8.2", 2001);

                var hello = MessageHelper.Hello(gameId, guid, password);
                SendMessage(ref hello);

                Console.WriteLine($"{guid} Connected");
            }
            catch
            {
                Console.WriteLine($"{guid} Failed to Connect");
            }
        }

        public async void Run()
        {
            await Task.Factory.StartNew(() =>
            {
                var playerId = -1;

                var x = 0.0f;
                var y = 0.0f;
                var lastTickId = 0;
                var angle = 0.0f;

                HandleReceive();

                while (Socket.Connected)
                {
                    var lastUpdate = Environment.TickCount;

                    while (Messages.TryDequeue(out var payload))
                    {
                        var ms = new MemoryStream(payload.Data);
                        using var rdr = new NetworkReader(ms);

                        switch (payload.Id)
                        {
                            case MessageHelper.FAILURE:
                                {
                                    var errorCode = rdr.ReadInt32();
                                    var errorDescription = rdr.ReadUTF();

                                    Console.WriteLine($"Failure: {errorCode} - {errorDescription}");
                                }
                                break;
                            case MessageHelper.MAPINFO:
                                {
                                    var width = rdr.ReadInt32();
                                    var height = rdr.ReadInt32();
                                    var name = rdr.ReadUTF();
                                    var displayName = rdr.ReadUTF();
                                    var seed = rdr.ReadUInt32();
                                    var background = rdr.ReadInt32();
                                    var difficulty = rdr.ReadInt32();
                                    var allowTeleport = rdr.ReadBoolean();
                                    var showDisplays = rdr.ReadBoolean();
                                    var music = rdr.ReadUTF();
                                    var disableShooting = rdr.ReadBoolean();
                                    var disableAbilitites = rdr.ReadBoolean();

                                    if (CharacterId == -1)
                                    {
                                        var create = MessageHelper.Create(0x030e, 0);
                                        SendMessage(ref create);
                                    }
                                    else
                                    {
                                        var load = MessageHelper.Load(CharacterId);
                                        SendMessage(ref load);
                                    }
                                }
                                break;
                            case MessageHelper.CREATE_SUCCESS:
                                {
                                    playerId = rdr.ReadInt32();
                                    CharacterId = rdr.ReadInt32();
                                }
                                break;
                            case MessageHelper.UPDATE:
                                {
                                    var len = rdr.ReadInt16();
                                    var tiles = new List<TileData>(len);
                                    for (var i = 0; i < len; i++)
                                        tiles.Add(new TileData(rdr));

                                    len = rdr.ReadInt16();
                                    var newObjs = new List<ObjectData>(len);
                                    for (var i = 0; i < len; i++)
                                        newObjs.Add(new ObjectData(rdr));

                                    len = rdr.ReadInt16();
                                    var drops = new List<int>(len);
                                    for (var i = 0; i < len; i++)
                                        drops.Add(rdr.ReadInt32());

                                    foreach (var obj in newObjs)
                                    {
                                        if (obj.Status.ObjectId == playerId)
                                        {
                                            x = obj.Status.X;
                                            y = obj.Status.Y;
                                        }
                                    }
                                }
                                break;
                            case MessageHelper.NEWTICK:
                                {
                                    lastTickId = rdr.ReadInt32();
                                    var tickTime = rdr.ReadInt32();
                                    var statuses = new List<ObjectStatusData>(rdr.ReadInt16());
                                    for (var i = 0; i < statuses.Capacity; i++)
                                        statuses.Add(new ObjectStatusData(rdr));

                                    angle += 2;
                                    x -= (float)Math.Cos(angle) * 0.4f * (tickTime / 1000.0f) * 5.0f;
                                    y -= (float)Math.Sin(angle) * 0.4f * (tickTime / 1000.0f) * 5.0f;

                                    // todo
                                    var move = MessageHelper.Move(lastTickId, lastUpdate, x, y);
                                    SendMessage(ref move);
                                }
                                break;
                            case MessageHelper.ALLYSHOOT:
                                {

                                }
                                break;
                            case MessageHelper.PING:
                                {
                                    var pong = MessageHelper.Pong(rdr.ReadInt32(), Environment.TickCount);
                                    SendMessage(ref pong);
                                }
                                break;
                            case MessageHelper.GOTOACK:
                                {

                                }
                                break;
                        }
                    }

                    Thread.Sleep(16);
                }

                Console.WriteLine("ENDED");
                Socket.Close();
            });
        }

        public async void HandleReceive()
        {
            try
            {
                var headerBuffer = new byte[5];

                while (Socket.Connected)
                {
                    var length = await Socket.ReceiveAsync(headerBuffer, SocketFlags.None);
                    if (length != 5)
                    {
                        Console.WriteLine("length != 5");
                        Socket.Close();
                        break;
                    }
                    
                    length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headerBuffer, 0)) - 5;

                    var payload = new Payload();
                    payload.Id = headerBuffer[4];
                    payload.Data = new byte[length];

                    Array.Clear(headerBuffer, 0, headerBuffer.Length);

                    var payloadCount = await Socket.ReceiveAsync(payload.Data, SocketFlags.None);
                    if (payloadCount != length)
                    {
                        Console.WriteLine($"{payload.Id} | {payloadCount} != {length}");
                        Socket.Close();
                        break;
                    }

                    Messages.Enqueue(payload);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Socket.Close();
            }
        }

        public void SendMessage(ref OutgoingMessageData outgoingMessageData)
        {
            var buffer = outgoingMessageData.GetBuffer();
            try
            {
                _ = Socket.Send(buffer);
            }
            catch
            {
            }
        }
    }

    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var bots = new List<Bot>();

            for (var i = 0; i < 63; i++)
            {
                var bot = new Bot($"test{i}@gmail.com", "12345", -2, 1);
                bot.Run();
                bots.Add(bot);
            }

            while (true)
            {
                Thread.Sleep(16);
            }
        }
    }

    public struct TileData
    {
        public readonly short X;
        public readonly short Y;
        public readonly ushort Type;

        public TileData(NetworkReader rdr)
        {
            X = rdr.ReadInt16();
            Y = rdr.ReadInt16();
            Type = rdr.ReadUInt16();
        }
    }

    public struct ObjectData
    {
        public readonly ushort Type;
        public readonly ObjectStatusData Status;

        public ObjectData(NetworkReader rdr)
        {
            Type = rdr.ReadUInt16();
            Status = new ObjectStatusData(rdr);
        }
    }

    public struct ObjectStatusData
    {
        public readonly int ObjectId;
        public readonly float X;
        public readonly float Y;
        public readonly List<StatData> StatDatas;

        public ObjectStatusData(NetworkReader rdr)
        {
            ObjectId = rdr.ReadInt32();
            X = rdr.ReadSingle();
            Y = rdr.ReadSingle();
            StatDatas = new List<StatData>(rdr.ReadInt16());
            for(var i = 0; i < StatDatas.Capacity; i++)
                StatDatas.Add(new StatData(rdr));
        }
    }

    public struct StatData
    {
        public const byte MAX_HP_STAT = 0;
        public const byte HP_STAT = 1;
        public const byte SIZE_STAT = 2;
        public const byte MAX_MP_STAT = 3;
        public const byte MP_STAT = 4;
        public const byte NEXT_LEVEL_EXP_STAT = 5;
        public const byte EXP_STAT = 6;
        public const byte LEVEL_STAT = 7;
        public const byte ATTACK_STAT = 20;
        public const byte DEFENSE_STAT = 21;
        public const byte SPEED_STAT = 22;
        public const byte INVENTORY_0_STAT = 8;
        public const byte INVENTORY_1_STAT = 9;
        public const byte INVENTORY_2_STAT = 10;
        public const byte INVENTORY_3_STAT = 11;
        public const byte INVENTORY_4_STAT = 12;
        public const byte INVENTORY_5_STAT = 13;
        public const byte INVENTORY_6_STAT = 14;
        public const byte INVENTORY_7_STAT = 15;
        public const byte INVENTORY_8_STAT = 16;
        public const byte INVENTORY_9_STAT = 17;
        public const byte INVENTORY_10_STAT = 18;
        public const byte INVENTORY_11_STAT = 19;
        public const byte VITALITY_STAT = 26;
        public const byte WISDOM_STAT = 27;
        public const byte DEXTERITY_STAT = 28;
        public const byte CONDITION_STAT = 29;
        public const byte NUM_STARS_STAT = 30;
        public const byte NAME_STAT = 31;
        public const byte TEX1_STAT = 32;
        public const byte TEX2_STAT = 33;
        public const byte MERCHANDISE_TYPE_STAT = 34;
        public const byte CREDITS_STAT = 35;
        public const byte MERCHANDISE_PRICE_STAT = 36;
        public const byte ACTIVE_STAT = 37;
        public const byte ACCOUNT_ID_STAT = 38;
        public const byte FAME_STAT = 39;
        public const byte MERCHANDISE_CURRENCY_STAT = 40;
        public const byte CONNECT_STAT = 41;
        public const byte MERCHANDISE_COUNT_STAT = 42;
        public const byte MERCHANDISE_MINS_LEFT_STAT = 43;
        public const byte MERCHANDISE_DISCOUNT_STAT = 44;
        public const byte MERCHANDISE_RANK_REQ_STAT = 45;
        public const byte MAX_HP_BOOST_STAT = 46;
        public const byte MAX_MP_BOOST_STAT = 47;
        public const byte ATTACK_BOOST_STAT = 48;
        public const byte DEFENSE_BOOST_STAT = 49;
        public const byte SPEED_BOOST_STAT = 50;
        public const byte VITALITY_BOOST_STAT = 51;
        public const byte WISDOM_BOOST_STAT = 52;
        public const byte DEXTERITY_BOOST_STAT = 53;
        public const byte OWNER_ACCOUNT_ID_STAT = 54;
        public const byte RANK_REQUIRED_STAT = 55;
        public const byte NAME_CHOSEN_STAT = 56;
        public const byte CURR_FAME_STAT = 57;
        public const byte NEXT_CLASS_QUEST_FAME_STAT = 58;
        public const byte GLOW_COLOR = 59;
        public const byte SINK_LEVEL_STAT = 60;
        public const byte ALT_TEXTURE_STAT = 61;
        public const byte GUILD_NAME_STAT = 62;
        public const byte GUILD_RANK_STAT = 63;
        public const byte BREATH_STAT = 64;
        public const byte HEALTH_POTION_STACK_STAT = 65;
        public const byte MAGIC_POTION_STACK_STAT = 66;
        public const byte BACKPACK_0_STAT = 67;
        public const byte BACKPACK_1_STAT = 68;
        public const byte BACKPACK_2_STAT = 69;
        public const byte BACKPACK_3_STAT = 70;
        public const byte BACKPACK_4_STAT = 71;
        public const byte BACKPACK_5_STAT = 72;
        public const byte BACKPACK_6_STAT = 73;
        public const byte BACKPACK_7_STAT = 74;
        public const byte HASBACKPACK_STAT = 75;
        public const byte TEXTURE_STAT = 76;
        public const byte RANK = 77;
        public const byte LD_TIMER_STAT = 78;
        public const byte BASESTAT = 79;
        public const byte POINTS = 80;
        public const byte MAXEDLIFE = 81;
        public const byte MAXEDMANA = 82;
        public const byte MAXEDATT = 83;
        public const byte MAXEDDEF = 84;
        public const byte MAXEDSPD = 85;
        public const byte MAXEDDEX = 86;
        public const byte MAXEDVIT = 87;
        public const byte MAXEDWIS = 88;
        public const byte CHAT_COLOR = 113;
        public const byte NAME_CHAT_COLOR = 114;
        public const byte GLOW_ENEMY_COLOR = 115;
        public const byte XP_BOOSTED = 116;
        public const byte XP_TIMER_BOOST = 117;
        public const byte UPGRADEENABLED = 119;
        public const byte CONDITION_STAT_2 = 120;
        public const byte PARTYID = 121;
        public const byte INVDATA0 = 122;
        public const byte INVDATA1 = 123;
        public const byte INVDATA2 = 124;
        public const byte INVDATA3 = 125;
        public const byte INVDATA4 = 126;
        public const byte INVDATA5 = 127;
        public const byte INVDATA6 = 128;
        public const byte INVDATA7 = 129;
        public const byte INVDATA8 = 130;
        public const byte INVDATA9 = 131;
        public const byte INVDATA10 = 132;
        public const byte INVDATA11 = 133;
        public const byte BACKPACKDATA0 = 134;
        public const byte BACKPACKDATA1 = 135;
        public const byte BACKPACKDATA2 = 136;
        public const byte BACKPACKDATA3 = 137;
        public const byte BACKPACKDATA4 = 138;
        public const byte BACKPACKDATA5 = 139;
        public const byte BACKPACKDATA6 = 140;
        public const byte BACKPACKDATA7 = 141;
        public const byte SPS_LIFE_COUNT = 142;
        public const byte SPS_LIFE_COUNT_MAX = 143;
        public const byte SPS_MANA_COUNT = 144;
        public const byte SPS_MANA_COUNT_MAX = 145;
        public const byte SPS_DEFENSE_COUNT = 146;
        public const byte SPS_DEFENSE_COUNT_MAX = 147;
        public const byte SPS_ATTACK_COUNT = 148;
        public const byte SPS_ATTACK_COUNT_MAX = 149;
        public const byte SPS_DEXTERITY_COUNT = 150;
        public const byte SPS_DEXTERITY_COUNT_MAX = 151;
        public const byte SPS_SPEED_COUNT = 152;
        public const byte SPS_SPEED_COUNT_MAX = 153;
        public const byte SPS_VITALITY_COUNT = 154;
        public const byte SPS_VITALITY_COUNT_MAX = 155;
        public const byte SPS_WISDOM_COUNT = 156;
        public const byte SPS_WISDOM_COUNT_MAX = 157;
        public const byte ENGINE_VALUE = 158;
        public const byte ENGINE_TIME = 159;

        public readonly byte Type;
        public readonly string StrValue;
        public readonly int Value;

        public StatData(NetworkReader rdr)
        {
            Type = rdr.ReadByte();
            switch (Type)
            {
                case NAME_STAT:
                case GUILD_NAME_STAT:
                case INVDATA0:
                case INVDATA1:
                case INVDATA2:
                case INVDATA3:
                case INVDATA4:
                case INVDATA5:
                case INVDATA6:
                case INVDATA7:
                case INVDATA8:
                case INVDATA9:
                case INVDATA10:
                case INVDATA11:
                case BACKPACKDATA0:
                case BACKPACKDATA1:
                case BACKPACKDATA2:
                case BACKPACKDATA3:
                case BACKPACKDATA4:
                case BACKPACKDATA5:
                case BACKPACKDATA6:
                case BACKPACKDATA7:
                    StrValue = rdr.ReadUTF();
                    Value = 0;
                    break;

                default:
                    StrValue = null;
                    Value = rdr.ReadInt32();
                    break;
            }
        }
    }

    public static class MessageHelper
    {
        public const byte FAILURE = 0;
        public const byte CREATE_SUCCESS = 1;
        public const byte CREATE = 2;
        public const byte PLAYERSHOOT = 3;
        public const byte MOVE = 4;
        public const byte PLAYERTEXT = 5;
        public const byte TEXT = 6;
        public const byte SERVERPLAYERSHOOT = 7;
        public const byte DAMAGE = 8;
        public const byte UPDATE = 9;
        public const byte UPDATEACK = 10;
        public const byte NOTIFICATION = 11;
        public const byte NEWTICK = 12;
        public const byte INVSWAP = 13;
        public const byte USEITEM = 14;
        public const byte SHOWEFFECT = 15;
        public const byte HELLO = 16;
        public const byte GOTO = 17;
        public const byte INVDROP = 18;
        public const byte INVRESULT = 19;
        public const byte RECONNECT = 20;
        public const byte PING = 21;
        public const byte PONG = 22;
        public const byte MAPINFO = 23;
        public const byte LOAD = 24;
        public const byte PIC = 25;
        public const byte SETCONDITION = 26;
        public const byte TELEPORT = 27;
        public const byte USEPORTAL = 28;
        public const byte DEATH = 29;
        public const byte BUY = 30;
        public const byte BUYRESULT = 31;
        public const byte AOE = 32;
        public const byte GROUNDDAMAGE = 33;
        public const byte PLAYERHIT = 34;
        public const byte ENEMYHIT = 35;
        public const byte AOEACK = 36;
        public const byte SHOOTACK = 37;
        public const byte OTHERHIT = 38;
        public const byte SQUAREHIT = 39;
        public const byte GOTOACK = 40;
        public const byte EDITACCOUNTLIST = 41;
        public const byte ACCOUNTLIST = 42;
        public const byte QUESTOBJID = 43;
        public const byte CHOOSENAME = 44;
        public const byte NAMERESULT = 45;
        public const byte CREATEGUILD = 46;
        public const byte GUILDRESULT = 47;
        public const byte GUILDREMOVE = 48;
        public const byte GUILDINVITE = 49;
        public const byte ALLYSHOOT = 50;
        public const byte ENEMYSHOOT = 51;
        public const byte REQUESTTRADE = 52;
        public const byte TRADEREQUESTED = 53;
        public const byte TRADESTART = 54;
        public const byte CHANGETRADE = 55;
        public const byte TRADECHANGED = 56;
        public const byte ACCEPTTRADE = 57;
        public const byte CANCELTRADE = 58;
        public const byte TRADEDONE = 59;
        public const byte TRADEACCEPTED = 60;
        public const byte CLIENTSTAT = 61;
        public const byte CHECKCREDITS = 62;
        public const byte ESCAPE = 63;
        public const byte FILE = 64;
        public const byte INVITEDTOGUILD = 65;
        public const byte JOINGUILD = 66;
        public const byte CHANGEGUILDRANK = 67;
        public const byte PLAYSOUND = 68;
        public const byte GLOBAL_NOTIFICATION = 69;
        public const byte RESKIN = 70;
        public const byte UPGRADESTAT = 71;
        public const byte SWITCH_MUSIC = 73;
        public const byte FORGEFUSION = 74;
        public const byte MARKET_SEARCH = 75;
        public const byte MARKET_SEARCH_RESULT = 76;
        public const byte MARKET_BUY = 77;
        public const byte MARKET_BUY_RESULT = 78;
        public const byte MARKET_ADD = 79;
        public const byte MARKET_ADD_RESULT = 80;
        public const byte MARKET_REMOVE = 81;
        public const byte MARKET_REMOVE_RESULT = 82;
        public const byte MARKET_MY_OFFERS = 83;
        public const byte MARKET_MY_OFFERS_RESULT = 84;
        public const byte OPTIONS = 85;
        public const byte BOUNTYREQUEST = 86; /* Start the Bounty */
        public const byte BOUNTYMEMBERLISTREQUEST = 87; /* Client ask for Players in the Area */
        public const byte BOUNTYMEMBERLISTSEND = 88; /* Server sends all the data to the Client */
        public const byte PARTY_INVITE = 89;
        public const byte INVITED_TO_PARTY = 90;
        public const byte JOIN_PARTY = 91;
        public const byte POTION_STORAGE_INTERACTION = 92;
        public const byte TALISMAN_ESSENCE_DATA = 100;
        public const byte TALISMAN_ESSENCE_ACTION = 101;

        public static OutgoingMessageData Hello(int gameId, string guid, string password)
        {
            var ret = new OutgoingMessageData(HELLO);
            ret.WriteUTF16("1.3.0-bot");
            ret.WriteInt32(gameId);
            ret.WriteUTF16(guid);
            ret.WriteUTF16(password);
            ret.WriteInt32(0);
            ret.WriteInt16(0);
            ret.WriteInt32(0);
            return ret;
        }

        public static OutgoingMessageData Create(int classType, int skinType)
        {
            var ret = new OutgoingMessageData(CREATE);
            ret.WriteInt16(classType);
            ret.WriteInt16(skinType);
            return ret;
        }

        public static OutgoingMessageData Pong(int serial, int time)
        {
            var ret = new OutgoingMessageData(PONG);
            ret.WriteInt32(serial);
            ret.WriteInt32(time);
            return ret;
        }

        public static OutgoingMessageData Load(int characterId)
        {
            var ret = new OutgoingMessageData(LOAD);
            ret.WriteInt32(characterId);
            return ret;
        }

        public static OutgoingMessageData Move(int tickId, int time, float x, float y)
        {
            var ret = new OutgoingMessageData(MOVE);
            ret.WriteInt32(tickId);
            ret.WriteInt32(time);
            ret.WriteFloat(x);
            ret.WriteFloat(y);
            ret.WriteInt16(0);
            return ret;
        }
    }

    public struct OutgoingMessageData
    {
        public byte MessageId;
        public List<byte> Buffer;
        public int Length;

        public OutgoingMessageData(byte messageId)
        {
            MessageId = messageId;
            Buffer = new List<byte>();
            Length = 0;
        }

        public void WriteByte(int value)
        {
            Buffer.Add((byte)value);
            Length += 1;
        }

        public void WriteByte(byte value)
        {
            Buffer.Add(value);
            Length += 1;
        }

        public void WriteBoolean(bool value)
        {
            Buffer.Add((byte)(value ? 1 : 0));
            Length += 1;
        }

        public void WriteFloat(float value)
        {
            var val = BitConverter.GetBytes(value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += 4;
        }

        public void WriteFloat(double value)
        {
            var val = BitConverter.GetBytes((float)value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += 4;
        }

        public void WriteDouble(double value)
        {
            var val = BitConverter.GetBytes(value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += 8;
        }

        public void WriteInt16(short value)
        {
            Buffer.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
            Length += 2;
        }

        public void WriteInt16(int value)
        {
            Buffer.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value)));
            Length += 2;
        }

        public void WriteInt32(int value)
        {
            Buffer.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
            Length += 4;
        }

        public void WriteUTF16(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt16((short)bytes.Length);
            Buffer.AddRange(bytes);
            Length += bytes.Length;
        }

        public void WriteUTF32(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt32(bytes.Length);
            Buffer.AddRange(bytes);
            Length += bytes.Length;
        }

        public byte[] GetBuffer()
        {
            // 4 bytes
            // 1 byte
            // N bytes

            if (Buffer.Count != Length + 5)
            {
                Buffer.InsertRange(0, BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Length + 5)));
                Buffer.Insert(4, MessageId);
            }
            return Buffer.ToArray();
        }

        public void Reset()
        {
            MessageId = 0;
            Buffer.Clear();
            Length = 0;
        }
    }
}