using common.resources;
using System;

namespace wServer.core.objects
{
    public partial class Player
    {
        private float _bleeding;
        private int _canTpCooldownTime;
        private float _healing;
        private int _newbieTime;

        public bool CheckMarbleHand()
        {
            var ok = false;
            for (var i = 0; i < 20; i++)
            {
                var inv = Inventory[i];
                if (inv != null && inv.ObjectId == "Severed Marble Hand")
                    ok = true;
            }

            return ok == true;
        }
        public bool CheckCerberusCore()
        {
            var ok = false;
            for (var i = 0; i < 20; i++)
            {
                var inv = Inventory[i];
                if (inv != null && inv.ObjectId == "Cerberus's Left Claw")
                    ok = true;
            }

            return ok == true;
        }
        public bool CheckCerberusRightClaw()
        {
            var ok = false;
            for (var i = 0; i < 20; i++)
            {
                var inv = Inventory[i];
                if (inv != null && inv.ObjectId == "Cerberus's Right Claw")
                    ok = true;
            }

            return ok == true;
        }
        public bool CheckCerberusLeftClaw()
        {
            var ok = false;
            for (var i = 0; i < 20; i++)
            {
                var inv = Inventory[i];
                if (inv != null && inv.ObjectId == "Cerberus's Core")
                    ok = true;
            }

            return ok == true;
        }
        public bool CheckMPRegeneration()
        {

            var ok = false;
            for (var i = 0; i < 20; i++)
            {
                var inv = Inventory[i];
                if (inv != null && inv.ObjectId == "Talisman of Mana" && MP <= Stats[1] / 2)
                    ok = true;
            }

            return ok == true;
        }

        public bool CheckRegeneration()
        {
            var ok = false;
            for (var i = 0; i < 20; i++)
            {
                var inv = Inventory[i];
                if (inv != null && inv.ObjectId == "Gem of Life" && HP <= Stats[0] / 2)
                    ok = true;
            }

            return ok == true;
        }

        public bool IsVisibleToEnemy()
        {
            if (_newbieTime > 0)
                return false;

            if (HasConditionEffect(ConditionEffects.Paused))
                return false;

            if (HasConditionEffect(ConditionEffects.Invisible))
                return false;

            if (HasConditionEffect(ConditionEffects.Hidden))
                return false;

            return true;
        }

        public bool TPCooledDown() => !(_canTpCooldownTime > 0);

        internal void RestartTPPeriod() => _canTpCooldownTime = 0;

        internal void SetNewbiePeriod() => _newbieTime = 3000;

        internal void SetTPDisabledPeriod() => _canTpCooldownTime = 10000;

