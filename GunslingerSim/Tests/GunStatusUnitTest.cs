using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class GunStatusUnitTest : BaseUnitTest
    {
        #region Setup

        IGun palmPistol;
        IGun artificerPalmPistol;
        IGun pistol;
        IGun musket;
        IGun pepperbox;
        IGun blunderbuss;

        IEnemy enemy;

        Rng always3;

        CombatStats combatStats;
        GunFactory factory;

        public GunStatusUnitTest()
        {
            AddTest(nameof(Test_Constructor_NullArgs), Test_Constructor_NullArgs);
            AddTest(nameof(Test_Constructor_SunnyDay), Test_Constructor_SunnyDay);

            AddTest(nameof(Test_HasShotLoaded_NoAmmo), Test_HasShotLoaded_NoAmmo);
            AddTest(nameof(Test_HasShotLoaded_HasAmmo), Test_HasShotLoaded_HasAmmo);
            AddTest(nameof(Test_HasShotLoaded_InfAmmo), Test_HasShotLoaded_InfAmmo);

            AddTest(nameof(Test_CanFire_GunMisfired), Test_CanFire_GunMisfired);
            AddTest(nameof(Test_CanFire_GunBroken), Test_CanFire_GunBroken);
            AddTest(nameof(Test_CanFire_GunOkay), Test_CanFire_GunOkay);

            AddTest(nameof(Test_ReloadChamber_GunMisfired), Test_ReloadChamber_GunMisfired);
            AddTest(nameof(Test_ReloadChamber_GunBroken), Test_ReloadChamber_GunBroken);
            AddTest(nameof(Test_ReloadChamber_StillHasAmmo), Test_ReloadChamber_StillHasAmmo);
            AddTest(nameof(Test_ReloadChamber_InfAmmo), Test_ReloadChamber_InfAmmo);
            AddTest(nameof(Test_ReloadChamber_OutOfAmmo), Test_ReloadChamber_OutOfAmmo);

            AddTest(nameof(Test_FixMisfire_GunOkay), Test_FixMisfire_GunOkay);
            AddTest(nameof(Test_FixMisfire_GunBroken), Test_FixMisfire_GunBroken);
            AddTest(nameof(Test_FixMisfire_NegativeProf), Test_FixMisfire_NegativeProf);
            AddTest(nameof(Test_FixMisfire_Fail), Test_FixMisfire_Fail);
            AddTest(nameof(Test_FixMisfire_Success), Test_FixMisfire_Success);

            AddTest(nameof(Test_Shoot_NullArgs), Test_Shoot_NullArgs);
            AddTest(nameof(Test_Shoot_CannotFire), Test_Shoot_CannotFire);
            AddTest(nameof(Test_Shoot_NoShotLoaded), Test_Shoot_NoShotLoaded);
            AddTest(nameof(Test_Shoot_Misfire), Test_Shoot_Misfire);
            AddTest(nameof(Test_Shoot_SuccessiveShootsDoNotChangeMods), Test_Shoot_SuccessiveShootsDoNotChangeMods);
            AddTest(nameof(Test_Shoot_Miss), Test_Shoot_Miss);
            AddTest(nameof(Test_Shoot_Hit), Test_Shoot_Hit);
            AddTest(nameof(Test_Shoot_Crit), Test_Shoot_Crit);

            AddTest(nameof(Test_Cost_SingleShot), Test_Cost_SingleShot);
            AddTest(nameof(Test_Cost_MultipleShots), Test_Cost_MultipleShots);
            AddTest(nameof(Test_Cost_SingleShotMiss), Test_Cost_SingleShotMiss);
            AddTest(nameof(Test_Cost_Broken), Test_Cost_Broken);
        }

        protected override void OneTimeSetup()
        {
            factory = new GunFactory();
            always3 = new Rng(3);
            combatStats = new CombatStats(new Modifier(3), new Modifier(2), 20);
        }

        protected override void Setup()
        {
            palmPistol = factory.Get(GunType.PalmPistol);
            artificerPalmPistol = factory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty);
            pistol = factory.Get(GunType.Pistol);
            musket = factory.Get(GunType.Musket);
            pepperbox = factory.Get(GunType.Pepperbox);
            blunderbuss = factory.Get(GunType.Blunderbuss);

            enemy = new Enemy(13);
        }

        protected override void TearDown()
        {
            palmPistol = null;
            artificerPalmPistol = null;
            pistol = null;
            musket = null;
            pepperbox = null;
            blunderbuss = null;

            enemy = null;
        }

        private void MultipleFire(IEnemy enemy, CombatStats combatStats, IGunStatus status, int numClips)
        {
            for (int i = 0; i < numClips; i++)
            {
                while (status.HasShotLoaded())
                {
                    status.Shoot(enemy, combatStats);
                }
                status.ReloadChamber();
            }
        }

        #endregion Setup

        #region Constructor

        private void Test_Constructor_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => new GunStatus(null, palmPistol));
            Assert.Throws<ArgumentNullException>(() => new GunStatus(always1, null));
        }

        private void Test_Constructor_SunnyDay()
        {
            Assert.DoesNotThrow(() => new GunStatus(always1, palmPistol));
        }

        #endregion Constructor

        #region Has Shot Loaded

        private void Test_HasShotLoaded_NoAmmo()
        {
            GunStatus status = new GunStatus(always10, palmPistol);
            status.Shoot(enemy, combatStats);

            Assert.IsTrue(!status.HasShotLoaded());
        }

        private void Test_HasShotLoaded_HasAmmo()
        {
            GunStatus status = new GunStatus(always10, pistol);

            Assert.IsTrue(status.HasShotLoaded());
        }

        private void Test_HasShotLoaded_InfAmmo()
        {
            GunStatus status = new GunStatus(always10, artificerPalmPistol);

            status.Shoot(enemy, combatStats);   //keep shooting
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);

            Assert.IsTrue(status.HasShotLoaded());
        }

        #endregion Has Shot Loaded

        #region Can Fire

        private void Test_CanFire_GunMisfired()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire

            Assert.IsTrue(!status.CanFire());
        }

        private void Test_CanFire_GunBroken()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire
            status.FixMisfire(0);   //try fix, will get broke

            Assert.IsTrue(!status.CanFire());
        }

        private void Test_CanFire_GunOkay()
        {
            GunStatus status = new GunStatus(always1, blunderbuss);

            Assert.IsTrue(status.CanFire());
        }

        #endregion Can Fire

        #region Reload Chamber

        private void Test_ReloadChamber_GunMisfired()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire

            Assert.Throws<ArgumentException>(() => status.ReloadChamber());
        }

        private void Test_ReloadChamber_GunBroken()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire
            status.FixMisfire(0);   //try fix, will get broke

            Assert.Throws<ArgumentException>(() => status.ReloadChamber());
        }

        private void Test_ReloadChamber_StillHasAmmo()
        {
            GunStatus status = new GunStatus(always1, pistol);
            Assert.Throws<ArgumentException>(() => status.ReloadChamber());
        }

        private void Test_ReloadChamber_InfAmmo()
        {
            GunStatus status = new GunStatus(always1, artificerPalmPistol);
            Assert.Throws<ArgumentException>(() => status.ReloadChamber());
        }

        private void Test_ReloadChamber_OutOfAmmo()
        {
            GunStatus status = new GunStatus(always10, pistol);

            //shoot x4 to run out of ammo
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);

            Assert.IsTrue(!status.HasShotLoaded());
            Assert.DoesNotThrow(() => status.ReloadChamber());
            Assert.IsTrue(status.HasShotLoaded());
            Assert.IsTrue(status.CanFire());
        }

        #endregion Reload Chamber

        #region Fix Misfire
        //TODO: this

        private void Test_FixMisfire_GunOkay()
        {
            GunStatus status = new GunStatus(always1, pepperbox);

            Assert.Throws<ArgumentException>(() => status.FixMisfire(3));
        }

        private void Test_FixMisfire_GunBroken()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire
            status.FixMisfire(0);   //try fix, will get broke

            Assert.Throws<ArgumentException>(() => status.FixMisfire(3));
        }

        private void Test_FixMisfire_NegativeProf()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire

            Assert.Throws<ArgumentException>(() => status.FixMisfire(-1));
            Assert.Throws<ArgumentException>(() => status.FixMisfire(-63));
            Assert.Throws<ArgumentException>(() => status.FixMisfire(int.MinValue));
        }

        private void Test_FixMisfire_Fail()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire

            Assert.DoesNotThrow(() => status.FixMisfire(0));
            Assert.AreEqual(GunFiringStatus.Broken, status.Status);
            Assert.IsTrue(!status.CanFire());
        }

        private void Test_FixMisfire_Success()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire

            Assert.DoesNotThrow(() => status.FixMisfire(20));
            Assert.AreEqual(GunFiringStatus.Okay, status.Status);
            Assert.IsTrue(status.CanFire());

            Assert.DoesNotThrow(() => status.Shoot(enemy, combatStats));
        }

        #endregion Fix Misfire

        #region Shoot

        private void Test_Shoot_NullArgs()
        {
            GunStatus status = new GunStatus(always1, blunderbuss);
            Assert.Throws<ArgumentNullException>(() => status.Shoot(null, combatStats));
            Assert.Throws<ArgumentNullException>(() => status.Shoot(enemy, null));
        }

        private void Test_Shoot_CannotFire()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            status.Shoot(enemy, combatStats);   //shoot, will misfire

            Assert.Throws<ArgumentException>(() => status.Shoot(enemy, combatStats));   //shoot, will misfire
            Assert.AreEqual(GunFiringStatus.Misfired, status.Status);

            status.FixMisfire(0);   //try fix, will get broke
            Assert.Throws<ArgumentException>(() => status.Shoot(enemy, combatStats));
            Assert.AreEqual(GunFiringStatus.Broken, status.Status);
        }

        private void Test_Shoot_NoShotLoaded()
        {
            GunStatus status = new GunStatus(always1, palmPistol);
            status.Shoot(enemy, combatStats);

            Assert.Throws<ArgumentException>(() => status.Shoot(enemy, combatStats));
            Assert.IsTrue(!status.HasShotLoaded());
        }

        private void Test_Shoot_Misfire()
        {
            GunStatus status = new GunStatus(always1, pepperbox);
            AttackSummary summary = null;
            Assert.DoesNotThrow(() => summary = status.Shoot(enemy, combatStats));

            Assert.AreEqual(GunFiringStatus.Misfired, status.Status);
            Assert.IsTrue(!status.CanFire());
            Assert.IsNotNull(summary);
        }

        private void Test_Shoot_SuccessiveShootsDoNotChangeMods()
        {
            GunStatus status = new GunStatus(always20, pepperbox);

            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);
            status.Shoot(enemy, combatStats);

            Assert.AreEqual(0, pepperbox.HitModifier.Get());
        }

        private void Test_Shoot_Miss()
        {
            GunStatus status = new GunStatus(always3, pepperbox);
            status.Shoot(enemy, combatStats);
            AttackSummary summary = null;
            Assert.DoesNotThrow(() => summary = status.Shoot(enemy, combatStats));

            Assert.AreEqual(GunFiringStatus.Okay, status.Status);
            Assert.IsTrue(status.CanFire());

            Assert.IsNotNull(summary);
            Assert.AreEqual(6, summary.AttackRoll);
            Assert.AreEqual(0, summary.Damage);
            Assert.IsTrue(!summary.Hit);
            Assert.IsTrue(!summary.Crit);
        }

        private void Test_Shoot_Hit()
        {
            GunStatus status = new GunStatus(always10, pepperbox);
            status.Shoot(enemy, combatStats);
            AttackSummary summary = null;
            Assert.DoesNotThrow(() => summary = status.Shoot(enemy, combatStats));

            Assert.AreEqual(GunFiringStatus.Okay, status.Status);
            Assert.IsTrue(status.CanFire());

            Assert.IsNotNull(summary);
            Assert.AreEqual(13, summary.AttackRoll);
            Assert.AreEqual(12, summary.Damage);
            Assert.IsTrue(summary.Hit);
            Assert.IsTrue(!summary.Crit);
        }

        private void Test_Shoot_Crit()
        {
            GunStatus status = new GunStatus(always20, pepperbox);
            status.Shoot(enemy, combatStats);
            AttackSummary summary = null;
            Assert.DoesNotThrow(() => summary = status.Shoot(enemy, combatStats));

            Assert.AreEqual(GunFiringStatus.Okay, status.Status);
            Assert.IsTrue(status.CanFire());

            Assert.IsNotNull(summary);
            Assert.AreEqual(23, summary.AttackRoll);
            Assert.AreEqual(42, summary.Damage);    //dmg kinda wonky because of always20
            Assert.IsTrue(summary.Hit);
            Assert.IsTrue(summary.Crit);
        }

        #endregion Shoot

        #region Cost

        private void Test_Cost_SingleShot()
        {
            GunStatus palmPistolStatus = new GunStatus(always10, palmPistol);
            GunStatus artificerPalmPistolStatus = new GunStatus(always10, artificerPalmPistol);
            GunStatus pistolStatus = new GunStatus(always10, pistol);
            GunStatus musketStatus = new GunStatus(always10, musket);
            GunStatus pepperboxStatus = new GunStatus(always10, pepperbox);
            GunStatus blunderbussStatus = new GunStatus(always10, blunderbuss);

            Assert.AreEqual(0, palmPistolStatus.Cost);
            Assert.AreEqual(0, artificerPalmPistolStatus.Cost);
            Assert.AreEqual(0, pistolStatus.Cost);
            Assert.AreEqual(0, musketStatus.Cost);
            Assert.AreEqual(0, pepperboxStatus.Cost);
            Assert.AreEqual(0, blunderbussStatus.Cost);

            palmPistolStatus.Shoot(enemy, combatStats);
            artificerPalmPistolStatus .Shoot(enemy, combatStats);
            pistolStatus.Shoot(enemy, combatStats);
            musketStatus.Shoot(enemy, combatStats);
            pepperboxStatus.Shoot(enemy, combatStats);
            blunderbussStatus.Shoot(enemy, combatStats);

            Assert.AreEqual(10, palmPistolStatus.Cost);
            Assert.AreEqual(0, artificerPalmPistolStatus.Cost);
            Assert.AreEqual(20, pistolStatus.Cost);
            Assert.AreEqual(25, musketStatus.Cost);
            Assert.AreEqual(20, pepperboxStatus.Cost);
            Assert.AreEqual(100, blunderbussStatus.Cost);
        }

        private void Test_Cost_MultipleShots()
        {
            GunStatus palmPistolStatus = new GunStatus(always10, palmPistol);
            GunStatus artificerPalmPistolStatus = new GunStatus(always10, artificerPalmPistol);
            GunStatus pistolStatus = new GunStatus(always10, pistol);
            GunStatus musketStatus = new GunStatus(always10, musket);
            GunStatus pepperboxStatus = new GunStatus(always10, pepperbox);
            GunStatus blunderbussStatus = new GunStatus(always10, blunderbuss);

            MultipleFire(enemy, combatStats, palmPistolStatus, 10); //clip size 1 -> 10 rnds

            for (int i = 0; i < 10; i++)    //Never reloads!
            {
                artificerPalmPistolStatus.Shoot(enemy, combatStats);
            }

            MultipleFire(enemy, combatStats, pistolStatus, 10); //clip size 4 -> 40 rnds
            MultipleFire(enemy, combatStats, musketStatus, 10); //clip size 1 -> 10 rnds
            MultipleFire(enemy, combatStats, pepperboxStatus, 10);  //clip size 6 -> 60 rnds
            MultipleFire(enemy, combatStats, blunderbussStatus, 10);    //clip size 1 -> 10 rnds

            Assert.AreEqual(100, palmPistolStatus.Cost);
            Assert.AreEqual(0, artificerPalmPistolStatus.Cost); //free ammo :D
            Assert.AreEqual(800, pistolStatus.Cost);
            Assert.AreEqual(250, musketStatus.Cost);
            Assert.AreEqual(1200, pepperboxStatus.Cost);
            Assert.AreEqual(1000, blunderbussStatus.Cost);
        }

        private void Test_Cost_SingleShotMiss()
        {
            Rng always3 = new Rng(3);
            enemy = new Enemy(30);
            GunStatus palmPistolStatus = new GunStatus(always3, palmPistol);
            GunStatus artificerPalmPistolStatus = new GunStatus(always3, artificerPalmPistol);
            GunStatus pistolStatus = new GunStatus(always3, pistol);
            GunStatus musketStatus = new GunStatus(always3, musket);
            GunStatus pepperboxStatus = new GunStatus(always3, pepperbox);
            GunStatus blunderbussStatus = new GunStatus(always3, blunderbuss);

            Assert.AreEqual(0, palmPistolStatus.Cost);
            Assert.AreEqual(0, artificerPalmPistolStatus.Cost);
            Assert.AreEqual(0, pistolStatus.Cost);
            Assert.AreEqual(0, musketStatus.Cost);
            Assert.AreEqual(0, pepperboxStatus.Cost);
            Assert.AreEqual(0, blunderbussStatus.Cost);

            palmPistolStatus.Shoot(enemy, combatStats);
            artificerPalmPistolStatus .Shoot(enemy, combatStats);
            pistolStatus.Shoot(enemy, combatStats);
            musketStatus.Shoot(enemy, combatStats);
            pepperboxStatus.Shoot(enemy, combatStats);
            blunderbussStatus.Shoot(enemy, combatStats);

            Assert.AreEqual(10, palmPistolStatus.Cost);
            Assert.AreEqual(0, artificerPalmPistolStatus.Cost);
            Assert.AreEqual(20, pistolStatus.Cost);
            Assert.AreEqual(25, musketStatus.Cost);
            Assert.AreEqual(20, pepperboxStatus.Cost);
            Assert.AreEqual(100, blunderbussStatus.Cost);
        }

        private void Test_Cost_Broken()
        {
            GunStatus palmPistolStatus = new GunStatus(always1, palmPistol);
            GunStatus artificerPalmPistolStatus = new GunStatus(always1, artificerPalmPistol);
            GunStatus pistolStatus = new GunStatus(always1, pistol);
            GunStatus musketStatus = new GunStatus(always1, musket);
            GunStatus pepperboxStatus = new GunStatus(always1, pepperbox);
            GunStatus blunderbussStatus = new GunStatus(always1, blunderbuss);

            palmPistolStatus.Shoot(enemy, combatStats);
            artificerPalmPistolStatus.Shoot(enemy, combatStats);
            pistolStatus.Shoot(enemy, combatStats);
            musketStatus.Shoot(enemy, combatStats);
            pepperboxStatus.Shoot(enemy, combatStats);
            blunderbussStatus.Shoot(enemy, combatStats);

            palmPistolStatus.FixMisfire(0);
            artificerPalmPistolStatus.FixMisfire(0);
            pistolStatus.FixMisfire(0);
            musketStatus.FixMisfire(0);
            pepperboxStatus.FixMisfire(0);
            blunderbussStatus.FixMisfire(0);

            Assert.AreEqual(1260, palmPistolStatus.Cost);
            Assert.AreEqual(1250, artificerPalmPistolStatus.Cost);
            Assert.AreEqual(3770, pistolStatus.Cost);
            Assert.AreEqual(7525, musketStatus.Cost);
            Assert.AreEqual(6270, pepperboxStatus.Cost);
            Assert.AreEqual(7600, blunderbussStatus.Cost);
        }

        #endregion Cost

    }
}
