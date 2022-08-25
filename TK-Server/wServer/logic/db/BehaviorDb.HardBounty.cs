using common;
using common.resources;
using Org.BouncyCastle.Asn1.Cms;
using StackExchange.Redis;
using wServer.core.objects;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ HardBounty = () => Behav()
        .Init("Cerebrus",
            new State(
                new ScaleHP2(20),
                    new State("Pause",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(10, "Start")
                        ),
                    new State("Start",
                        new Taunt("AROOOOOOOOOOOOO!"),
                        new TimedTransition(3000, "Fight")
                        ),
                    new State("Fight",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new StayCloseToSpawn(3, 15),
                        new Wander(0.4),
                        new Shoot(12, 1, projectileIndex: 3, coolDown: 500, predictive: 1.1),
                        new Shoot(15, 3, projectileIndex: 4, fixedAngle: 45, shootAngle: 10, rotateAngle: 25, coolDown: 100),
                        new Shoot(15, 3, projectileIndex: 4, fixedAngle: 135, shootAngle: 10, rotateAngle: 25, coolDown: 100),
                        new Shoot(15, 3, projectileIndex: 4, fixedAngle: 225, shootAngle: 10, rotateAngle: 25, coolDown: 100),
                        new Shoot(15, 3, projectileIndex: 4, fixedAngle: 315, shootAngle: 10, rotateAngle: 25, coolDown: 100),
                        new HpLessTransition(0.50, "Fight 2")
                        ),
                    new State("Fight 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ReturnToSpawn(1),
                        new TimedTransition(2500, "Fight 2.1")
                        ),
                    new State ("Fight 2.1",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(15, 8, projectileIndex: 5, coolDown: 1000),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 0, coolDown: 2800, coolDownOffset: 0),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 6, coolDown: 2800, coolDownOffset: 200),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 12, coolDown: 2800, coolDownOffset: 400),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 18, coolDown: 2800, coolDownOffset: 600),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 24, coolDown: 2800, coolDownOffset: 800),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 30, coolDown: 2800, coolDownOffset: 1000),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 36, coolDown: 2800, coolDownOffset: 1200),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 42, coolDown: 2800, coolDownOffset: 1400),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 48, coolDown: 2800, coolDownOffset: 1600),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 54, coolDown: 2800, coolDownOffset: 1800),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 60, coolDown: 2800, coolDownOffset: 2000),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 66, coolDown: 2800, coolDownOffset: 2200),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 72, coolDown: 2800, coolDownOffset: 2400),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 78, coolDown: 2800, coolDownOffset: 2600),
                        new Shoot(15, 4, projectileIndex: 2, shootAngle: 90, fixedAngle: 84, coolDown: 2800, coolDownOffset: 2800),
                        new HpLessTransition(0.10, "Fight 3")
                        ),
                    new State("Fight 3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ChangeSize(10, 150),
                        new Flash(0xFF0000, 5, 10),
                        new HealSelf(coolDown: 500, amount: 25, percentage: true),
                        new TimedTransition(5000, "Fight 3.1")
                        ),
                    new State("Fight 3.1",
                        new Chase(8, coolDown: 0),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(15, 8, projectileIndex: 5, coolDown: 1000),
                        new Shoot(15, 2, projectileIndex: 0, shootAngle: 15, coolDown: 400),
                        new Shoot(15, 1, projectileIndex: 1, coolDown: 800, coolDownOffset: 0),
                        new Shoot(15, 2, projectileIndex: 1, shootAngle: 15, coolDown: 800, coolDownOffset: 200),
                        new Shoot(15, 2, projectileIndex: 1, shootAngle: 25, coolDown: 800, coolDownOffset: 400)
                         )
                    ),
             new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
                  new Threshold(0.05,
                    new ItemLoot("Cerberus's Ribcage", 0.0015),
                    new ItemLoot("Beast Gem", 0.0015),
                    new ItemLoot("Heart of the Beast", 0.0015)
                  
                )
            )
        .Init("Grim Reaper",
            new State(
                new State("Awaken",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("I, Thanatos, God of Death, faithful servant of Hades, have come to reap your souls. I will avenge Cereberus"),
                  //  new ChangeSize(3, 500),
                  //  new Flash(0x000001, 5, 2),
                    new ScaleHP2(20),
                    new TimedTransition(5000, "1")
                    ),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Prioritize(
                            new Chase(),
                            new Shoot(12, count: 1,projectileIndex:0, coolDown: 500)
                            ),
                    new Chase(),
                    new Taunt("Soul of the deads... I command thee to do my biding... explode."),
                     new TossObject2("Reaper's Bomb", 30, coolDown:5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 360),
                    new Shoot(20, 4, 15, 0, coolDown: 5000),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 0, coolDownOffset: 0, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 45, coolDownOffset: 100, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 90, coolDownOffset: 200, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 135, coolDownOffset: 300, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 180, coolDownOffset: 400, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 225, coolDownOffset: 500, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 270, coolDownOffset: 600, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 315, coolDownOffset: 700, coolDown: 900),
                    new Shoot(20, 1, projectileIndex: 1, fixedAngle: 360, coolDownOffset: 800, coolDown: 900),
                    new HpLessTransition(.85, "2")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("ΝΟΜΟΘΕΣΙΑ ΣΤΟΝ ΟΠΛΟ!"),
                    new TossObject2("Hard Bounty Skeletons", 6, 0, coolDown:   100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 10, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 20, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 30, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 40, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 50, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 60, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 70, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 80, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 90, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 100, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 115, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 120, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 130, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 140, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 150, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 160, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 170, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 180, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 190, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 200, coolDown: 100000, randomToss: true),
					//new TossObject2("Hard Bounty Skeletons", 6, 210, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 220, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 230, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 240, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 250, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 260, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 270, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 280, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 290, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 300, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 310, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 320, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 330, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 340, coolDown: 100000, randomToss: true),
				//	new TossObject2("Hard Bounty Skeletons", 6, 350, coolDown: 100000, randomToss: true),
					new TossObject2("Hard Bounty Skeletons", 6, 360, coolDown: 100000, randomToss: true),

					//new Taunt("Soul of the deads... I command thee to do my biding... explode."),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 0),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 45),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 90),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 135),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 180),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 225),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 270),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 315),
					//new TossObject2("Reaper's Bomb", 30, coolDown: 3500, angle: 360),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 0),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 45),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 90),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 135),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 180),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 225),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 270),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 315),
					//new TossObject2("Reaper's Bomb", 25, coolDown: 3500, angle: 360),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 0),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 45),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 90),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 135),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 180),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 225),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 270),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 315),
					//new TossObject2("Reaper's Bomb", 20, coolDown: 3500, angle: 360),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 0),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 45),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 90),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 135),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 180),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 225),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 270),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 315),
					//new TossObject2("Reaper's Bomb", 15, coolDown: 3500, angle: 360),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 0),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 45),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 90),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 135),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 180),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 225),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 270),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 315),
					//new TossObject2("Reaper's Bomb", 10, coolDown: 3500, angle: 360),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 0),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 45),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 90),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 135),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 180),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 225),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 270),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 315),
					//new TossObject2("Reaper's Bomb", 5, coolDown: 3500, angle: 360),        
					new TimedTransition(5000, "3")
                    ),
                new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new Prioritize(
                        new Chase(),
                        new Shoot(20, 10, 30, 1, coolDown: 500)
                        ),
                    new TossObject2("Hard Bounty Skeletons", 2, 0, coolDown: 3000, randomToss: true),
                    new TossObject2("Hard Bounty Skeletons", 2, 0, coolDown: 3000, randomToss: true),
                    new TossObject2("Hard Bounty Skeletons", 2, 0, coolDown: 3000, randomToss: true),
                    new Chase(),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, predictive: 0.2, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: 10, predictive: 0.2, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: 20, predictive: 0.2, coolDown: 700), 
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: -10, predictive: 0.2, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: -20, predictive: 0.2, coolDown: 400),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, predictive: 0.6, coolDown: 700),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: 30, predictive: 0.6, coolDown: 600),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: 40, predictive: 0.6, coolDown: 700), 
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: -30, predictive: 0.6, coolDown: 500),
                    new Shoot(15, 1, shootAngle: 5, projectileIndex: 1, angleOffset: -40, predictive: 0.6, coolDown: 400),
                    new HpLessTransition(0.5, "4")
                    ),
                new State("4",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("Hee hee... the difference between you and me is that... im immortal ..."),
                    new ChangeSize(3, 700),
                    new HealSelf(coolDown: 10000, 200000),
                    new Taunt("Now.. Let me introduce to you my lovely Scythe, Cronus"),
                    new TossObject2("Reaper's Scythe", 5, 0, coolDown: 99999),
                    new TossObject2("Reaper's Scythe", 5, 180, coolDown: 99999),
                    new Taunt("Cronuses, eliminate these fools for me hee hee"),
                    new TimedTransition(1000, "5")
                    ),
                new State("5",
                    new Taunt("Behold the might of Hell!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new TossObject2("Hard Bounty Skeletons", coolDown: 10000, randomToss: true),
                    new TossObject2("Hard Bounty Skeletons", coolDown: 10000, randomToss: true),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 360),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 0, fixedAngle: 0, coolDown: 400),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 10, fixedAngle: 10, coolDown: 400),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 20, fixedAngle: 20, coolDown: 400),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: -10, fixedAngle: -20, coolDown: 400),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: -20, fixedAngle: -10, coolDown: 400),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 180, fixedAngle: 180, coolDown: 600),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 170, fixedAngle: 170, coolDown: 600),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 160, fixedAngle: 160, coolDown: 600),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 190, fixedAngle: 190, coolDown: 600),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 200, fixedAngle: 200, coolDown: 600),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 90, fixedAngle: 90, coolDown: 800),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 80, fixedAngle: 80, coolDown: 800),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 70, fixedAngle: 70, coolDown: 800),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 100, fixedAngle: 100, coolDown: 800),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 110, fixedAngle: 110, coolDown: 800),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: -90, fixedAngle: -90, coolDown: 1000),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: -80, fixedAngle: -80, coolDown: 1000),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: -70, fixedAngle: -70, coolDown: 1000),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 100, fixedAngle: -100, coolDown: 1000),
                    new Shoot(15, projectileIndex: 1, count: 2, shootAngle: 110, fixedAngle: -110, coolDown: 1000),
                    new Shoot(15, 18, shootAngle: 10, projectileIndex: 0, angleOffset: 10, coolDown: 10000, coolDownOffset: 6000),
                    new HpLessTransition(.1, "6")
                    ),
                new State("6",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You fools.. I was trying to save you... now you face Hades in all his might... be warned... his powers are tenfold greater than me and Cerebrus combined..."),
                    new TimedTransition(3000, "7")
                    ),
                new State("7",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(30, 48, projectileIndex: 1, defaultAngle: 0, shootAngle: 7.5, coolDown: 100000),
                    new Shoot(30, 24, projectileIndex: 0, defaultAngle: 0, shootAngle: 15, coolDown: 100000, coolDownOffset: 50),
                    new Shoot(30, 48, projectileIndex: 1, defaultAngle: 0, shootAngle: 7.5, coolDown: 1000000),
                                     new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 30, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 25, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 20, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 15, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 10, coolDown: 5000, angle: 360),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 0),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 45),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 90),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 135),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 180),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 225),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 270),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 315),
                    new TossObject2("Reaper's Bomb", 5, coolDown: 5000, angle: 360),
                    new Suicide()
                    )
                ), 
                new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
                  new Threshold(0.05,
                    new ItemLoot("Raven's Head", 1),
                    new ItemLoot("Thanatos's Garments", 1)
                )
            )
        .Init("Reaper's Scythe",
            new State(
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new Taunt("My Master calls, I shall serve faithfully."),
                    new Chase(),
                    new TimedTransition(1000, "1")
                    ),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new Prioritize(
                        new Chase(3.5),
                        new Shoot(50, 4, 40, 0, coolDown: 500)
                        ),
                    new Shoot(15, 20, fixedAngle: 360 / 20, projectileIndex: 0, coolDownOffset: 0, coolDown: 1000),
                    new EntityNotExistsTransition("Grim Reaper", 100, "Decay")
                    ),
                new State("Decay",
                    new Taunt("We've failed thee."),
                    new Suicide()
                    )
                )
            )
        .Init("Dark Skeleton",
            new State(
                new State("Start",
                    new Orbit(0.4,4,10,"Wraith"),
                    new Taunt("Leave this place fools!"),
                    new Shoot(5,1,null,0,coolDown:500),
                    new Spawn("Hard Bounty Skeletons",5,1, coolDown:10000)                 
                        )
                    ),
             new Threshold(0.001,
                LootTemplates.DustLoot()
                ),
                new Threshold(.05,
                    new ItemLoot("Dark Skeleton's Shield", 0.0015)
                    )
            )
        .Init("Hard Bounty Skeletons",
            new State(
                new State("Summoned",
                    new Prioritize(
                        new Chase(),
                        new Shoot(3,3, 20, coolDown:1000)
                        ),
                    new Chase()
                    )
                )
            )
        .Init("Reaper's Bomb",
            new State(
                new State("Wait",
                    new TimedTransition(1000, "Suicde")
                    ),
                new State("Suicde",
                    new Shoot(10, 8,60,0,coolDown:100000),
                    new TimedTransition(500,"Dead")
                    
                    ),
                new State("Dead",
                    new Suicide()
                    )
                )
            )
        .Init("Wraith",
            new State(
                new State("Start",
                    new Taunt("Eat this!"),
                    new Shoot(30,2,20,0,coolDown:3000),
                    new Shoot(30,1,projectileIndex:1,predictive:1,coolDown:500)             
                    )
                )
            )
        .Init("Hell Hound",
            new State(
                new State("Wait",
                    new Wander(0.3),
                    new TimedTransition(5000,"Begin")
                    ),
                new State("Begin",
                    new Taunt("My leige...I serve thee until I die!"),
                    new Prioritize(
                        new Chase(),
                        new Shoot(10, 3, 30, 0, predictive: 1)
                        ),
                    new Shoot(5, 10, fixedAngle: 10, shootAngle: 35, projectileIndex: 1),
                    new ReturnToSpawn(.4)
                    )
                )
            )
        .Init("NPC Ghost 1",
            new State(
                new State("lol",
                new Taunt(cooldown: 30000, "Urgh.... Help Me... heros... the passphrase is in ascending orders of the NPC's number..."),
                new Wander(.3),
                new PlayerTextTransition("Die", "omae wa mao shinderu", 100)
                ),
                new State("Die",
                    new Taunt("NANI?!"),
                    new Decay(0)
                    )
            )
            )
        .Init("NPC Ghost 2",
            new State(
                new Taunt(cooldown: 30000, "Hee Hee... I wonder if the japanese phrase that ends in nani will kill NPC Ghost 1.. lol..."),
                  new Wander(.3)
                )
            )
        .Init("NPC Ghost 3",
            new State(
                new Taunt(cooldown: 30000, "Heros.. We were just like you... now we are trapped here forever... i-i forgot the entire passphrase.. but I do remember it starting with 'lets enter'..."),
                new Wander(.3)
                )
            )
        .Init("NPC Ghost 4",
            new State(
                new Taunt(cooldown: 30000, "Its been so long since I seen living humans... we tried fighting Cerebrus too... except he was too powerful... maybe you can try? all I remember was 'the gates'.."),
                new Wander(.3)
                )
            )
        .Init("NPC Ghost 5",
            new State(
                new Taunt(cooldown: 30000, "Eternal Damnation.. please... someone end this madness.. 'to hades' castle'.. what does that mean?"),
                new Wander(.3)
                )
            )
         .Init("NPC Ghost 6",
            new State(
                new Taunt(cooldown: 30000, "Where is my family..."),
                new Wander(.3)
                )
            )
        .Init("NPC Ghost 7",
            new State(
                new Taunt(cooldown: 30000, "I want my mommy..."),
                new Wander(.3)
                )
            )
        .Init("NPC Ghost 8",
            new State(
                new Taunt(cooldown: 30000, "Please don't devour my soul..."),
                new Wander(.3)
                )
            )
        .Init("NPC Ghost 9",
            new State(
                new Taunt(cooldown: 30000, "Help me become mortal again..."),
                new Wander(.3)
                )
            );
    }
}