using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Objects
{
    public class GunStatus : Gun, IGunStatus
    {
        private static int UniqueGunSeed = 0;
        public int CurrentAmmo { get; private set; }
        public GunFiringStatus Status { get; private set; }
        public int Cost { get; private set; }
        public int UniqueId { get; }

        private Rng Rng;

        public GunStatus(Rng rng,
                         IGun gun)
        {
            Assert.IsNotNull(gun);
            Assert.IsNotNull(rng);

            Properties = gun.Properties;
            Reload = gun.Reload;
            Misfire = gun.Misfire;
            Range = gun.Range;
            GunCost = gun.GunCost;
            AmmoCost = gun.AmmoCost;
            HitModifier = gun.HitModifier;
            DamageModifier = gun.DamageModifier;
            DamageDice = gun.DamageDice;    //TODO: deep copy?
            Cost = 0;
            UniqueId = GetUniqueSeed();

            CurrentAmmo = Reload;
            Status = GunFiringStatus.Okay;
            Rng = rng;
        }

        public bool HasShotLoaded()
        {
            return Reload == CommonConstants.InfiniteAmmo ||
                   CurrentAmmo > 0;
        }

        public bool CanFire()
        {
            return Status == GunFiringStatus.Okay;
        }

        public void ReloadChamber()
        {
            Assert.IsTrue(Status == GunFiringStatus.Okay);
            Assert.IsTrue(CurrentAmmo == 0);

            CurrentAmmo = Reload;
        }

        public bool FixMisfire(int tinkerToolProficiency)
        {
            ValidateFixMisfire(tinkerToolProficiency);

            bool success = RollFixMisfire(tinkerToolProficiency);
            Status = success
                ? GunFiringStatus.Okay
                : GunFiringStatus.Broken;

            if (Status == GunFiringStatus.Broken)
            {
                //If the gun is broken, we have to spend 1/4th its cost to repair.
                //Note gun cost is in gold. Convert to copper.
                Cost += ((GunCost * 100) / 4);
            }

            return success;
        }

        public AttackSummary Shoot(IEnemy enemy, CombatStats combatStats)
        {
            ValidateShoot(enemy, combatStats);

            int hitRoll = Rng.Roll(RollType.d20);

            AttackSummary summary;
            if (ShotMisfired(hitRoll))
            {
                HandleMisfire();
                summary = GetMisfireAttackSummary();
            }
            else
            {
                IModifier finalHitMod = HitModifier.Add(combatStats.HitModifier);
                IModifier finalDmgMod = DamageModifier.Add(combatStats.DamageModifier);

                summary = HandleShoot(enemy,
                                      hitRoll,
                                      finalHitMod,
                                      finalDmgMod,
                                      combatStats.CritValue);
            }

            DecrementCurrentAmmo();

            return summary;
        }

        public bool Equals(IGunStatus other)
        {
            return other != null &&
                   UniqueId == other.UniqueId;
        }

        private AttackSummary GetMisfireAttackSummary()
        {
            return new AttackSummary(0, 0, false, false);
        }

        private AttackSummary HandleShoot(IEnemy enemy,
                                          int hitRoll,
                                          IModifier hitMod,
                                          IModifier dmgMod,
                                          int critValue)
        {
            int attackResult = hitRoll + hitMod.Get();
            bool crit = Crit(critValue, hitRoll);

            int damageDone = 0;
            bool hit = false;
            if (crit)
            {
                enemy.CritHit();
                damageDone = GetCritDamage(dmgMod);
                enemy.TakeDamage(damageDone);
                hit = true;
            }
            else
            {
                if (enemy.TryHit(attackResult))
                {
                    damageDone = GetDamage(dmgMod);
                    enemy.TakeDamage(damageDone);
                    hit = true;
                }
            }

            return new AttackSummary(attackResult, damageDone, hit, crit);
        }

        private int GetUniqueSeed()
        {
            int seed = UniqueGunSeed;
            UniqueGunSeed++;

            return seed;
        }

        private bool ShotMisfired(int roll)
        {
            return roll <= Misfire;
        }

        private void HandleMisfire()
        {
            Status = GunFiringStatus.Misfired;
        }

        private void DecrementCurrentAmmo()
        {
            if (CurrentAmmo != CommonConstants.InfiniteAmmo)
            {
                CurrentAmmo--;
                Cost += AmmoCost;
            }
        }

        private int GetCritDamage(IModifier mod)
        {
            return DamageDice.Select(x => Rng.Roll(x) + 
                                          Rng.Roll(x))  //x2 because crit!
                             .Sum() +
                   mod.GetCrit();
        }

        private int GetDamage(IModifier mod)
        {
            int dmgDieResult = DamageDice.Select(x => Rng.Roll(x))
                             .Sum();
            int x = mod.Get();

            return dmgDieResult + x;

        }

        private bool Crit(int critValue, int roll)
        {
            return roll >= critValue;
        }

        private void ValidateShoot(IEnemy enemy, CombatStats combatStats)
        {
            Assert.IsNotNull(enemy);
            Assert.IsNotNull(combatStats);

            Assert.IsTrue(CanFire());
            Assert.IsTrue(HasShotLoaded());
        }

        private void ValidateFixMisfire(int tinkerToolProficiency)
        {
            Assert.IsTrue(Status == GunFiringStatus.Misfired);
            Assert.IsTrue(tinkerToolProficiency >= 0);
        }

        private bool RollFixMisfire(int tinkerToolProficiency)
        {
            int misfireDc = Misfire + CommonConstants.BaseMisfireDc;
            int roll = Rng.Roll(RollType.d20);
            int result = tinkerToolProficiency + roll;

            return result >= misfireDc;
        }
    }
}