        private bool CanHpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Sick))
                return false;

            if (HasConditionEffect(ConditionEffects.Bleeding))
                return false;

            return true;
        }

        private bool CanMpRegen() => !(HasConditionEffect(ConditionEffects.Quiet) || HasConditionEffect(ConditionEffects.NinjaSpeedy));

        private void HandleEffects(TickTime time)
        {
            if (Client == null || Client.Account == null) return;
            if (CheckCerberusLeftClaw())
            {
                ApplyConditionEffect(ConditionEffectIndex.DazedImmune);
                ApplyConditionEffect(ConditionEffectIndex.Dazed, 0);
            }
            else if (!CheckCerberusLeftClaw() && HasConditionEffect(ConditionEffects.DazedImmune))
            {
                ApplyConditionEffect(ConditionEffectIndex.DazedImmune, 0);
            }
            if (CheckCerberusRightClaw())
            {
                ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);
                ApplyConditionEffect(ConditionEffectIndex.Slowed, 0);
            }
            else if (!CheckCerberusRightClaw() && HasConditionEffect(ConditionEffects.SlowedImmune))
            {
                ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, 0);
            }
            if (CheckCerberusCore())
            {
                ApplyConditionEffect(ConditionEffectIndex.DazedImmune);
                ApplyConditionEffect(ConditionEffectIndex.Dazed, 0);
                ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);
                ApplyConditionEffect(ConditionEffectIndex.StunImmune);
                ApplyConditionEffect(ConditionEffectIndex.Slowed, 0);
                ApplyConditionEffect(ConditionEffectIndex.Stunned, 0);
            }
            else if (!CheckCerberusCore() && (HasConditionEffect(ConditionEffects.DazedImmune)))
                ApplyConditionEffect(ConditionEffectIndex.DazedImmune, 0);
            else if (!CheckCerberusCore() && (HasConditionEffect(ConditionEffects.SlowedImmune)))
                ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, 0);
            else if (!CheckCerberusCore() && (HasConditionEffect(ConditionEffects.StunImmune)))
                ApplyConditionEffect(ConditionEffectIndex.StunImmune, 0);
            if (CheckMarbleHand())
            {
                ApplyConditionEffect(ConditionEffectIndex.UnstableImmune);
                ApplyConditionEffect(ConditionEffectIndex.Unstable, 0);
            }
            else if (!CheckMarbleHand() && HasConditionEffect(ConditionEffects.UnstableImmune))
            {
                ApplyConditionEffect(ConditionEffectIndex.UnstableImmune, 0);
            }

            if (CheckRegeneration())
            {
                ApplyConditionEffect(ConditionEffectIndex.Regeneration);
            }
            else if (!CheckRegeneration() && HasConditionEffect(ConditionEffects.Regeneration))
            {
                ApplyConditionEffect(ConditionEffectIndex.Regeneration, 0);
            }

            if (CheckMPRegeneration())
            {
                ApplyConditionEffect(ConditionEffectIndex.MPTRegeneration);
            }
            else if (!CheckMPRegeneration() && HasConditionEffect(ConditionEffects.MPTRegeneration))
            {
                ApplyConditionEffect(ConditionEffectIndex.MPTRegeneration, 0);
            }

            if (Client.Account.Hidden && !HasConditionEffect(ConditionEffects.Hidden))
            {
                ApplyConditionEffect(ConditionEffectIndex.Hidden);
                ApplyConditionEffect(ConditionEffectIndex.Invincible);
                CoreServerManager.ConnectionManager.Clients[Client].Hidden = true;
            }

            if (HasConditionEffect(ConditionEffects.Healing) && !HasConditionEffect(ConditionEffects.Sick))
            {
                if (_healing > 1)
                {
                    HP = Math.Min(Stats[0], HP + (int)_healing);
                    _healing -= (int)_healing;
                }
                _healing += 28 * time.DeltaTime;
            }

            if (HasConditionEffect(ConditionEffects.Quiet) && MP > 0)
                MP = 0;

            if (HasConditionEffect(ConditionEffects.Bleeding) && HP > 1)
            {
                if (_bleeding > 1)
                {
                    HP -= (int)_bleeding;
                    if (HP < 1)
                        HP = 1;
                    _bleeding -= (int)_bleeding;
                }
                _bleeding += 28 * time.DeltaTime;
            }

            if (HasConditionEffect(ConditionEffects.NinjaSpeedy))
            {
                MP = Math.Max(0, (int)(MP - 10 * time.ElaspedMsDelta / 1000f));

                if (MP == 0)
                    ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy, 0);
            }
            if (HasConditionEffect(ConditionEffects.NinjaBerserk))
            {
                MP = Math.Max(0, (int)(MP - 10 * time.ElaspedMsDelta / 1000f));

                if (MP == 0)
                    ApplyConditionEffect(ConditionEffectIndex.NinjaBerserk, 0);
            }

            if (_newbieTime > 0)
            {
                _newbieTime -= time.ElaspedMsDelta;
                if (_newbieTime < 0)
                    _newbieTime = 0;
            }

            if (_canTpCooldownTime > 0)
            {
                _canTpCooldownTime -= time.ElaspedMsDelta;
                if (_canTpCooldownTime <= 0)
                    _canTpCooldownTime = 0;
            }
        }
    }
}
