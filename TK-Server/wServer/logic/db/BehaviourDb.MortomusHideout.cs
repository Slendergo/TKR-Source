using common.resources;
using System.Runtime.InteropServices;
using wServer.core.objects;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.networking.packets.outgoing;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ MortomusHideout = () => Behav()
        .Init("Mortomus Check",
            new State(
                new State("Check",
                    new EntityNotExistsTransition("HC Controller", 200, "Remove")
                    ),
                new State("Remove",
                    new OpenGate("HC Neon Wall GATE", 7)
                    )
                )
            )
        .Init("HC Controller",
             new State(
                 new ScaleHP2(30),
                 new ConditionalEffect(ConditionEffectIndex.Invincible, perm: true),
                 new State("Start",
                     new PlayerWithinTransition(4, "1")
                     ),
                 new State("1",
                     new TimedTransition(0, "2")
                     ),
                 new State("2",
                     new SetAltTexture(2),
                     new TimedTransition(100, "3")
                     ),
                 new State("3",
                     new SetAltTexture(1),
                     new Taunt(true, "You may have seen me before, but I am the true protector of Mortomus!.. Defeat me, warrior, and I shall grant you acess to what lies ahead!"),
                     new TimedTransition(2000, "4")
                     ),
                 new State("4",
                     new Taunt(true, "If you say 'Ready' Prepare to face your worse Nightmare!", "Prepare yourself...Say, 'READY' when you wish the battle to begin!"),
                     new TimedTransition(0, "5")
                     ),
                 new State("5",
                     new PlayerTextTransition("7", "ready"),
                     new SetAltTexture(3),
                     new TimedTransition(500, "6")
                     ),
                 new State("6",
                     new PlayerTextTransition("7", "ready"),
                     new SetAltTexture(4),
                     new TimedTransition(500, "6_1")
                     ),
                 new State("6_1",
                     new PlayerTextTransition("7", "ready"),
                     new SetAltTexture(3),
                     new TimedTransition(500, "6_2")
                     ),
                 new State("6_2",
                     new PlayerTextTransition("7", "ready"),
                     new SetAltTexture(4),
                     new TimedTransition(500, "6_3")
                     ),
                 new State("6_3",
                     new SetAltTexture(3),
                     new PlayerTextTransition("7", "ready"),
                     new TimedTransition(500, "6_4")
                     ),
                 new State("6_4",
                     new SetAltTexture(1),
                     new PlayerTextTransition("7", "ready")
                     ),
                 new State("7",
                     new SetAltTexture(0),
                     new OrderOnce(100, "HC South Spawner", "Stage 1"),
                     new OrderOnce(100, "HC West Spawner", "Stage 1"),
                     new OrderOnce(100, "HC East Spawner", "Stage 1"),
                     new OrderOnce(100, "Hc North Spawner", "Stage 1"),
                     new EntityExistsTransition("Haunted Golem", 999, "Check 1")
                     ),
                 new State("Check 1",
                     new EntitiesNotExistsTransition(100, "8", "Haunted Golem", "Undead Blood Bat")
                     ),
                 new State("8",
                     new SetAltTexture(2),
                     new TimedTransition(0, "9")
                     ),
                 new State("9",
                     new SetAltTexture(1),
                     new TimedTransition(2000, "10")
                     ),
                 new State("10",
                     new Taunt(true, "I hope you're prepared to meet your Doom! Next wave starts in 3 seconds!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                     new TimedTransition(0, "11")
                     ),
                 new State("11",
                     new SetAltTexture(3),
                     new TimedTransition(500, "12")
                     ),
                 new State("12",
                     new SetAltTexture(4),
                     new TimedTransition(500, "13")
                     ),
                 new State("13",
                     new SetAltTexture(3),
                     new TimedTransition(500, "14")
                     ),
                 new State("14",
                     new SetAltTexture(4),
                     new TimedTransition(500, "15")
                     ),
                 new State("15",
                     new SetAltTexture(3),
                     new TimedTransition(500, "16")
                     ),
                 new State("16",
                     new SetAltTexture(4),
                     new TimedTransition(500, "17")
                     ),
                 new State("17",
                     new SetAltTexture(3),
                     new TimedTransition(500, "18")
                     ),
                 new State("18",
                     new SetAltTexture(4),
                     new TimedTransition(500, "19")
                     ),
                 new State("19",
                     new SetAltTexture(3),
                     new TimedTransition(500, "20")
                     ),
                 new State("20",
                     new SetAltTexture(4),
                     new TimedTransition(500, "21")
                     ),
                 new State("21",
                     new SetAltTexture(3),
                     new TimedTransition(500, "22")
                     ),
                 new State("22",
                     new SetAltTexture(4),
                     new TimedTransition(500, "23")
                     ),
                 new State("23",
                     new SetAltTexture(3),
                     new TimedTransition(500, "24")
                     ),
                 new State("24",
                     new SetAltTexture(4),
                     new TimedTransition(500, "25")
                     ),
                 new State("25",
                     new SetAltTexture(1),
                     new TimedTransition(500, "26")
                     ),
                 new State("26",
                     new SetAltTexture(2),
                     new TimedTransition(100, "27")
                     ),
                 new State("27",
                     new SetAltTexture(0),
                     new OrderOnce(100, "HC South Spawner", "Stage 2"),
                     new OrderOnce(100, "HC West Spawner", "Stage 2"),
                     new OrderOnce(100, "HC East Spawner", "Stage 2"),
                     new OrderOnce(100, "HC North Spawner", "Stage 2"),
                     new EntityExistsTransition("Haunted Golem", 999, "Check 2")
                     ),
                 new State("Check 2",
                     new EntitiesNotExistsTransition(100, "28", "Haunted Golem", "Undead Blood Bat")
                     ),
                 new State("28",
                     new SetAltTexture(2),
                     new TimedTransition(0, "29")
                     ),
                 new State("29",
                     new SetAltTexture(1),
                     new TimedTransition(2000, "30")
                     ),
                 new State("30",
                     new Taunt(true, "The next wave will appear in 3 seconds. Leave or Else!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                     new TimedTransition(0, "31")
                     ),
                 new State("31",
                     new SetAltTexture(3),
                     new TimedTransition(500, "32")
                     ),
                 new State("32",
                     new SetAltTexture(4),
                     new TimedTransition(500, "33")
                     ),
                 new State("33",
                     new SetAltTexture(3),
                     new TimedTransition(500, "34")
                     ),
                 new State("34",
                     new SetAltTexture(4),
                     new TimedTransition(500, "35")
                     ),
                 new State("35",
                     new SetAltTexture(3),
                     new TimedTransition(500, "36")
                     ),
                 new State("36",
                     new SetAltTexture(4),
                     new TimedTransition(500, "37")
                     ),
                 new State("37",
                     new SetAltTexture(3),
                     new TimedTransition(500, "38")
                     ),
                 new State("38",
                     new SetAltTexture(4),
                     new TimedTransition(500, "39")
                     ),
                 new State("39",
                     new SetAltTexture(3),
                     new TimedTransition(500, "40")
                     ),
                 new State("40",
                     new SetAltTexture(4),
                     new TimedTransition(500, "41")
                     ),
                 new State("41",
                     new SetAltTexture(3),
                     new TimedTransition(500, "42")
                     ),
                 new State("42",
                     new SetAltTexture(4),
                     new TimedTransition(500, "43")
                     ),
                 new State("43",
                     new SetAltTexture(3),
                     new TimedTransition(500, "44")
                     ),
                 new State("44",
                     new SetAltTexture(4),
                     new TimedTransition(500, "45")
                     ),
                 new State("45",
                     new SetAltTexture(1),
                     new TimedTransition(100, "46")
                     ),
                 new State("46",
                     new SetAltTexture(2),
                     new TimedTransition(100, "47")
                     ),
                 new State("47",
                     new SetAltTexture(0),
                     new OrderOnce(100, "HC South Spawner", "Stage 3"),
                     new OrderOnce(100, "HC West Spawner", "Stage 3"),
                     new OrderOnce(100, "HC East Spawner", "Stage 3"),
                     new OrderOnce(100, "HC North Spawner", "Stage 3"),
                     new EntityExistsTransition("Haunted Golem", 999, "Check 3")
                     ),
                 new State("Check 3",
                     new EntitiesNotExistsTransition(100, "48", "Haunted Golem", "Undead Blood Bat", "Cursed Mermaid")
                     ),
                 new State("48",
                     new SetAltTexture(2),
                     new TimedTransition(0, "49")
                     ),
                 new State("49",
                     new SetAltTexture(1),
                     new TimedTransition(2000, "50")
                     ),
                 new State("50",
                     new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                     new TimedTransition(0, "51")
                     ),
                 new State("51",
                     new SetAltTexture(3),
                     new TimedTransition(500, "52")
                     ),
                 new State("52",
                     new SetAltTexture(4),
                     new TimedTransition(500, "53")
                     ),
                 new State("53",
                     new SetAltTexture(3),
                     new TimedTransition(500, "54")
                     ),
                 new State("54",
                     new SetAltTexture(4),
                     new TimedTransition(500, "55")
                     ),
                 new State("55",
                     new SetAltTexture(3),
                     new TimedTransition(500, "56")
                     ),
                 new State("56",
                     new SetAltTexture(4),
                     new TimedTransition(500, "57")
                     ),
                 new State("57",
                     new SetAltTexture(3),
                     new TimedTransition(500, "58")
                     ),
                 new State("58",
                     new SetAltTexture(4),
                     new TimedTransition(500, "59")
                     ),
                 new State("59",
                     new SetAltTexture(3),
                     new TimedTransition(500, "60")
                     ),
                 new State("60",
                     new SetAltTexture(4),
                     new TimedTransition(500, "61")
                     ),
                 new State("61",
                     new SetAltTexture(3),
                     new TimedTransition(500, "62")
                     ),
                 new State("62",
                     new SetAltTexture(4),
                     new TimedTransition(500, "63")
                     ),
                 new State("63",
                     new SetAltTexture(3),
                     new TimedTransition(500, "64")
                     ),
                 new State("64",
                     new SetAltTexture(4),
                     new TimedTransition(500, "65")
                     ),
                 new State("65",
                     new SetAltTexture(1),
                     new TimedTransition(100, "66")
                     ),
                 new State("66",
                     new SetAltTexture(2),
                     new TimedTransition(100, "67")
                     ),
                 new State("67",
                     new SetAltTexture(0),
                     new OrderOnce(100, "HC South Spawner", "Stage 4"),
                     new OrderOnce(100, "HC West Spawner", "Stage 4"),
                     new OrderOnce(100, "HC East Spawner", "Stage 4"),
                     new OrderOnce(100, "HC North Spawner", "Stage 4"),
                     //new OrderOnce(100, "HC Zombie Spawner", "Stage 4"),
                     new EntityExistsTransition("Haunted Golem", 999, "Check 4")
                     ),
                 new State("Check 4",
                     new EntitiesNotExistsTransition(100, "68", "Haunted Golem", "Undead Blood Bat", "Cursed Mermaid", "Crawling Devourer")
                     ),
                 new State("68",
                     new SetAltTexture(2),
                     new TimedTransition(0, "69")
                     ),
                 new State("69",
                     new SetAltTexture(1),
                     new TimedTransition(2000, "70")
                     ),
                new State("70",
                     new Taunt(true, "NOOO! Mortomus! I have failed you! They have awoken you!"),
                     new TimedTransition(0, "71")
                     ),
                 new State("71",
                     new SetAltTexture(3),
                     new TimedTransition(500, "72")
                     ),
                 new State("72",
                     new SetAltTexture(4),
                     new TimedTransition(500, "73")
                     ),
                 new State("73",
                     new SetAltTexture(3),
                     new TimedTransition(500, "74")
                     ),
                 new State("74",
                     new SetAltTexture(4),
                     new TimedTransition(500, "75")
                     ),
                 new State("75",
                     new SetAltTexture(3),
                     new TimedTransition(500, "76")
                     ),
                 new State("76",
                     new SetAltTexture(4),
                     new TimedTransition(500, "77")
                     ),
                 new State("77",
                     new SetAltTexture(3),
                     new TimedTransition(500, "78")
                     ),
                 new State("78",
                     new SetAltTexture(4),
                     new TimedTransition(500, "79")
                     ),
                 new State("79",
                     new SetAltTexture(3),
                     new TimedTransition(500, "80")
                     ),
                 new State("80",
                     new SetAltTexture(4),
                     new TimedTransition(500, "81")
                     ),
                 new State("81",
                     new SetAltTexture(3),
                     new TimedTransition(500, "82")
                     ),
                 new State("82",
                     new SetAltTexture(4),
                     new TimedTransition(500, "83")
                     ),
                 new State("83",
                     new SetAltTexture(3),
                     new TimedTransition(500, "84")
                     ),
                 new State("84",
                     new SetAltTexture(4),
                     new TimedTransition(500, "85")
                     ),
                 new State("85",
                     new SetAltTexture(1),
                     new TimedTransition(100, "86")
                     ),
                 new State("86",
                     new SetAltTexture(2),
                     new TimedTransition(100, "87")
                     ),
                 new State("87",
                     new SetAltTexture(0),
                     new TimedTransition(1000, "Suicide")
                     ),
                 new State("Suicide",
                    new Suicide()
                    ),
                new State("Remove",
                    //new OpenGate("HC Neon Wall TEST", 7),
                    new Suicide()
                    )
                )
            )
         .Init("HC South Spawner",
             new State(
                 new ScaleHP2(30),
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new State("Leech"
                     ),
                  new State("Stage 1",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 2",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 2)
                     ),
                 new State("Stage 3",
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 4",
                     new Spawn("Crawling Devourer", maxChildren: 1),
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1),
                     new TimedTransition(1000, "Suicide")
                     ),
                 new State("Suicide",
                     new Suicide()
                     )
                 )
             )
         .Init("HC East Spawner",
             new State(
                 new ScaleHP2(30),
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new State("Leech"
                     ),
                  new State("Stage 1",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 2",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 2)
                     ),
                 new State("Stage 3",
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 4",
                     new Spawn("Crawling Devourer", maxChildren: 1),
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1),
                     new TimedTransition(1000, "Suicide")
                     ),
                 new State("Suicide",
                     new Suicide()
                     )
                 )
             )
         .Init("HC West Spawner",
             new State(
                 new ScaleHP2(30),
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new State("Leech"
                     ),
                  new State("Stage 1",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 2",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 2)
                     ),
                 new State("Stage 3",
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 4",
                     new Spawn("Crawling Devourer", maxChildren: 1),
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1),
                     new TimedTransition(1000, "Suicide")
                     ),
                 new State("Suicide",
                     new Suicide()
                     )
                 )
             )
         .Init("HC North Spawner",
             new State(
                 new ScaleHP2(30),
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new State("Leech"
                     ),
                  new State("Stage 1",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 2",
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 2)
                     ),
                 new State("Stage 3",
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1)
                     ),
                 new State("Stage 4",
                     new Spawn("Crawling Devourer", maxChildren: 1),
                     new Spawn("Cursed Mermaid", maxChildren: 1),
                     new Spawn("Undead Blood Bat", maxChildren: 1),
                     new Spawn("Haunted Golem", maxChildren: 1),
                     new TimedTransition(1000, "Suicide")
                     ),
                 new State("Suicide",
                     new Suicide()
                     )
                 )
             )
        .Init("Mortomus, Keeper of Souls",
            new State(
                new ScaleHP2(30),
                new State("Awaiting",
                    new Taunt("Defeat my MINIONS to gain access to me!"),
                    new EntitiesNotExistsTransition(200, "idle", "Cursed Mermaid", "Haunted Golem", "Crawling Devourer", "Undead Blood Bat")
                    ),
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new PlayerWithinTransition(20, "prepare")
                    ),
                new State("prepare",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("AT LAST!"),
                    new Taunt("Allow me to introduce myself!"),
                    new ChangeSize(5, 150),
                    new Flash(0xFF2B2B, 5, 10),
                    new TimedTransition(5000, "rage")
                    ),
                new State("rage",
                    new Chase(4),
                    new TossObject("Undead Blood Bat", 4, 90, coolDown: 8000),
                    new TossObject("Undead Blood Bat", 4, 270, coolDown: 8000),
                    new Shoot(12, 4, shootAngle: 10, projectileIndex: 0, coolDown: 400),
                    new HpLessTransition(.85, "Prepare attack1")
                    ),
                new State("Prepare attack1",
                    new ReturnToSpawn(3),
                    new RemoveEntity(100, "Undead Blood Bat"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("Now that we’ve had a proper introduction, let me end you NOW!"),
                    new TimedTransition(5000, "attack1")
                    ),
                new State("attack1",
                    new ConditionalEffect(ConditionEffectIndex.Armored, false),
                    new Shoot(20, 2, shootAngle: 20, projectileIndex: 0, predictive: 1, coolDown: 300),
                    new Shoot(12, 1, projectileIndex: 1, predictive: 1.2, coolDown: 600),
                    new TimedTransition(8000, "attack2.1")
                    ),
                new State("attack2.1",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 300),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 500),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.2")
                    ),
                new State("attack2.2",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.3")
                    ),
                new State("attack2.3",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.4")
                    ),
                new State("attack2.4",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.5")
                    ),
                new State("attack2.5",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.6")
                    ),
                new State("attack2.6",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.7")
                    ),
                new State("attack2.7",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.8")
                    ),
                new State("attack2.8",
                    new Wander(.25),
                    new HpLessTransition(.6, "attack3"),
                    new Shoot(10, 4, projectileIndex: 2, shootAngle: 5, predictive: 1.2, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 3, shootAngle: 10, predictive: .8, coolDown: 800),
                    new Grenade(radius: 2, damage: 80, range: 30, coolDown: 800, effect: ConditionEffectIndex.Darkness, effectDuration: 1000, color: 0xffffff),
                    new Grenade(3, 65, range: 7, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 10, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 13, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(1000, "attack2.1")
                    ),
                new State("attack3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("HAHAHAH!"),
                    new Chase(4),
                    new Shoot(10, 2, projectileIndex: 5, shootAngle: 20, predictive: 1, coolDown: 350),
                    new Grenade(3, 65, range: 8, fixedAngle: 0, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 45, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 90, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 135, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 180, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 225, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 270, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new Grenade(3, 65, range: 8, fixedAngle: 315, coolDown: 800, ConditionEffectIndex.Bleeding, 2000, color: 0x5279FD),
                    new TimedTransition(15000, "attack4")
                    ),
                new State("attack4",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new ReturnToSpawn(3),
                    new TimedTransition(1000, "attack4.1")
                    ),
                new State("attack4.1",
                    new ReturnToSpawn(3),
                    new Shoot(12, 3, projectileIndex: 6, shootAngle: 15, predictive: 1, coolDown: 400),
                    new Shoot(15, 1, projectileIndex: 7, predictive: 1.2, coolDown: 800),
                    new TossObject("Haunted Bat", 4, 0, coolDown: 999999, coolDownOffset: 100),
                    new TossObject("Haunted Bat", 4, 180, coolDown: 999999, coolDownOffset: 100),
                    new TossObject("Killer Bat", 6, 45, coolDown: 999999, coolDownOffset: 500),
                    new TossObject("Killer Bat", 6, 225, coolDown: 999999, coolDownOffset: 500),
                    new TossObject("Demon Bat", 8, 90, coolDown: 999999, coolDownOffset: 1000),
                    new TossObject("Demon Bat", 8, 270, coolDown: 999999, coolDownOffset: 1000),
                    new HpLessTransition(.35, "attack5")
                    ),
                new State("attack5",
                    new RemoveEntity(100, "Haunted Bat"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new Taunt("MY CHILDREN! I SUMMON YOU!"),
                    new TimedTransition(2000, "attack5.1")
                    ),
                new State("attack5.1",
                    new Wander(.25),
                    new TossObject("Undead Blood Bat", 5, 0, coolDown: 99999),
                    new TossObject("Crawling Devourer", 5, 90, coolDown: 99999),
                    new TossObject("Cursed Mermaid", 5, 180, coolDown: 99999),
                    new TossObject("Haunted Golem", 5, 270, coolDown: 99999),
                    new Shoot(10, 4, projectileIndex: 8, shootAngle: 10, predictive: 1, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 9, shootAngle: 20, predictive: .8, coolDown: 350),
                    new HpLessTransition(.15, "attack6")
                    ),
                new State("attack6",
                    new ReturnToSpawn(3),
                    new RemoveEntity(100, "Undead Blood Bat"),
                    new RemoveEntity(100, "Crawling Devourer"),
                    new RemoveEntity(100, "Cursed Mermaid"),
                    new RemoveEntity(100, "Haunted Golem"),
                    new Shoot(10, 4, projectileIndex: 10, shootAngle: 10, predictive: 1, coolDown: 500),
                    new Shoot(15, 2, projectileIndex: 11, shootAngle: 20, predictive: .8, coolDown: 350),
                    new HpLessTransition(.1, "dead")
                    ),
                new State("dead",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, false),
                    new Taunt("M-my children!!! NOOOO!"),
                    new Shoot(20, 25, projectileIndex: 10, coolDown: 99999),
                    new TimedTransition(500, "dead1")
                    ),
                new State("dead1",
                    new Suicide()
                     )
                ),
                new Threshold(0.01,
                LootTemplates.DustLoot()
                    ),
            new Threshold(0.05,
                new TierLoot(12, ItemType.Weapon, 0.05),
                new TierLoot(13, ItemType.Armor, 0.05),
                new TierLoot(5, ItemType.Ring, 0.05),
                new TierLoot(6, ItemType.Ability, 0.07),
                new ItemLoot("Potion of Wisdom", 0.5),
                new ItemLoot("Potion of Speed", 0.5),
                new ItemLoot("Potion of Vitality", 0.5),
                new ItemLoot("Potion of Dexterity", 0.5),
                new ItemLoot("Potion of Life", 0.5),
                new ItemLoot("Potion of Mana", 0.5),
                new ItemLoot("Potion of Defense", 0.5),
                new ItemLoot("Potion of Attack", 0.5),
                new ItemLoot("Old Cleric's Cloak", 0.0016),
                new ItemLoot("Mortomus' Shovel", 0.0001),
                new ItemLoot("Ghostly Warrior's Lantern", 0.0016),
                new ItemLoot("Scepter of Whispers", 0.0016),
                new ItemLoot("Magic Dust", 0.5)
                )
            )
        .Init("Undead Blood Bat",
            new State(
                new ScaleHP2(4),
                new State("attack",
                    new Taunt("SCREEECH"),
                    new Charge(3, 10, coolDown: 1500),
                    new Shoot(2, 8, shootAngle: 30, projectileIndex: 0, predictive: .7, coolDown: 500),
                    new Shoot(6, 1, projectileIndex: 0, predictive: 1, coolDown: 250)
                    )
                )
            )
        .Init("Haunted Bat",
            new State(
                new ScaleHP2(4),
                new Taunt("SCREEECH"),
                //new OrbitBehavior("Mortomus, Keeper of Souls", speedVariability: 0, speed: 0.5, radius: 4.0, radiusVariability: 0),
                new Orbit(0.5,4.0),
                new Shoot(30, 1, projectileIndex: 0, coolDown: 200)
                )
            )
        .Init("Killer Bat",
            new State(
                new ScaleHP2(4),
                new Taunt("SCREEECH"),
                //new OrbitBehavior("Mortomus, Keeper of Souls", speedVariability: 0, speed: 1.0, radius: 6.0, radiusVariability: 0),
                new Orbit(1, 6.0),
                new Shoot(30, 1, projectileIndex: 0, coolDown: 155)
                )
            )
        .Init("Demon Bat",
            new State(
                new ScaleHP2(4),
                 new Taunt("SCREEECH"),
                 //new OrbitBehavior("Mortomus, Keeper of Souls", speedVariability: 0, speed: 1.5, radius: 8.0, radiusVariability: 0),
                 new Orbit(1, 8.0),
                 new Shoot(30, 1, projectileIndex: 0, coolDown: 55)
                )
            )
        .Init("Haunted Golem",
            new State(
                new ScaleHP2(5),
                new State("attack",
                    new Chase(4),
                    new Shoot(4, 3, shootAngle: 10, projectileIndex: 0, predictive: .9, coolDown: 800)
                    )
                )
            )
        .Init("Crawling Devourer",
            new State(
                new ScaleHP2(4),
                new State("attack",
                    new Wander(.25),
                    new Shoot(6, 5, shootAngle: 12, projectileIndex: 0, predictive: 1.2, coolDown: 1000)
                    )
                )
            )
        .Init("Cursed Mermaid",
            new State(
                new ScaleHP2(5),
                 new State("attack",
                     new Wander(.15),
                     new Grenade(radius: 3, damage: 80, range: 8, coolDown: 2000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000, color: 0xffffff),
                     new Shoot(8, 3, shootAngle: 15, projectileIndex: 0, predictive: 1, coolDown: 1500)
                    )
                ));
    }
}