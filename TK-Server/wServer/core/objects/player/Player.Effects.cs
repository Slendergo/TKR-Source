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

            if (Client.Account.Hidden && !HasConditionEffect(ConditionEffects.Hidden))
            {
                ApplyConditionEffect(ConditionEffectIndex.Hidden);
                ApplyConditionEffect(ConditionEffectIndex.Invincible);
                GameServer.ConnectionManager.Clients[Client].Hidden = true;
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
                MP = Math.Max(0, (int)(MP - 10 * time.DeltaTime));

                if (MP == 0)
                    ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy, 0);
            }

            if (HasConditionEffect(ConditionEffects.NinjaBerserk))
            {
                MP = Math.Max(0, (int)(MP - 10 * time.DeltaTime));

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
