using TKR.Shared.resources;
using TKR.WorldServer.logic.behaviors;
using TKR.WorldServer.logic.loot;
using TKR.WorldServer.logic.transitions;

namespace TKR.WorldServer.logic
{
    partial class BehaviorDb
    {
        private class Fiery_Succubus
        {
            public static int fire_shard = 0;
            public static int fire_enchanted_bullet = 1;
        }

        private class Fiery_Twin_Succubus
        {
            public static int fire_shard = 0;
            public static int fire_enchanted_bullet = 1;
            public static int vulcanum = 2;
        }

        private _ EasyBounty = () => Behav()
        .Init("Easy Bounty Wingus the Succubus Queen",
            new State(
                new State("Pause",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new PlayerWithinTransition(3, "Start")
                    ),
                new State("Start",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, duration: 3000),
                    new Taunt("Who enters my domain... and awakened me?"),
                    new ScaleHP2(15, range: 30),
                    new TimedTransition(3000, "Fight")
                    ),
                new State("Fight",
                    new Taunt("I do not know of whom you are... but I will make sure you suffer for waking me up!"),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored), // ok
                    new Grenade(3, 120, 15, coolDown: 400),
                    new HpLessTransition(0.95, "Fight2")
                    ),
                new State("Fight2",
                    new Prioritize(
                        new Chase()
                        ),
                    new Taunt("Hmm... humans are so frail... be careful sweety ;D."),
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored, false, 0), // ok
                    new HealSelf(amount: 20000, coolDown: 2500),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 130, coolDownOffset: 800, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 140, coolDownOffset: 1000, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 150, coolDownOffset: 1200, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                    new Shoot(10, count: 8, coolDown: 4000, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 100, coolDownOffset: 1600, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 90, coolDownOffset: 1800, shootAngle: 90),
                    new Shoot(10, count: 4, coolDown: 4000, fixedAngle: 90, coolDownOffset: 2000, shootAngle: 22.5),
                    new HpLessTransition(.85, "Wait")
                    ),
                new State("Wait",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, duration: 7000), // ok
                    new Flash(0xff00ff00, 0.1, 7000),
                    new ChangeSize(5, 300),
                    new Chase(7, range: 0.5),
                    new Taunt("Hold up... I don't even know why I am toying with you.  Let me end this."),
                    new Shoot(10, count: 3, shootAngle: 20, predictive: 1, coolDown: 500),
                    new TimedTransition(7000, "Nightmare")
                    ),
                new State("Nightmare",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new Taunt("Consider me your worst nightmare!"),
                    new Flash(0xff00ff00, 0.2, 30),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 0, angleOffset: 0, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 1, angleOffset: 5 * 1, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 2, angleOffset: 5 * 2, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 3, angleOffset: 5 * 3, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 4, angleOffset: 5 * 4, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 5, angleOffset: 5 * 5, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 6, angleOffset: 5 * 6, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 7, angleOffset: 5 * 7, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 8, angleOffset: 5 * 8, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 9, angleOffset: 5 * 9, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 10, angleOffset: 5 * 10, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 11, angleOffset: 5 * 11, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 12, angleOffset: 5 * 12, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 13, angleOffset: 5 * 13, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 14, angleOffset: 5 * 14, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 15, angleOffset: 5 * 15, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 16, angleOffset: 5 * 16, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 17, angleOffset: 5 * 17, shootAngle: 60),
                    new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 18, angleOffset: 5 * 18, shootAngle: 60),
                    new HpLessTransition(0.6, "Nightmare2")
                    ),
                new State("Nightmare2",
                    new Taunt("Every possible battle option you have in mind will end in failure."),
                    new Shoot(30, projectileIndex: 1, count: 1, coolDown: 1000),
                    new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 0, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 400, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 800, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1200, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1600, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 0, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 400, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 800, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1200, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1600, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 2000, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 2400, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 2800, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3200, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3600, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 2000, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 2400, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 2800, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3200, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3600, angleOffset: 270, shootAngle: 9),
                    new HpLessTransition(0.45, "Waiting")
                    ),
                new State("Waiting",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt("My little ones, come and help Mommy <3 ! "),
                    new Spawn("Fiery Twin Succubus", maxChildren: 3, coolDown: 10000),
                    new Spawn("Icy Twin Succubus", maxChildren: 3, coolDown: 10000),
                    new Flash(0xFF0000, .1, 1000),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0),
                    new Shoot(10, count: 2, shootAngle: 20, predictive: 1, coolDown: 200),
                    new Shoot(50, projectileIndex: 3, count: 5, coolDown: 4000, coolDownOffset: 0, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 400, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 800, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 1200, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 1600, angleOffset: 0, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 5, coolDown: 4000, coolDownOffset: 0, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 400, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 800, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 1200, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 1600, angleOffset: 180, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 5, coolDown: 4000, coolDownOffset: 2000, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 2400, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 2800, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 3200, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 3600, angleOffset: 90, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 5, coolDown: 4000, coolDownOffset: 2000, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 2400, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 2800, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 3200, angleOffset: 270, shootAngle: 9),
                    new Shoot(50, projectileIndex: 3, count: 9, coolDown: 4000, coolDownOffset: 3600, angleOffset: 270, shootAngle: 9),
                    new HpLessTransition(0.15, "Spawn")
                    ),
                new State("Spawn",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt("ENOUGH IS ENOUGH. AYRIN, TAMIR, COME FORTH AND DO YOUR DUTIES."),
                    new Flash(0xffffff, 0.5, 2),
                    new ChangeSize(5, 150),
                    new TossObject("Ayrin", 3, 0, coolDown: 999999, coolDownOffset: 2000),
                    new TossObject("Tamir", 3, 180, coolDown: 999999, coolDownOffset: 2000),
                    new TimedTransition(5000, "CheckifDead")
                    ),
                new State("CheckifDead",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(2048, "Enraged", "Ayrin", "Tamir")
                    ),
                new State("Enraged",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored, true),
                    new Taunt("My Children... I WILL DRAG ALL OF YOU DOWN WITH ME."),
                    new Flash(0xff0000, 5, 999),
                    new ChangeSize(7, 300),
                    new Shoot(30, projectileIndex: 6, count: 2, coolDown: 1000),
                    new Shoot(5, 8, fixedAngle: 360 / 8, projectileIndex: 5, coolDown: 200),
                    new HpLessTransition(0.01, "Dead")
                    ),
                new State("Dead",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt("Finally, peace and rest at last."),
                    new Flash(0xFF0000, .1, 1000),
                    new TimedTransition(2000, "Suicide")
                    ),
                new State("Suicide",
                    new ConditionEffectBehavior(ConditionEffectIndex.StunImmune),
                    new Shoot(0, 20, fixedAngle: 360 / 20, projectileIndex: 6),
                    new Suicide()
                    )
                ),
                new Threshold(0.06,
                new ItemLoot("Wingus's Breastplate", 0.0015),
                new ItemLoot("Wand of Everlasting Love", 0.0015),
                new ItemLoot("Succubus's Tail", 0.002),
                new ItemLoot("Seductive Charm", 0.0015)
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Life", 0.5),
                new ItemLoot("Potion of Mana", 0.5)
                )
            )
        .Init("Ayrin",
            new State(
                new State("Awaken",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new PlayerWithinTransition(10, "Start")
                    ),
                new State("Start",
                    new Taunt("My mother... What have you done..."),
                    new ScaleHP2(20),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new Shoot(15, count: 2, shootAngle: 16, projectileIndex: 0, predictive: 0.4, coolDown: 800),
                    new Grenade(2.5, 80, 6),
                    new Shoot(20, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 1100),
                    new HpLessTransition(0.9, "Fuck this shit")
                    ),
                new State("Fuck this shit",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Taunt("I will avenge you..."),
                    new Shoot(25, count: 3, shootAngle: 20, projectileIndex: 0, predictive: 0.3, coolDown: 1000),
                    new Shoot(20, count: 6, shootAngle: 40, projectileIndex: 2, predictive: 0.4, coolDown: 900),
                    new Grenade(radius: 5, damage: 40, range: 20, coolDown: 400),
                    new HpLessTransition(0.77, "Ur actually gay")
                    ),
                new State("Ur actually gay",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored, false, 0),
                    new Taunt("It does not make sense... How do puny mortals like you have such power??"),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 90, coolDownOffset: 0, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 100, coolDownOffset: 200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 110, coolDownOffset: 400, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 120, coolDownOffset: 600, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 130, coolDownOffset: 800, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 140, coolDownOffset: 1000, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 150, coolDownOffset: 1200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 160, coolDownOffset: 1400, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 160, coolDownOffset: 0, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 150, coolDownOffset: 200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 140, coolDownOffset: 400, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 130, coolDownOffset: 600, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 120, coolDownOffset: 800, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 110, coolDownOffset: 1000, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 100, coolDownOffset: 1200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 90, coolDownOffset: 1400, coolDown: 10000),
                    new HpLessTransition(0.50, "K bye")
                    ),
                new State("K bye",
                    new Prioritize(
                        new Chase(speed: 5),
                        new Wander(0.4)
                        ),
                    new Taunt("How am I getting overpowered...I was made to be undefeatable..."),
                    new Shoot(30, count: 4, shootAngle: 25, projectileIndex: 3, predictive: 0.5, coolDown: 1000),
                    new Shoot(25, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 1200),
                    new Grenade(2.5, 100, 10),
                    new Shoot(50, count: 1, shootAngle: 1, projectileIndex: 1, predictive: 0.8, coolDown: 700),
                    new HpLessTransition(0.35, "SpawnMinion")
                    ),
                new State("SpawnMinion",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt("Come forth minions, protect me!"),
                    new Spawn("Icy Twin Succubus", maxChildren: 5, coolDown: 10000),
                    new TimedTransition(2000, "Checkifdead")
                    ),
                new State("Checkifdead",
                    new EntitiesNotExistsTransition(50, "Jeff", "Icy Twin Succubus")
                    ),
                new State("Jeff",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new Taunt("NOOOO MY END IS NEAR!"),
                    new Shoot(30, count: 2, shootAngle: 16, projectileIndex: 0, predictive: 0.4, coolDown: 600),
                    new Shoot(30, count: 8, shootAngle: 45, projectileIndex: 1, predictive: 0.2, coolDown: 1100),
                    new Grenade(2.5, 100, 10),
                    //	new ManaDrainBomb(6, 200, 15, coolDown: 300),
                    new Shoot(25, count: 3, shootAngle: 20, projectileIndex: 3, predictive: 0.3, coolDown: 900),
                    new HpLessTransition(0.20, "DeathSuicide")
                    ),
                new State("DeathSuicide",
                    new EntitiesNotExistsTransition(50, "Dead1", "Tamir"),
                    new EntitiesNotExistsTransition(50, "Dead2", "Tamir")
                    ),
                new State("Dead1",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Taunt("Killing my brother is going to be your last mistake..."),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 90, coolDownOffset: 0, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 100, coolDownOffset: 200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 110, coolDownOffset: 400, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 120, coolDownOffset: 600, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 130, coolDownOffset: 800, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 140, coolDownOffset: 1000, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 150, coolDownOffset: 1200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 160, coolDownOffset: 1400, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 160, coolDownOffset: 0, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 150, coolDownOffset: 200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 140, coolDownOffset: 400, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 130, coolDownOffset: 600, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 120, coolDownOffset: 800, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 110, coolDownOffset: 1000, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 100, coolDownOffset: 1200, coolDown: 10000),
                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, fixedAngle: 90, coolDownOffset: 1400, coolDown: 10000),
                    new Shoot(30, count: 1, shootAngle: 10, projectileIndex: 0, predictive: 0.5, coolDown: 800),
                    new Shoot(25, count: 3, shootAngle: 20, projectileIndex: 3, predictive: 0.3, coolDown: 1100),
                    new HpLessTransition(0.1, "Final Death")
                    ),
                new State("Dead2",
                    new Taunt("Hmph, maybe humans are stronger then I could have imagined..."),

                    new Shoot(50, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 1200),
                    new Shoot(50, count: 1, shootAngle: 10, projectileIndex: 0, predictive: 0.4, coolDown: 750),
                    new Shoot(50, count: 4, shootAngle: 25, projectileIndex: 3, predictive: 0.2, coolDown: 900),
                    new HpLessTransition(0.1, "Final Death")
                    ),
                new State("Final Death",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Flash(0xff0000, 0.8, 60),
                    new Taunt("Mother...Brother... I am sorry..."),
                    new TimedTransition(1000, "2")
                    ),
                new State("2",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Flash(0xff0000, 0.6, 60),
                    new Taunt("Enjoy while you can humans, we shall be back..."),
                    new TimedTransition(1000, "1")
                    ),
                new State("1",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Flash(0xff0000, 0.3, 70),
                    new Taunt("Goodbye mortals."),
                    new TimedTransition(1000, "Goodbye")
                    ),
                new State("Goodbye",
                    new Shoot(0, count: 10, projectileIndex: 4, shootAngle: 36, fixedAngle: 0),
                    new Suicide()
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Attack", 0.8),
                new ItemLoot("Potion of Mana", 0.5),
                new ItemLoot("Potion of Defense", 0.8),
                new ItemLoot("Potion of Speed", 0.8)
                ///		  new ItemLoot("Succubus Horn", 0.3)
                ),
            new Threshold(1,
                new TierLoot(12, ItemType.Armor, 0.5),
                new TierLoot(11, ItemType.Armor, 0.4),
                new TierLoot(10, ItemType.Armor, 0.3)
                )
            )
        .Init("Tamir",
            new State(
                new State("Awaken",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new PlayerWithinTransition(10, "Start")
                    ),
                new State("Start",
                    new Taunt("My mother... What have you done..."),
                    new ScaleHP2(20),
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new Shoot(15, count: 2, shootAngle: 16, projectileIndex: 0, predictive: 0.4, coolDown: 800),
                    new Grenade(2.5, 80, 6),
                    new Shoot(20, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 1100),
                    new HpLessTransition(0.9, "begin")
                    ),
                new State("begin",
                    new Taunt("I will avenge you..."),
                    new HealSelf(amount: 15000, coolDown: 7500),
                    //new ManaDrainBomb(radius: 2, damage: 75, range: 6, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000),
                    new Shoot(radius: 8, count: 6, shootAngle: 30, projectileIndex: 0, coolDown: 1200),
                    new Shoot(radius: 12, projectileIndex: 2, coolDown: 2000),
                    new Shoot(radius: 4, projectileIndex: 1, coolDown: 6000),
                    new Shoot(radius: 2, count: 2, shootAngle: 22.5, projectileIndex: 3, coolDown: 6000, coolDownOffset: 200),
                    new Shoot(radius: 3, count: 3, shootAngle: 22.5, projectileIndex: 3, coolDown: 6000, coolDownOffset: 400),
                    new Shoot(radius: 4, count: 4, shootAngle: 22.5, projectileIndex: 3, coolDown: 6000, coolDownOffset: 600),
                    new Shoot(radius: 5, count: 5, shootAngle: 22.5, projectileIndex: 3, coolDown: 6000, coolDownOffset: 800),
                    new Shoot(radius: 8, count: 12, shootAngle: 360 / 12, projectileIndex: 1, coolDown: 4750),
                    new Shoot(radius: 8, count: 8, shootAngle: 360 / 8, projectileIndex: 2, coolDown: 4750, coolDownOffset: 200),
                    new HpLessTransition(0.77, "Fight")
                    ),
                new State("Fight",
                    new Taunt(1.0, true, "I obtain my Mother's power...", " You will PAY for killing Wingus!"),
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Shoot(radius: 50, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 200, coolDownOffset: 2000),
                    new HpLessTransition(0.55, "SpawnMinion")
                    ),
                new State("SpawnMinion",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Spawn("Fiery Twin Succubus", maxChildren: 5, coolDown: 10000),
                    new TimedTransition(2000, "Checkifdead")
                    ),
                new State("Checkifdead",
                    new EntitiesNotExistsTransition(50, "Warning", "Fiery Twin Succubus")
                    ),
                new State("Warning",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new Flash(0xff0000, 0.5, 60),
                    new Taunt("Hmm.. So you have beaten me... interesting."),
                    new Wander(1),
                    new Shoot(10, count: 8, projectileIndex: 3, coolDown: 2000),
                    new HpLessTransition(0.2, "Death Encounter")
                    ),
                new State("Death Encounter",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Flash(0xff0000, 0.8, 60),
                    new Taunt("I failed..mother... forgive me.."),
                    new TimedTransition(1000, "2")
                    ),
                new State("2",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable, false, 0), // ok
                    new Flash(0xff0000, 0.6, 60),
                    new Taunt("I am dying..."),
                    new TimedTransition(1000, "1")
                    ),
                new State("1",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invulnerable), // ok
                    new Flash(0xff0000, 0.3, 70),
                    new Taunt("Goodbye."),
                    new TimedTransition(1000, "Goodbye")
                    ),
                new State("Goodbye",
                    new Shoot(0, count: 10, projectileIndex: 4, shootAngle: 36, fixedAngle: 0),
                    new Suicide()
                    )
                ),
            new Threshold(0.01,
                new ItemLoot("Potion of Dexterity", 0.8),
                new ItemLoot("Potion of Mana", 0.5),
                new ItemLoot("Potion of Vitality", 0.8),
                new ItemLoot("Potion of Wisdom", 0.8)
                ///  new ItemLoot("Succubus Horn", 0.3)
                ),
            new Threshold(1,
                new TierLoot(9, ItemType.Weapon, 0.5),
                new TierLoot(10, ItemType.Weapon, 0.4),
                new TierLoot(11, ItemType.Weapon, 0.3)

                )
            )
        .Init("Fiery Succubus",
            new State(
                new Prioritize(
                    new Chase()
                    ),
                new State("Shoot1",
                    new Shoot(30, count: 1, shootAngle: 10, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.3, coolDown: 400),
                    new Shoot(30, count: 3, shootAngle: 20, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.2, coolDown: 800),
                    new HpLessTransition(0.67, "Shoot2")
                    ),
                new State("Shoot2",
                    new Shoot(30, count: 8, shootAngle: 45, projectileIndex: Fiery_Succubus.fire_shard, coolDown: 1000),
                    new Shoot(30, count: 1, shootAngle: 10, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.3, coolDown: 400),
                    new HpLessTransition(0.34, "Shoot3")
                    ),
                new State("Shoot3",
                    new Shoot(30, count: 5, shootAngle: 65, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.1, coolDown: 700),
                    new Shoot(30, count: 3, shootAngle: 20, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.2, coolDown: 800),
                    new Shoot(20, count: 1, shootAngle: 10, projectileIndex: Fiery_Succubus.fire_enchanted_bullet, predictive: 0.3, coolDown: 400),
                    new HpLessTransition(0.12, "Suicide")
                    ),
                new State("Suicide",
                    new Shoot(0, count: 10, projectileIndex: Fiery_Succubus.fire_enchanted_bullet, shootAngle: 36, fixedAngle: 0),
                    new Suicide()
                    )
                 )
            )
        .Init("Fiery Twin Succubus",
            new State(
                new Prioritize(
                    new Chase()
                    ),
                new State("Shooting1",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 5, coolDown: 3000, coolDownOffset: 0, shootAngle: 72),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 5, coolDown: 3000, coolDownOffset: 800, shootAngle: 72),
                    new HpLessTransition(0.7, "Shooting2")
                    ),
                new State("Shooting2",
                    new Wander(0.4),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 8, coolDown: 2000, coolDownOffset: 0, angleOffset: 0, shootAngle: 45),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.vulcanum, count: 8, coolDown: 2000, coolDownOffset: 1000, angleOffset: 22.5, shootAngle: 45),
                    new HpLessTransition(0.5, "Shooting3")
                    ),
                new State("Shooting3",
                    new Wander(0.4),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_enchanted_bullet, count: 1, coolDown: 1000),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.vulcanum, count: 8, shootAngle: 45, coolDown: 1300),
                    new HpLessTransition(0.32, "Shooting4")
                    ),
                new State("Shooting4",
                    new Wander(0.4),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 8, angleOffset: 0, shootAngle: 45),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.vulcanum, count: 8, angleOffset: 22.5, shootAngle: 45),
                    new HpLessTransition(0.15, "SpawnMinion")
                    ),
                new State("SpawnMinion",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new Spawn("Fiery Succubus", maxChildren: 6, coolDown: 10000),
                    new TimedTransition(5000, "Suicide")
                    ),
                new State("Suicide",
                    new Shoot(0, count: 10, projectileIndex: Fiery_Twin_Succubus.fire_enchanted_bullet, shootAngle: 36, fixedAngle: 0),
                    new Suicide()
                    )
                 )
            )
        .Init("Icy Succubus",
            new State(
                new Prioritize(
                    new Chase()
                    ),
                new State("Shoot1",
                    new Shoot(30, count: 1, shootAngle: 10, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.3, coolDown: 400),
                    new Shoot(30, count: 3, shootAngle: 20, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.2, coolDown: 800),
                    new HpLessTransition(0.67, "Shoot2")
                    ),
                new State("Shoot2",
                    new Shoot(30, count: 8, shootAngle: 45, projectileIndex: Fiery_Succubus.fire_shard, coolDown: 1000),
                    new Shoot(30, count: 1, shootAngle: 10, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.3, coolDown: 400),
                    new HpLessTransition(0.34, "Shoot3")
                    ),
                new State("Shoot3",
                    new Shoot(30, count: 5, shootAngle: 65, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.1, coolDown: 700),
                    new Shoot(30, count: 3, shootAngle: 20, projectileIndex: Fiery_Succubus.fire_shard, predictive: 0.2, coolDown: 800),
                    new Shoot(20, count: 1, shootAngle: 10, projectileIndex: Fiery_Succubus.fire_enchanted_bullet, predictive: 0.3, coolDown: 400),
                    new HpLessTransition(0.12, "Suicide")
                    ),
                new State("Suicide",
                    new Shoot(0, count: 10, projectileIndex: Fiery_Succubus.fire_enchanted_bullet, shootAngle: 36, fixedAngle: 0),
                    new Suicide()
                    )
                 )
            )
        .Init("Icy Twin Succubus",
            new State(
                new Prioritize(
                    new Chase()
                    ),
                new State("Shooting1",
                    new ConditionEffectBehavior(ConditionEffectIndex.Armored),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 5, coolDown: 3000, coolDownOffset: 0, shootAngle: 72),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 5, coolDown: 3000, coolDownOffset: 800, shootAngle: 72),
                    new HpLessTransition(0.7, "Shooting2")
                    ),
                new State("Shooting2",
                    new Wander(0.4),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 8, coolDown: 2000, coolDownOffset: 0, angleOffset: 0, shootAngle: 45),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.vulcanum, count: 8, coolDown: 2000, coolDownOffset: 1000, angleOffset: 22.5, shootAngle: 45),
                    new HpLessTransition(0.5, "Shooting3")
                    ),
                new State("Shooting3",
                    new Wander(0.4),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_enchanted_bullet, count: 1, coolDown: 1000),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.vulcanum, count: 8, shootAngle: 45, coolDown: 1300),
                    new HpLessTransition(0.32, "Shooting4")
                    ),
                new State("Shooting4",
                    new Wander(0.4),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.fire_shard, count: 8, angleOffset: 0, shootAngle: 45),
                    new Shoot(30, projectileIndex: Fiery_Twin_Succubus.vulcanum, count: 8, angleOffset: 22.5, shootAngle: 45),
                    new HpLessTransition(0.15, "SpawnMinion")
                    ),
                new State("SpawnMinion",
                    new ConditionEffectBehavior(ConditionEffectIndex.Invincible),
                    new Spawn("Fiery Succubus", maxChildren: 6, coolDown: 10000),
                    new TimedTransition(5000, "Suicide")
                    ),
                new State("Suicide",
                    new Shoot(0, count: 10, projectileIndex: Fiery_Twin_Succubus.fire_enchanted_bullet, shootAngle: 36, fixedAngle: 0),
                    new Suicide()
                    )
                 )
            );
    }
}
