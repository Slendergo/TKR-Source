using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
       private _ HauntedCemetery = () => Behav()
        .Init("Arena Dn Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Classic Ghost", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                new State("Stage 4",
                    new Spawn("Werewolf", maxChildren: 1),
                    new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    )
                )
            )
        .Init("Arena Up Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Classic Ghost", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Werewolf", maxChildren: 1)
                    ),
                new State("Stage 3",
                    new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                new State("Stage 4",
                    new Spawn("Classic Ghost", maxChildren: 3)
                    ),
                new State("Stage 5",
                    new Suicide()
                    )
                )
            )
        .Init("Arena Lf Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Werewolf", maxChildren: 1),
                    new Spawn("Classic Ghost", maxChildren: 1)
                    ),
                new State("Stage 3",
                    new Spawn("Classic Ghost", maxChildren: 1),
                    new Spawn("Werewolf", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Werewolf", maxChildren: 1),
                    new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    )
                )
            )
        .Init("Arena Rt Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Werewolf", maxChildren: 1)
                    ),
                new State("Stage 3",
                    new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                new State("Stage 4",
                    new Spawn("Classic Ghost", maxChildren: 3)
                    ),
                new State("Stage 5",
                    new Suicide()
                    )
                )
            )
        .Init("Area 1 Controller",
            new State(
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
                    new Taunt(true, "Welcome to my domain. l challenge you, warrior, to defeat my undead hordes and claim your prize...."),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new Taunt(true, "Say,'READY' when you are ready to face your opponeants.", "Prepare yourself...Say, 'READY' when you wish the battle to begin!"),
                    new TimedTransition(0, "5")
                    ),
                //repeat
                new State("5",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6")
                    ),
                new State("6",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(4),
                    new TimedTransition(200, "6_1")
                    ),
                new State("6_1",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6_2")
                    ),
                new State("6_2",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(4),
                    new TimedTransition(200, "6_3")
                    ),
                new State("6_3",
                    new SetAltTexture(3),
                    new PlayerTextTransition("7", "ready"),
                    new TimedTransition(200, "6_4")
                    ),
                new State("6_4",
                    new SetAltTexture(1),
                    new PlayerTextTransition("7", "ready")
                    ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(100, "Arena South Gate Spawner", "Stage 1"),
                    new Order(100, "Arena West Gate Spawner", "Stage 1"),
                    new Order(100, "Arena East Gate Spawner", "Stage 1"),
                    new Order(100, "Arena North Gate Spawner", "Stage 1"),
                    new EntityExistsTransition("Arena Skeleton", 999, "Check 1")
                    ),
                new State("Check 1",
                    new EntitiesNotExistsTransition(100, "8", "Arena Skeleton", "Troll 1", "Troll 2")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "11")
                    ),
                new State("11",
                    new SetAltTexture(3),
                    new TimedTransition(200, "12")
                    ),
                new State("12",
                    new SetAltTexture(4),
                    new TimedTransition(200, "13")
                    ),
                new State("13",
                    new SetAltTexture(3),
                    new TimedTransition(200, "14")
                    ),
                new State("14",
                    new SetAltTexture(4),
                    new TimedTransition(200, "15")
                    ),
                new State("15",
                    new SetAltTexture(3),
                    new TimedTransition(200, "16")
                    ),
                new State("16",
                    new SetAltTexture(4),
                    new TimedTransition(200, "17")
                    ),
                new State("17",
                    new SetAltTexture(3),
                    new TimedTransition(200, "18")
                    ),
                new State("18",
                    new SetAltTexture(4),
                    new TimedTransition(200, "19")
                    ),
                new State("19",
                    new SetAltTexture(3),
                    new TimedTransition(200, "20")
                    ),
                new State("20",
                    new SetAltTexture(4),
                    new TimedTransition(200, "21")
                    ),
                new State("21",
                    new SetAltTexture(3),
                    new TimedTransition(200, "22")
                    ),
                new State("22",
                    new SetAltTexture(4),
                    new TimedTransition(200, "23")
                    ),
                new State("23",
                    new SetAltTexture(3),
                    new TimedTransition(200, "24")
                    ),
                new State("24",
                    new SetAltTexture(4),
                    new TimedTransition(100, "25")
                    ),
                new State("25",
                    new SetAltTexture(1),
                    new TimedTransition(100, "26")
                    ),
                new State("26",
                    new SetAltTexture(2),
                    new TimedTransition(100, "27")
                    ),
                new State("27",
                    new SetAltTexture(0),
                    new Order(100, "Arena South Gate Spawner", "Stage 2"),
                    new Order(100, "Arena West Gate Spawner", "Stage 2"),
                    new Order(100, "Arena East Gate Spawner", "Stage 2"),
                    new Order(100, "Arena North Gate Spawner", "Stage 2"),
                    new EntityExistsTransition("Arena Skeleton", 999, "Check 2")
                    ),
                new State("Check 2",
                    new EntitiesNotExistsTransition(100, "28", "Arena Skeleton", "Troll 1", "Troll 2")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "31")
                    ),
                new State("31",
                    new SetAltTexture(3),
                    new TimedTransition(200, "32")
                    ),
                new State("32",
                    new SetAltTexture(4),
                    new TimedTransition(200, "33")
                    ),
                new State("33",
                    new SetAltTexture(3),
                    new TimedTransition(200, "34")
                    ),
                new State("34",
                    new SetAltTexture(4),
                    new TimedTransition(200, "35")
                    ),
                new State("35",
                    new SetAltTexture(3),
                    new TimedTransition(200, "36")
                    ),
                new State("36",
                    new SetAltTexture(4),
                    new TimedTransition(200, "37")
                    ),
                new State("37",
                    new SetAltTexture(3),
                    new TimedTransition(200, "38")
                    ),
                new State("38",
                    new SetAltTexture(4),
                    new TimedTransition(200, "39")
                    ),
                new State("39",
                    new SetAltTexture(3),
                    new TimedTransition(200, "40")
                    ),
                new State("40",
                    new SetAltTexture(4),
                    new TimedTransition(200, "41")
                    ),
                new State("41",
                    new SetAltTexture(3),
                    new TimedTransition(200, "42")
                    ),
                new State("42",
                    new SetAltTexture(4),
                    new TimedTransition(200, "43")
                    ),
                new State("43",
                    new SetAltTexture(3),
                    new TimedTransition(200, "44")
                    ),
                new State("44",
                    new SetAltTexture(4),
                    new TimedTransition(100, "45")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 3"),
                    new Order(100, "Arena West Gate Spawner", "Stage 3"),
                    new Order(100, "Arena East Gate Spawner", "Stage 3"),
                    new Order(100, "Arena North Gate Spawner", "Stage 3"),
                    new EntityExistsTransition("Arena Skeleton", 999, "Check 3")
                    ),
                new State("Check 3",
                    new EntitiesNotExistsTransition(100, "48", "Arena Skeleton", "Troll 1", "Troll 2")
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
                    new TimedTransition(200, "52")
                    ),
                new State("52",
                    new SetAltTexture(4),
                    new TimedTransition(200, "53")
                    ),
                new State("53",
                    new SetAltTexture(3),
                    new TimedTransition(200, "54")
                    ),
                new State("54",
                    new SetAltTexture(4),
                    new TimedTransition(200, "55")
                    ),
                new State("55",
                    new SetAltTexture(3),
                    new TimedTransition(200, "56")
                    ),
                new State("56",
                    new SetAltTexture(4),
                    new TimedTransition(200, "57")
                    ),
                new State("57",
                    new SetAltTexture(3),
                    new TimedTransition(200, "58")
                    ),
                new State("58",
                    new SetAltTexture(4),
                    new TimedTransition(200, "59")
                    ),
                new State("59",
                    new SetAltTexture(3),
                    new TimedTransition(200, "60")
                    ),
                new State("60",
                    new SetAltTexture(4),
                    new TimedTransition(200, "61")
                    ),
                new State("61",
                    new SetAltTexture(3),
                    new TimedTransition(200, "62")
                    ),
                new State("62",
                    new SetAltTexture(4),
                    new TimedTransition(200, "63")
                    ),
                new State("63",
                    new SetAltTexture(3),
                    new TimedTransition(200, "64")
                    ),
                new State("64",
                    new SetAltTexture(4),
                    new TimedTransition(100, "65")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 4"),
                    new Order(100, "Arena West Gate Spawner", "Stage 4"),
                    new Order(100, "Arena East Gate Spawner", "Stage 4"),
                    new Order(100, "Arena North Gate Spawner", "Stage 4"),
                    new EntityExistsTransition("Arena Skeleton", 999, "Check 4")
                    ),
                new State("Check 4",
                    new EntitiesNotExistsTransition(100, "68", "Arena Skeleton", "Troll 1", "Troll 2")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "71")
                    ),
                new State("71",
                    new SetAltTexture(3),
                    new TimedTransition(200, "72")
                    ),
                new State("72",
                    new SetAltTexture(4),
                    new TimedTransition(200, "73")
                    ),
                new State("73",
                    new SetAltTexture(3),
                    new TimedTransition(200, "74")
                    ),
                new State("74",
                    new SetAltTexture(4),
                    new TimedTransition(200, "75")
                    ),
                new State("75",
                    new SetAltTexture(3),
                    new TimedTransition(200, "76")
                    ),
                new State("76",
                    new SetAltTexture(4),
                    new TimedTransition(200, "77")
                    ),
                new State("77",
                    new SetAltTexture(3),
                    new TimedTransition(200, "78")
                    ),
                new State("78",
                    new SetAltTexture(4),
                    new TimedTransition(200, "79")
                    ),
                new State("79",
                    new SetAltTexture(3),
                    new TimedTransition(200, "80")
                    ),
                new State("80",
                    new SetAltTexture(4),
                    new TimedTransition(200, "81")
                    ),
                new State("81",
                    new SetAltTexture(3),
                    new TimedTransition(200, "82")
                    ),
                new State("82",
                    new SetAltTexture(4),
                    new TimedTransition(200, "83")
                    ),
                new State("83",
                    new SetAltTexture(3),
                    new TimedTransition(200, "84")
                    ),
                new State("84",
                    new SetAltTexture(4),
                    new TimedTransition(100, "85")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 5"),
                    new Order(100, "Arena West Gate Spawner", "Stage 5"),
                    new Order(100, "Arena East Gate Spawner", "Stage 5"),
                    new Order(100, "Arena North Gate Spawner", "Stage 5"),
                    new EntityExistsTransition("Troll 3", 999, "Check 5")
                    ),
                new State("Check 5",
                    new EntityNotExistsTransition("Troll 3", 100, "88")
                    ),
                new State("88",
                    new TransformOnDeath("Haunted Cemetery Gates Portal"),
                    new Suicide()
                    )
                )
            )
        .Init("Arena South Gate Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 2",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Troll 1", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Troll 1", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
               new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Suicide()
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Warrior", maxChildren: 1),
                    new Spawn("Arena Risen Archer", maxChildren: 1)
                    ),
                new State("Stage 15",
                    new Suicide()
                    )
                )
            )
        .Init("Arena East Gate Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 15",
                    new Suicide()
                    )
                )
            )
        .Init("Arena West Gate Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Archer", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Brawler", maxChildren: 1),
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Archer", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Archer", maxChildren: 1),
                    new Spawn("Arena Risen Mage", maxChildren: 1)
                    ),
                new State("Stage 15",
                    new Suicide()
                    )
                )
            )
        .Init("Arena North Gate Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Troll 1", maxChildren: 1)
                    ),
                new State("Stage 3",
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Spawn("Troll 3", maxChildren: 1)
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Spawn("Arena Ghost Bride", maxChildren: 1)
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Mage", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Warrior", maxChildren: 2)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Warrior", maxChildren: 1),
                    new Spawn("Arena Risen Mage", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Warrior", maxChildren: 2)
                    ),
                new State("Stage 15",
                    new Spawn("Arena Grave Caretaker", maxChildren: 1)
                    )
                )
            )
        .Init("Arena Skeleton",
            new State(
                new Wander(0.5),
                new Follow(0.4, 10, 3),
                new Shoot(8, 1, projectileIndex: 0, coolDown: 500)
                )
            )
        .Init("Troll 1",
            new State(
                new Follow(0.6, 14, 2, 2000, coolDown: 2000),
                new Shoot(12, 1, projectileIndex: 0, coolDown: 450)
                )
            )
        .Init("Troll 2",
            new State(
                new Wander(0.3),
                new Follow(0.7, 10, duration: 2000, coolDown: 3000),
                new Shoot(10, 1, projectileIndex: 0, coolDown: 550),
                new Grenade(3, 85, 10, coolDown: 1000)
                )
            )
        .Init("Troll 3",
            new State(
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new MoveTo(5, 0, 7),
                    new TimedTransition(2000, "2")
                    ),
                new State("2",
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 400),
                    new TossObject("Arena Mushroom", 10, coolDown: 7000),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 500),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 1, fixedAngle: 0, coolDownOffset: 1000),
                    new HpLessTransition(0.5, "3")
                    ),
                new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 400),
                    new TossObject("Arena Mushroom", 10, coolDown: 7000),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 500),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 1, fixedAngle: 0, coolDownOffset: 1000),
                    new TimedTransition(500, "Check")
                    ),
                new State("Check", //antilag :D
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Spawn("Arena Skeleton", maxChildren: 1, coolDown: 3000),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 400),
                    new TossObject("Arena Mushroom", 10, coolDown: 7000),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 500),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 1, fixedAngle: 0, coolDownOffset: 1000),
                    new EntityNotExistsTransition("Arena Skeleton", 100, "4")
                    ),
                new State("4",
                    new Prioritize(
                        new Follow(1, 20, 1, 1000, 7000)
                        ),
                    new Flash(0xFFFFFF, .1, 5),
                    new Shoot(20, 1, projectileIndex: 0, coolDown: 400),
                    new TossObject("Arena Mushroom", 10, coolDown: 7000),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 500),
                    new Shoot(20, 6, shootAngle: 72, projectileIndex: 1, fixedAngle: 0, coolDownOffset: 1000)
                    )
                ),
            new Threshold(1,
                new ItemLoot("Potion of Speed", 1)
                ),
            new Threshold(5,
                new ItemLoot("Potion of Wisdom", 0.1)
                ),
            new Threshold(0.01,
                new TierLoot(6, ItemType.Weapon, 0.3),
                new TierLoot(6, ItemType.Weapon, 0.3),
                new TierLoot(7, ItemType.Weapon, 0.25),
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(9, ItemType.Weapon, 0.15),
                new TierLoot(6, ItemType.Armor, 0.3),
                new TierLoot(7, ItemType.Armor, 0.25),
                new TierLoot(8, ItemType.Armor, 0.2),
                new TierLoot(9, ItemType.Armor, 0.15),
                new TierLoot(3, ItemType.Ability, 0.2),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(5, ItemType.Ability, 0.10),
                new TierLoot(3, ItemType.Ring, 0.2),
                new TierLoot(4, ItemType.Ring, 0.15),
                //new ItemLoot("Amulet of Resurrection", 0.01),
                new ItemLoot("Wine Cellar Incantation", 0.01),
                new ItemLoot("Skeleton Warrior Skin", 0.01)
                )
            )
        .Init("Arena Mushroom",
            new State(
                new State("Texture1",
                    new SetAltTexture(0),
                    new TimedTransition(1000, "Texture2")
                    ),
                new State("Texture2",
                    new SetAltTexture(1),
                    new TimedTransition(1000, "Texture3")
                    ),
                new State("Texture3",
                    new SetAltTexture(2),
                    new TimedTransition(500, "Boom")
                    ),
                new State("Boom",
                    new Shoot(100, 6, shootAngle: 72, projectileIndex: 0, fixedAngle: 0),
                    new Suicide()
                    )
                )
            )
        .Init("Area 2 Controller",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
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
                    new Taunt(true, "Congratulations on making it past the first area! This area will not be so easy!"),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new Taunt(true, "Say,'READY' when you are ready to face your opponeants.", "Prepare yourself...Say, 'READY' when you wish the battle to begin!"),
                    new TimedTransition(0, "5")
                    ),
                //repeat
                new State("5",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6")
                    ),
                new State("6",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(4),
                    new TimedTransition(200, "6_1")
                    ),
                new State("6_1",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6_2")
                    ),
                new State("6_2",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(4),
                    new TimedTransition(200, "6_3")
                    ),
                new State("6_3",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6_4")
                    ),
                new State("6_4",
                    new SetAltTexture(1),
                    new PlayerTextTransition("7", "ready")
                    ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(100, "Arena South Gate Spawner", "Stage 6"),
                    new Order(100, "Arena West Gate Spawner", "Stage 6"),
                    new Order(100, "Arena East Gate Spawner", "Stage 6"),
                    new Order(100, "Arena North Gate Spawner", "Stage 6"),
                    new EntityExistsTransition("Arena Ghost 2", 999, "Check 1")
                    ),
                new State("Check 1",
                    new EntitiesNotExistsTransition(100, "8", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "11")
                    ),
                new State("11",
                    new SetAltTexture(3),
                    new TimedTransition(200, "12")
                    ),
                new State("12",
                    new SetAltTexture(4),
                    new TimedTransition(200, "13")
                    ),
                new State("13",
                    new SetAltTexture(3),
                    new TimedTransition(200, "14")
                    ),
                new State("14",
                    new SetAltTexture(4),
                    new TimedTransition(200, "15")
                    ),
                new State("15",
                    new SetAltTexture(3),
                    new TimedTransition(200, "16")
                    ),
                new State("16",
                    new SetAltTexture(4),
                    new TimedTransition(200, "17")
                    ),
                new State("17",
                    new SetAltTexture(3),
                    new TimedTransition(200, "18")
                    ),
                new State("18",
                    new SetAltTexture(4),
                    new TimedTransition(200, "19")
                    ),
                new State("19",
                    new SetAltTexture(3),
                    new TimedTransition(200, "20")
                    ),
                new State("20",
                    new SetAltTexture(4),
                    new TimedTransition(200, "21")
                    ),
                new State("21",
                    new SetAltTexture(3),
                    new TimedTransition(200, "22")
                    ),
                new State("22",
                    new SetAltTexture(4),
                    new TimedTransition(200, "23")
                    ),
                new State("23",
                    new SetAltTexture(3),
                    new TimedTransition(200, "24")
                    ),
                new State("24",
                    new SetAltTexture(4),
                    new TimedTransition(100, "25")
                    ),
                new State("25",
                    new SetAltTexture(1),
                    new TimedTransition(100, "26")
                    ),
                new State("26",
                    new SetAltTexture(2),
                    new TimedTransition(100, "27")
                    ),
                new State("27",
                    new SetAltTexture(0),
                    new Order(100, "Arena South Gate Spawner", "Stage 7"),
                    new Order(100, "Arena West Gate Spawner", "Stage 7"),
                    new Order(100, "Arena East Gate Spawner", "Stage 7"),
                    new Order(100, "Arena North Gate Spawner", "Stage 7"),
                    new EntityExistsTransition("Arena Ghost 2", 999, "Check 2")
                    ),
                new State("Check 2",
                    new EntitiesNotExistsTransition(100, "28", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "31")
                    ),
                new State("31",
                    new SetAltTexture(3),
                    new TimedTransition(200, "32")
                    ),
                new State("32",
                    new SetAltTexture(4),
                    new TimedTransition(200, "33")
                    ),
                new State("33",
                    new SetAltTexture(3),
                    new TimedTransition(200, "34")
                    ),
                new State("34",
                    new SetAltTexture(4),
                    new TimedTransition(200, "35")
                    ),
                new State("35",
                    new SetAltTexture(3),
                    new TimedTransition(200, "36")
                    ),
                new State("36",
                    new SetAltTexture(4),
                    new TimedTransition(200, "37")
                    ),
                new State("37",
                    new SetAltTexture(3),
                    new TimedTransition(200, "38")
                    ),
                new State("38",
                    new SetAltTexture(4),
                    new TimedTransition(200, "39")
                    ),
                new State("39",
                    new SetAltTexture(3),
                    new TimedTransition(200, "40")
                    ),
                new State("40",
                    new SetAltTexture(4),
                    new TimedTransition(200, "41")
                    ),
                new State("41",
                    new SetAltTexture(3),
                    new TimedTransition(200, "42")
                    ),
                new State("42",
                    new SetAltTexture(4),
                    new TimedTransition(200, "43")
                    ),
                new State("43",
                    new SetAltTexture(3),
                    new TimedTransition(200, "44")
                    ),
                new State("44",
                    new SetAltTexture(4),
                    new TimedTransition(100, "45")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 8"),
                    new Order(100, "Arena West Gate Spawner", "Stage 8"),
                    new Order(100, "Arena East Gate Spawner", "Stage 8"),
                    new Order(100, "Arena North Gate Spawner", "Stage 8"),
                    new EntityExistsTransition("Arena Ghost 2", 999, "Check 3")
                    ),
                new State("Check 3",
                    new EntitiesNotExistsTransition(100, "48", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
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
                    new TimedTransition(200, "52")
                    ),
                new State("52",
                    new SetAltTexture(4),
                    new TimedTransition(200, "53")
                    ),
                new State("53",
                    new SetAltTexture(3),
                    new TimedTransition(200, "54")
                    ),
                new State("54",
                    new SetAltTexture(4),
                    new TimedTransition(200, "55")
                    ),
                new State("55",
                    new SetAltTexture(3),
                    new TimedTransition(200, "56")
                    ),
                new State("56",
                    new SetAltTexture(4),
                    new TimedTransition(200, "57")
                    ),
                new State("57",
                    new SetAltTexture(3),
                    new TimedTransition(200, "58")
                    ),
                new State("58",
                    new SetAltTexture(4),
                    new TimedTransition(200, "59")
                    ),
                new State("59",
                    new SetAltTexture(3),
                    new TimedTransition(200, "60")
                    ),
                new State("60",
                    new SetAltTexture(4),
                    new TimedTransition(200, "61")
                    ),
                new State("61",
                    new SetAltTexture(3),
                    new TimedTransition(200, "62")
                    ),
                new State("62",
                    new SetAltTexture(4),
                    new TimedTransition(200, "63")
                    ),
                new State("63",
                    new SetAltTexture(3),
                    new TimedTransition(200, "64")
                    ),
                new State("64",
                    new SetAltTexture(4),
                    new TimedTransition(100, "65")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 9"),
                    new Order(100, "Arena West Gate Spawner", "Stage 9"),
                    new Order(100, "Arena East Gate Spawner", "Stage 9"),
                    new Order(100, "Arena North Gate Spawner", "Stage 9"),
                    new EntityExistsTransition("Arena Ghost 2", 999, "Check 4")
                    ),
                new State("Check 4",
                    new EntitiesNotExistsTransition(100, "68", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "71")
                    ),
                new State("71",
                    new SetAltTexture(3),
                    new TimedTransition(200, "72")
                    ),
                new State("72",
                    new SetAltTexture(4),
                    new TimedTransition(200, "73")
                    ),
                new State("73",
                    new SetAltTexture(3),
                    new TimedTransition(200, "74")
                    ),
                new State("74",
                    new SetAltTexture(4),
                    new TimedTransition(200, "75")
                    ),
                new State("75",
                    new SetAltTexture(3),
                    new TimedTransition(200, "76")
                    ),
                new State("76",
                    new SetAltTexture(4),
                    new TimedTransition(200, "77")
                    ),
                new State("77",
                    new SetAltTexture(3),
                    new TimedTransition(200, "78")
                    ),
                new State("78",
                    new SetAltTexture(4),
                    new TimedTransition(200, "79")
                    ),
                new State("79",
                    new SetAltTexture(3),
                    new TimedTransition(200, "80")
                    ),
                new State("80",
                    new SetAltTexture(4),
                    new TimedTransition(200, "81")
                    ),
                new State("81",
                    new SetAltTexture(3),
                    new TimedTransition(200, "82")
                    ),
                new State("82",
                    new SetAltTexture(4),
                    new TimedTransition(200, "83")
                    ),
                new State("83",
                    new SetAltTexture(3),
                    new TimedTransition(200, "84")
                    ),
                new State("84",
                    new SetAltTexture(4),
                    new TimedTransition(100, "85")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 10"),
                    new Order(100, "Arena West Gate Spawner", "Stage 10"),
                    new Order(100, "Arena East Gate Spawner", "Stage 10"),
                    new Order(100, "Arena North Gate Spawner", "Stage 10"),
                    new EntityExistsTransition("Arena Possessed Girl", 999, "Check 5")
                    ),
                new State("Check 5",
                    new EntitiesNotExistsTransition(100, "88", "Arena Ghost Bride", "Arena Possessed Girl")
                    ),
                new State("88",
                    new TransformOnDeath("Haunted Cemetery Graves Portal"),
                    new Suicide()
                    )
                )
            )
        .Init("Arena Ghost 1",
            new State(
                new Prioritize(
                    new Wander(0.4),
                    new Follow(0.3, 10, 0, 2000, 4000)
                    ),
                new Shoot(10, 1, projectileIndex: 0, coolDown: 330)
                )
            )
        .Init("Arena Ghost 2",
            new State(
                new State("1",
                    new Prioritize(
                        new Wander(0.5),
                        new Charge(3, 10, 2000)
                        ),
                    new SetAltTexture(0),
                    new Shoot(10, 3, shootAngle: 30, projectileIndex: 0, coolDown: 1000),
                    new TimedTransition(4000, "2")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new Wander(0.4),
                    new TimedTransition(2000, "1")
                    )
                )
            )
        .Init("Arena Possessed Girl",
            new State(
                new Prioritize(
                    new Wander(0.3),
                    new Follow(0.5, 10, 1)
                    ),
                new State("1",
                    new Shoot(10, 8, projectileIndex: 0, coolDown: 400)
                    )
                )
            )
        .Init("Arena Ghost Bride",
            new State(
                new State("1",
                    new Wander(0.4),
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 300),
                    new Shoot(10, 4, projectileIndex: 1, coolDown: 400),
                    new HpLessTransition(0.75, "2")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new Order(100, "Arena Statue Left", "2"),
                    new TimedTransition(100, "3")
                    ),
                new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new EntityNotExistsTransition("Arena Statue Left", 100, "4")
                    ),
                new State("4",
                    new SetAltTexture(0),
                    new Wander(0.4),
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 300),
                    new Shoot(10, 4, projectileIndex: 1, coolDown: 400),
                    new HpLessTransition(0.5, "5")
                    ),
                new State("5",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new Order(100, "Arena Statue Right", "2"),
                    new TimedTransition(100, "6")
                    ),
                new State("6",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new EntityNotExistsTransition("Arena Statue Right", 100, "7")
                    ),
                new State("7",
                    new SetAltTexture(0),
                    new Wander(0.4),
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 300),
                    new Shoot(10, 4, projectileIndex: 1, coolDown: 400)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Speed", 1)
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Wisdom", 0.1)
                ),
            new Threshold(0.01,
                new TierLoot(7, ItemType.Weapon, 0.25),
                new TierLoot(7, ItemType.Weapon, 0.25),
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(9, ItemType.Weapon, 0.15),
                new TierLoot(9, ItemType.Weapon, 0.15),
                new TierLoot(10, ItemType.Weapon, 0.1),
                new TierLoot(10, ItemType.Armor, 0.1),
                new TierLoot(8, ItemType.Armor, 0.2),
                new TierLoot(9, ItemType.Armor, 0.15),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(5, ItemType.Ability, 0.10),
                new TierLoot(3, ItemType.Ring, 0.2),
                new TierLoot(4, ItemType.Ring, 0.15),
                //new ItemLoot("Amulet of Resurrection", 0.01),
                new ItemLoot("Wine Cellar Incantation", 0.01),
                new ItemLoot("Ghost Huntress Skin", 0.01)
                )
            )
        .Init("Arena Statue Left",
            new State(
                new TransferDamageOnDeath("Arena Ghost Bride"),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1)
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(1000, "3")
                    ),
                new State("3",
                    new SetAltTexture(0),
                    new Prioritize(
                        new Follow(0.7, 10, 1, 4000, 1000)
                        ),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 200),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 30, coolDownOffset: 400),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 60, coolDownOffset: 600),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 90, coolDownOffset: 800),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 120, coolDownOffset: 1000),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 150, coolDownOffset: 1200),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 180, coolDownOffset: 1400),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 210, coolDownOffset: 1600),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 240, coolDownOffset: 1800),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 260, coolDownOffset: 2000),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 290, coolDownOffset: 2200),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 320, coolDownOffset: 2400)
                    )
                )
            )
        .Init("Arena Statue Right",
            new State(
                new TransferDamageOnDeath("Arena Ghost Bride"),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1)
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(1000, "3")
                    ),
                new State("3",
                    new SetAltTexture(0),
                    new Prioritize(
                        new Follow(0.7, 10, 1, 4000, 1000)
                        ),
                    new Shoot(10, 1, projectileIndex: 1, coolDown: 500),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 0, coolDownOffset: 200),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 30, coolDownOffset: 400),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 60, coolDownOffset: 600),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 90, coolDownOffset: 800),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 120, coolDownOffset: 1000),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 150, coolDownOffset: 1200),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 180, coolDownOffset: 1400),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 210, coolDownOffset: 1600),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 240, coolDownOffset: 1800),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 260, coolDownOffset: 2000),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 290, coolDownOffset: 2200),
                    new Shoot(10, 1, shootAngle: 0, projectileIndex: 0, fixedAngle: 320, coolDownOffset: 2400)
                    )
                )
            )
        .Init("Area 3 Controller",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
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
                    new Taunt("Your prowess in battle is impressive... and most annoying. This round shall crush you."),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new Taunt(true, "Say,'READY' when you are ready to face your opponeants.", "Prepare yourself...Say, 'READY' when you wish the battle to begin!"),
                    new TimedTransition(0, "5")
                    ),
                //repeat
                new State("5",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6")
                    ),
                new State("6",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(4),
                    new TimedTransition(200, "6_1")
                    ),
                new State("6_1",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6_2")
                    ),
                new State("6_2",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(4),
                    new TimedTransition(200, "6_3")
                    ),
                new State("6_3",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(3),
                    new TimedTransition(200, "6_4")
                    ),
                new State("6_4",
                    new SetAltTexture(1),
                    new PlayerTextTransition("7", "ready")
                    ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(100, "Arena South Gate Spawner", "Stage 11"),
                    new Order(100, "Arena West Gate Spawner", "Stage 11"),
                    new Order(100, "Arena East Gate Spawner", "Stage 11"),
                    new Order(100, "Arena North Gate Spawner", "Stage 11"),
                    new EntityExistsTransition("Arena Risen Warrior", 999, "Check 1")
                    ),
                new State("Check 1",
                    new EntitiesNotExistsTransition(100, "8", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "11")
                    ),
                new State("11",
                    new SetAltTexture(3),
                    new TimedTransition(200, "12")
                    ),
                new State("12",
                    new SetAltTexture(4),
                    new TimedTransition(200, "13")
                    ),
                new State("13",
                    new SetAltTexture(3),
                    new TimedTransition(200, "14")
                    ),
                new State("14",
                    new SetAltTexture(4),
                    new TimedTransition(200, "15")
                    ),
                new State("15",
                    new SetAltTexture(3),
                    new TimedTransition(200, "16")
                    ),
                new State("16",
                    new SetAltTexture(4),
                    new TimedTransition(200, "17")
                    ),
                new State("17",
                    new SetAltTexture(3),
                    new TimedTransition(200, "18")
                    ),
                new State("18",
                    new SetAltTexture(4),
                    new TimedTransition(200, "19")
                    ),
                new State("19",
                    new SetAltTexture(3),
                    new TimedTransition(200, "20")
                    ),
                new State("20",
                    new SetAltTexture(4),
                    new TimedTransition(200, "21")
                    ),
                new State("21",
                    new SetAltTexture(3),
                    new TimedTransition(200, "22")
                    ),
                new State("22",
                    new SetAltTexture(4),
                    new TimedTransition(200, "23")
                    ),
                new State("23",
                    new SetAltTexture(3),
                    new TimedTransition(200, "24")
                    ),
                new State("24",
                    new SetAltTexture(4),
                    new TimedTransition(100, "25")
                    ),
                new State("25",
                    new SetAltTexture(1),
                    new TimedTransition(100, "26")
                    ),
                new State("26",
                    new SetAltTexture(2),
                    new TimedTransition(100, "27")
                    ),
                new State("27",
                    new SetAltTexture(0),
                    new Order(100, "Arena South Gate Spawner", "Stage 12"),
                    new Order(100, "Arena West Gate Spawner", "Stage 12"),
                    new Order(100, "Arena East Gate Spawner", "Stage 12"),
                    new Order(100, "Arena North Gate Spawner", "Stage 12"),
                    new EntityExistsTransition("Arena Risen Warrior", 999, "Check 2")
                    ),
                new State("Check 2",
                    new EntitiesNotExistsTransition(100, "28", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "31")
                    ),
                new State("31",
                    new SetAltTexture(3),
                    new TimedTransition(200, "32")
                    ),
                new State("32",
                    new SetAltTexture(4),
                    new TimedTransition(200, "33")
                    ),
                new State("33",
                    new SetAltTexture(3),
                    new TimedTransition(200, "34")
                    ),
                new State("34",
                    new SetAltTexture(4),
                    new TimedTransition(200, "35")
                    ),
                new State("35",
                    new SetAltTexture(3),
                    new TimedTransition(200, "36")
                    ),
                new State("36",
                    new SetAltTexture(4),
                    new TimedTransition(200, "37")
                    ),
                new State("37",
                    new SetAltTexture(3),
                    new TimedTransition(200, "38")
                    ),
                new State("38",
                    new SetAltTexture(4),
                    new TimedTransition(200, "39")
                    ),
                new State("39",
                    new SetAltTexture(3),
                    new TimedTransition(200, "40")
                    ),
                new State("40",
                    new SetAltTexture(4),
                    new TimedTransition(200, "41")
                    ),
                new State("41",
                    new SetAltTexture(3),
                    new TimedTransition(200, "42")
                    ),
                new State("42",
                    new SetAltTexture(4),
                    new TimedTransition(200, "43")
                    ),
                new State("43",
                    new SetAltTexture(3),
                    new TimedTransition(200, "44")
                    ),
                new State("44",
                    new SetAltTexture(4),
                    new TimedTransition(100, "45")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 13"),
                    new Order(100, "Arena West Gate Spawner", "Stage 13"),
                    new Order(100, "Arena East Gate Spawner", "Stage 13"),
                    new Order(100, "Arena North Gate Spawner", "Stage 13"),
                    new EntityExistsTransition("Arena Risen Warrior", 999, "Check 3")
                    ),
                new State("Check 3",
                    new EntitiesNotExistsTransition(100, "48", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
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
                    new TimedTransition(200, "52")
                    ),
                new State("52",
                    new SetAltTexture(4),
                    new TimedTransition(200, "53")
                    ),
                new State("53",
                    new SetAltTexture(3),
                    new TimedTransition(200, "54")
                    ),
                new State("54",
                    new SetAltTexture(4),
                    new TimedTransition(200, "55")
                    ),
                new State("55",
                    new SetAltTexture(3),
                    new TimedTransition(200, "56")
                    ),
                new State("56",
                    new SetAltTexture(4),
                    new TimedTransition(200, "57")
                    ),
                new State("57",
                    new SetAltTexture(3),
                    new TimedTransition(200, "58")
                    ),
                new State("58",
                    new SetAltTexture(4),
                    new TimedTransition(200, "59")
                    ),
                new State("59",
                    new SetAltTexture(3),
                    new TimedTransition(200, "60")
                    ),
                new State("60",
                    new SetAltTexture(4),
                    new TimedTransition(200, "61")
                    ),
                new State("61",
                    new SetAltTexture(3),
                    new TimedTransition(200, "62")
                    ),
                new State("62",
                    new SetAltTexture(4),
                    new TimedTransition(200, "63")
                    ),
                new State("63",
                    new SetAltTexture(3),
                    new TimedTransition(200, "64")
                    ),
                new State("64",
                    new SetAltTexture(4),
                    new TimedTransition(100, "65")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 14"),
                    new Order(100, "Arena West Gate Spawner", "Stage 14"),
                    new Order(100, "Arena East Gate Spawner", "Stage 14"),
                    new Order(100, "Arena North Gate Spawner", "Stage 14"),
                    new EntityExistsTransition("Arena Risen Warrior", 999, "Check 4")
                    ),
                new State("Check 4",
                    new EntitiesNotExistsTransition(100, "68", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
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
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "71")
                    ),
                new State("71",
                    new SetAltTexture(3),
                    new TimedTransition(200, "72")
                    ),
                new State("72",
                    new SetAltTexture(4),
                    new TimedTransition(200, "73")
                    ),
                new State("73",
                    new SetAltTexture(3),
                    new TimedTransition(200, "74")
                    ),
                new State("74",
                    new SetAltTexture(4),
                    new TimedTransition(200, "75")
                    ),
                new State("75",
                    new SetAltTexture(3),
                    new TimedTransition(200, "76")
                    ),
                new State("76",
                    new SetAltTexture(4),
                    new TimedTransition(200, "77")
                    ),
                new State("77",
                    new SetAltTexture(3),
                    new TimedTransition(200, "78")
                    ),
                new State("78",
                    new SetAltTexture(4),
                    new TimedTransition(200, "79")
                    ),
                new State("79",
                    new SetAltTexture(3),
                    new TimedTransition(200, "80")
                    ),
                new State("80",
                    new SetAltTexture(4),
                    new TimedTransition(200, "81")
                    ),
                new State("81",
                    new SetAltTexture(3),
                    new TimedTransition(200, "82")
                    ),
                new State("82",
                    new SetAltTexture(4),
                    new TimedTransition(200, "83")
                    ),
                new State("83",
                    new SetAltTexture(3),
                    new TimedTransition(200, "84")
                    ),
                new State("84",
                    new SetAltTexture(4),
                    new TimedTransition(100, "85")
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
                    new Order(100, "Arena South Gate Spawner", "Stage 15"),
                    new Order(100, "Arena West Gate Spawner", "Stage 15"),
                    new Order(100, "Arena East Gate Spawner", "Stage 15"),
                    new Order(100, "Arena North Gate Spawner", "Stage 15"),
                    new EntityExistsTransition("Arena Grave Caretaker", 999, "Check 5")
                    ),
                new State("Check 5",
                    new EntitiesNotExistsTransition(100, "88", "Arena Blue Flame", "Arena Grave Caretaker")
                    ),
                new State("88",
                    new TransformOnDeath("Haunted Cemetery Final Rest Portal"),
                    new Suicide()
                    )
                )
            )
        .Init("Arena Risen Brawler",
            new State(
                new Prioritize(
                    new Wander(0.2),
                    new Follow(0.7, 10, 2)
                    ),
                new Shoot(10, 3, shootAngle: 10, projectileIndex: 0, coolDown: 650)
                )
            )
        .Init("Arena Risen Warrior",
            new State(
                new Prioritize(
                    new Follow(0.7, 10, 2)
                    ),
                new Shoot(10, 1, projectileIndex: 0, coolDown: 330)
                )
            )
        .Init("Arena Risen Mage",
            new State(
                new Prioritize(
                    new StayBack(0.9, 7)
                    ),
                new HealSelf(5000, 100),
                new Shoot(10, 1, projectileIndex: 0, coolDown: 330)
                )
            )
        .Init("Arena Risen Archer",
            new State(
                new Prioritize(
                    new StayBack(0.9, 7)
                    ),
                new Shoot(10, 3, shootAngle: 45, projectileIndex: 0, coolDown: 500),
                new Shoot(10, 1, projectileIndex: 1, coolDown: 1100)
                )
            )
        .Init("Arena Blue Flame",
            new State(
                new State("1",
                    new Prioritize(
                        new Orbit(1, 2, 10, "Arena Grave Caretaker")
                        ),
                    new HpLessTransition(0.5, "2")
                    ),
                new State("2",
                    new Prioritize(
                        new Charge(4, 10, 1500)
                        ),
                    new PlayerWithinTransition(3, "3")
                    ),
                new State("3",
                    new Shoot(10, 10, shootAngle: 32, projectileIndex: 0, fixedAngle: 0),
                    new Suicide()
                    )
                )
            )
        .Init("Arena Grave Caretaker",
            new State(
                new Spawn("Arena Blue Flame", maxChildren: 5),
                new Reproduce("Arena Blue Flame", 100, 5, coolDown: 500),
                new Shoot(10, 1, projectileIndex: 0, coolDown: 350),
                new Shoot(10, 2, shootAngle: 15, projectileIndex: 1, coolDown: 450),
                new State("1",
                    new StayBack(0.5, 6),
                    new TimedTransition(4000, "2")
                    ),
                new State("2",
                    new Follow(0.5, 10, 1),
                    new TimedTransition(4000, "1")
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Speed", 1)
                ),
            new Threshold(0.05,
                new ItemLoot("Potion of Wisdom", 0.1)
                ),
            new Threshold(0.01,
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(9, ItemType.Weapon, 0.15),
                new TierLoot(10, ItemType.Weapon, 0.1),
                new TierLoot(11, ItemType.Weapon, 0.08),
                new TierLoot(8, ItemType.Armor, 0.2),
                new TierLoot(9, ItemType.Armor, 0.15),
                new TierLoot(10, ItemType.Armor, 0.1),
                new TierLoot(11, ItemType.Armor, 0.08),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(5, ItemType.Ability, 0.10),
                new TierLoot(5, ItemType.Ability, 0.10),
                new TierLoot(3, ItemType.Ring, 0.2),
                new TierLoot(4, ItemType.Ring, 0.15),
                //new ItemLoot("Amulet of Resurrection", 0.01),
                new ItemLoot("Wine Cellar Incantation", 0.01),
                new ItemLoot("Etherite Dagger", 0.01),
                new ItemLoot("Ghastly Drape", 0.01),
                new ItemLoot("Mantle of Skuld", 0.01),
                new ItemLoot("Spectral Ring of Horrors", 0.01)
                )
            )
        .Init("Zombie Hulk",
            new State(
                new State("1",
                    new Prioritize(
                        new Wander(0.7)
                        ),
                    new Shoot(10, 3, shootAngle: 25, projectileIndex: 1, coolDown: 400),
                    new TimedTransition(3000, "2")
                    ),
                new State("2",
                    new Prioritize(
                        new Follow(0.9, 12, 1)
                        ),
                    new Shoot(10, 5, shootAngle: 72, projectileIndex: 0, coolDown: 300),
                    new TimedTransition(3000, "1")
                    )
                )
            )
        .Init("Classic Ghost",
            new State(
                new Wander(0.6),
                new Follow(0.6, 10, 1),
                new Shoot(10, 4, shootAngle: 15, projectileIndex: 0, coolDown: 400)
                )
            )
        .Init("Werewolf",
            new State(
                new Prioritize(
                    new Wander(0.3),
                    new StayBack(0.6, 10)
                    ),
                new Spawn("Mini Werewolf", maxChildren: 3),
                new Reproduce("Mini Werewolf", 8, 3, coolDown: 3000),
                new Shoot(10, 3, shootAngle: 15, projectileIndex: 0, coolDownOffset: 500),
                new Shoot(10, 5, shootAngle: 15, projectileIndex: 0, coolDownOffset: 1000)
                )
            )
        .Init("Mini Werewolf",
            new State(
                new Orbit(0.7, 2, 10, "Werewolf"),
                new Shoot(10, 1, projectileIndex: 0, coolDown: 300),
                new State("1",
                    new EntityNotExistsTransition("Mini Werewolf", 10, "2")
                    ),
                new State("2",
                    new Wander(0.4),
                    new EntityExistsTransition("Werewolf", 10, "1")
                    )
                )
            )
        .Init("Ghost of Skuld",
            new State(
                new SetAltTexture(11),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new DropPortalOnDeath("Realm Portal", 100),
                new State("Start",
                    new PlayerWithinTransition(4, "1")
                    ),
                new State("1",
                    new TimedTransition(0, "2")
                    ),
                new State("2",
                    new SetAltTexture(1),
                    new TimedTransition(100, "3")
                    ),
                new State("3",
                    new SetAltTexture(0),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new Taunt(true, "Say,'READY' when you are ready to face your opponeants.", "Prepare yourself...Say, 'READY' when you wish the battle to begin!"),
                    new TimedTransition(0, "5")
                    ),
                //repeat
                new State("5",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(12),
                    new TimedTransition(200, "6")
                    ),
                new State("6",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(13),
                    new TimedTransition(200, "6_1")
                    ),
                new State("6_1",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(12),
                    new TimedTransition(200, "6_2")
                    ),
                new State("6_2",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(13),
                    new TimedTransition(200, "6_3")
                    ),
                new State("6_3",
                    new PlayerTextTransition("7", "ready"),
                    new SetAltTexture(12),
                    new TimedTransition(200, "6_4")
                    ),
                new State("6_4",
                    new SetAltTexture(0),
                    new PlayerTextTransition("7", "ready")
                    ),
                new State("7",
                    new SetAltTexture(11),
                    new Order(100, "Arena Up Spawner", "Stage 1"),
                    new Order(100, "Arena Lf Spawner", "Stage 1"),
                    new Order(100, "Arena Dn Spawner", "Stage 1"),
                    new Order(100, "Arena Rt Spawner", "Stage 1"),
                    new EntityExistsTransition("Classic Ghost", 999, "Check 1")
                    ),
                new State("Check 1",
                    new EntitiesNotExistsTransition(100, "8", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                    ),
                new State("8",
                    new SetAltTexture(1),
                    new TimedTransition(0, "9")
                    ),
                new State("9",
                    new SetAltTexture(0),
                    new TimedTransition(2000, "10")
                    ),
                new State("10",
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "11")
                    ),
                new State("11",
                    new SetAltTexture(12),
                    new TimedTransition(200, "12")
                    ),
                new State("12",
                    new SetAltTexture(13),
                    new TimedTransition(200, "13")
                    ),
                new State("13",
                    new SetAltTexture(12),
                    new TimedTransition(200, "14")
                    ),
                new State("14",
                    new SetAltTexture(13),
                    new TimedTransition(200, "15")
                    ),
                new State("15",
                    new SetAltTexture(12),
                    new TimedTransition(200, "16")
                    ),
                new State("16",
                    new SetAltTexture(13),
                    new TimedTransition(200, "17")
                    ),
                new State("17",
                    new SetAltTexture(12),
                    new TimedTransition(200, "18")
                    ),
                new State("18",
                    new SetAltTexture(13),
                    new TimedTransition(200, "19")
                    ),
                new State("19",
                    new SetAltTexture(12),
                    new TimedTransition(200, "20")
                    ),
                new State("20",
                    new SetAltTexture(13),
                    new TimedTransition(200, "21")
                    ),
                new State("21",
                    new SetAltTexture(12),
                    new TimedTransition(200, "22")
                    ),
                new State("22",
                    new SetAltTexture(13),
                    new TimedTransition(200, "23")
                    ),
                new State("23",
                    new SetAltTexture(12),
                    new TimedTransition(200, "24")
                    ),
                new State("24",
                    new SetAltTexture(13),
                    new TimedTransition(100, "25")
                    ),
                new State("25",
                    new SetAltTexture(0),
                    new TimedTransition(100, "26")
                    ),
                new State("26",
                    new SetAltTexture(1),
                    new TimedTransition(100, "27")
                    ),
                new State("27",
                    new SetAltTexture(11),
                    new Order(100, "Arena Up Spawner", "Stage 2"),
                    new Order(100, "Arena Lf Spawner", "Stage 2"),
                    new Order(100, "Arena Dn Spawner", "Stage 2"),
                    new Order(100, "Arena Rt Spawner", "Stage 2"),
                    new EntityExistsTransition("Classic Ghost", 999, "Check 2")
                    ),
                new State("Check 2",
                    new EntitiesNotExistsTransition(100, "28", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                    ),
                new State("28",
                    new SetAltTexture(1),
                    new TimedTransition(0, "29")
                    ),
                new State("29",
                    new SetAltTexture(0),
                    new TimedTransition(2000, "30")
                    ),
                new State("30",
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "31")
                    ),
                new State("31",
                    new SetAltTexture(12),
                    new TimedTransition(200, "32")
                    ),
                new State("32",
                    new SetAltTexture(13),
                    new TimedTransition(200, "33")
                    ),
                new State("33",
                    new SetAltTexture(12),
                    new TimedTransition(200, "34")
                    ),
                new State("34",
                    new SetAltTexture(13),
                    new TimedTransition(200, "35")
                    ),
                new State("35",
                    new SetAltTexture(12),
                    new TimedTransition(200, "36")
                    ),
                new State("36",
                    new SetAltTexture(13),
                    new TimedTransition(200, "37")
                    ),
                new State("37",
                    new SetAltTexture(12),
                    new TimedTransition(200, "38")
                    ),
                new State("38",
                    new SetAltTexture(13),
                    new TimedTransition(200, "39")
                    ),
                new State("39",
                    new SetAltTexture(12),
                    new TimedTransition(200, "40")
                    ),
                new State("40",
                    new SetAltTexture(13),
                    new TimedTransition(200, "41")
                    ),
                new State("41",
                    new SetAltTexture(12),
                    new TimedTransition(200, "42")
                    ),
                new State("42",
                    new SetAltTexture(13),
                    new TimedTransition(200, "43")
                    ),
                new State("43",
                    new SetAltTexture(12),
                    new TimedTransition(200, "44")
                    ),
                new State("44",
                    new SetAltTexture(13),
                    new TimedTransition(100, "45")
                    ),
                new State("45",
                    new SetAltTexture(0),
                    new TimedTransition(100, "46")
                    ),
                new State("46",
                    new SetAltTexture(1),
                    new TimedTransition(100, "47")
                    ),
                new State("47",
                    new SetAltTexture(11),
                    new Order(100, "Arena Up Spawner", "Stage 3"),
                    new Order(100, "Arena Lf Spawner", "Stage 3"),
                    new Order(100, "Arena Dn Spawner", "Stage 3"),
                    new Order(100, "Arena Rt Spawner", "Stage 3"),
                    new EntityExistsTransition("Classic Ghost", 999, "Check 3")
                    ),
                new State("Check 3",
                    new EntitiesNotExistsTransition(100, "48", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                    ),
                new State("48",
                    new SetAltTexture(1),
                    new TimedTransition(0, "49")
                    ),
                new State("49",
                    new SetAltTexture(0),
                    new TimedTransition(2000, "50")
                    ),
                new State("50",
                    new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                    new TimedTransition(0, "51")
                    ),
                new State("51",
                    new SetAltTexture(12),
                    new TimedTransition(200, "52")
                    ),
                new State("52",
                    new SetAltTexture(13),
                    new TimedTransition(200, "53")
                    ),
                new State("53",
                    new SetAltTexture(12),
                    new TimedTransition(200, "54")
                    ),
                new State("54",
                    new SetAltTexture(13),
                    new TimedTransition(200, "55")
                    ),
                new State("55",
                    new SetAltTexture(12),
                    new TimedTransition(200, "56")
                    ),
                new State("56",
                    new SetAltTexture(13),
                    new TimedTransition(200, "57")
                    ),
                new State("57",
                    new SetAltTexture(12),
                    new TimedTransition(200, "58")
                    ),
                new State("58",
                    new SetAltTexture(13),
                    new TimedTransition(200, "59")
                    ),
                new State("59",
                    new SetAltTexture(12),
                    new TimedTransition(200, "60")
                    ),
                new State("60",
                    new SetAltTexture(13),
                    new TimedTransition(200, "61")
                    ),
                new State("61",
                    new SetAltTexture(12),
                    new TimedTransition(200, "62")
                    ),
                new State("62",
                    new SetAltTexture(13),
                    new TimedTransition(200, "63")
                    ),
                new State("63",
                    new SetAltTexture(12),
                    new TimedTransition(200, "64")
                    ),
                new State("64",
                    new SetAltTexture(13),
                    new TimedTransition(100, "65")
                    ),
                new State("65",
                    new SetAltTexture(0),
                    new TimedTransition(100, "66")
                    ),
                new State("66",
                    new SetAltTexture(1),
                    new TimedTransition(100, "67")
                    ),
                new State("67",
                    new SetAltTexture(11),
                    new Order(100, "Arena Up Spawner", "Stage 4"),
                    new Order(100, "Arena Lf Spawner", "Stage 4"),
                    new Order(100, "Arena Dn Spawner", "Stage 4"),
                    new Order(100, "Arena Rt Spawner", "Stage 4"),
                    new EntityExistsTransition("Classic Ghost", 999, "Check 4")
                    ),
                new State("Check 4",
                    new EntitiesNotExistsTransition(100, "68", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                    ),
                new State("68",
                    new SetAltTexture(1),
                    new TimedTransition(0, "69")
                    ),
                new State("69",
                    new SetAltTexture(0),
                    new TimedTransition(2000, "70")
                    ),
                new State("70",
                    new Taunt(true, "Congratulations on your victory, warrior. Your reward shall be..."),
                    new TimedTransition(0, "71")
                    ),
                new State("71",
                    new SetAltTexture(12),
                    new TimedTransition(200, "72")
                    ),
                new State("72",
                    new SetAltTexture(13),
                    new TimedTransition(200, "73")
                    ),
                new State("73",
                    new SetAltTexture(12),
                    new TimedTransition(200, "74")
                    ),
                new State("74",
                    new SetAltTexture(13),
                    new TimedTransition(200, "75")
                    ),
                new State("75",
                    new SetAltTexture(12),
                    new TimedTransition(200, "76")
                    ),
                new State("76",
                    new SetAltTexture(13),
                    new TimedTransition(200, "77")
                    ),
                new State("77",
                    new SetAltTexture(12),
                    new TimedTransition(200, "78")
                    ),
                new State("78",
                    new SetAltTexture(13),
                    new TimedTransition(200, "79")
                    ),
                new State("79",
                    new SetAltTexture(12),
                    new TimedTransition(200, "80")
                    ),
                new State("80",
                    new SetAltTexture(13),
                    new TimedTransition(200, "81")
                    ),
                new State("81",
                    new SetAltTexture(12),
                    new TimedTransition(200, "82")
                    ),
                new State("82",
                    new SetAltTexture(13),
                    new TimedTransition(200, "83")
                    ),
                new State("83",
                    new SetAltTexture(12),
                    new TimedTransition(200, "84")
                    ),
                new State("84",
                    new SetAltTexture(13),
                    new TimedTransition(100, "85")
                    ),
                new State("85",
                    new Taunt("A SWIFT DEATH!!!"),
                    new SetAltTexture(0),
                    new TimedTransition(200, "86")
                    ),
                new State("86",
                    new SetAltTexture(2),
                    new TimedTransition(200, "87")
                    ),
                new State("87",
                    new SetAltTexture(3),
                    new TimedTransition(200, "88")
                    ),
                new State("88",
                    new SetAltTexture(2),
                    new TimedTransition(200, "89")
                    ),
                new State("89",
                    new SetAltTexture(3),
                    new TimedTransition(200, "90")
                    ),
                new State("90",
                    new SetAltTexture(2),
                    new Shoot(10, 36, projectileIndex: 1, fixedAngle: 0, angleOffset: fixedAngle_RingAttack2),
                    new TimedTransition(0, "91")
                    ),
                new State("91",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TimedTransition(0, "92")
                    ),
                new State("92",
                    new SetAltTexture(0),
                    new Order(100, "Halloween Zombie Spawner", "1"),
                    new Shoot(20, 4, shootAngle: 15, projectileIndex: 0, coolDown: 500),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 0, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 90, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 180, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 270, coolDown: 5000),
                    new HpLessTransition(0.5, "93")
                    ),
                new State("93",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Spawn("Flying Flame Skull", maxChildren: 2),
                    new Shoot(20, 4, shootAngle: 15, projectileIndex: 0, coolDown: 500),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 0, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 90, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 180, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 270, coolDown: 5000),
                    new TimedTransition(5000, "94")
                    ),
                new State("94",
                    new Shoot(20, 4, shootAngle: 15, projectileIndex: 0, coolDown: 500),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 0, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 90, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 180, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 270, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 45, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 135, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 225, coolDown: 5000),
                    new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 315, coolDown: 5000)
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Vitality", 1)
                ),
            new Threshold(0.05,
                new ItemLoot("Potion of Wisdom", 0.1)
                ),
            new Threshold(0.01,
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(9, ItemType.Weapon, 0.15),
                new TierLoot(10, ItemType.Weapon, 0.1),
                new TierLoot(11, ItemType.Weapon, 0.08),
                new TierLoot(8, ItemType.Armor, 0.2),
                new TierLoot(8, ItemType.Armor, 0.2),
                new TierLoot(9, ItemType.Armor, 0.15),
                new TierLoot(10, ItemType.Armor, 0.1),
                new TierLoot(11, ItemType.Armor, 0.08),
                new TierLoot(3, ItemType.Ability, 0.2),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(4, ItemType.Ability, 0.15),
                new TierLoot(5, ItemType.Ability, 0.10),
                new TierLoot(4, ItemType.Ring, 0.15),
                new TierLoot(5, ItemType.Ring, 0.10),
                //new ItemLoot("Amulet of Resurrection", 0.01),
                //new ItemLoot("Tamed Werewolf Cub Egg", 0.03),
                new ItemLoot("Wine Cellar Incantation", 0.01),
                new ItemLoot("Plague Poison", 0.01),
                new ItemLoot("Resurrected Warrior's Armor", 0.01)
                )
            )
        .Init("Halloween Zombie Spawner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                ),
                new State("1",
                    new Spawn("Zombie Rise", maxChildren: 1),
                    new Reproduce("Zombie Rise", 1, 1, coolDown: 100),
                    new EntityNotExistsTransition("Ghost of Skuld", 100, "2")
                    ),
                new State("2",
                    new Suicide()
                    )
                )
            )
        .Init("Zombie Rise",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new TransformOnDeath("Blue Zombie"),
                new State("1",
                    new SetAltTexture(1),
                    new TimedTransition(100, "2")
                    ),
                new State("2",
                    new SetAltTexture(2),
                    new TimedTransition(100, "3")
                    ),
                new State("3",
                    new SetAltTexture(3),
                    new TimedTransition(100, "4")
                    ),
                new State("4",
                    new SetAltTexture(4),
                    new TimedTransition(100, "5")
                    ),
                new State("5",
                    new SetAltTexture(5),
                    new TimedTransition(100, "6")
                    ),
                new State("6",
                    new SetAltTexture(6),
                    new TimedTransition(100, "7")
                    ),
                new State("7",
                    new SetAltTexture(7),
                    new TimedTransition(100, "8")
                    ),
                new State("8",
                    new SetAltTexture(8),
                    new TimedTransition(100, "9")
                    ),
                new State("9",
                    new SetAltTexture(9),
                    new TimedTransition(100, "10")
                    ),
                new State("10",
                    new SetAltTexture(10),
                    new TimedTransition(100, "11")
                    ),
                new State("11",
                    new SetAltTexture(11),
                    new TimedTransition(100, "12")
                    ),
                new State("12",
                    new SetAltTexture(12),
                    new TimedTransition(100, "13")
                    ),
                new State("13",
                    new SetAltTexture(13),
                    new TimedTransition(100, "14")
                    ),
                new State("14",
                    new Suicide()
                    )
                )
            )
        .Init("Blue Zombie",
            new State(
                new Follow(0.03, 100, 1),
                new State("1",
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 500),
                    new EntityNotExistsTransition("Ghost of Skuld", 100, "2")
                    ),
                new State("2",
                    new Suicide()
                    )
                )
            )
        .Init("Flying Flame Skull",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Orbit(1, 5, 10, target: "Ghost of Skuld"),
                new State("1",
                    new Shoot(100, 10, shootAngle: 36, projectileIndex: 0, coolDown: 500),
                    new EntityNotExistsTransition("Ghost of Skuld", 100, "2")
                    ),
                new State("2",
                    new Suicide()
                    )
                )
            );

    }
}
