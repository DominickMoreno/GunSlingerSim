using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Tests
{
    public class GunFactoryUnitTest : BaseUnitTest
    {
        #region Setup

        private GunFactory factory;

        public GunFactoryUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);

            AddTest(nameof(Test_Get_PalmPistol), Test_Get_PalmPistol);
            AddTest(nameof(Test_Get_Pistol), Test_Get_Pistol);
            AddTest(nameof(Test_Get_Musket), Test_Get_Musket);
            AddTest(nameof(Test_Get_Pepperbox), Test_Get_Pepperbox);
            AddTest(nameof(Test_Get_Blunderbuss), Test_Get_Blunderbuss);
            AddTest(nameof(Test_Get_PalmPistolArtificerReload), Test_Get_PalmPistolArtificerReload);
        }

        protected override void OneTimeSetup()
        {
            factory = new GunFactory();
        }

        #endregion Setup

        #region Constructor

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new GunFactory());
        }

        #endregion Constructor

        #region Get

        private void Test_Get_PalmPistol()
        {
            IGun gun = null;
            Assert.DoesNotThrow(() => gun = factory.Get(GunType.PalmPistol));

            Assert.IsNotNull(gun);
            Assert.AreEqual(1, gun.DamageDice.Count);
            Assert.AreEqual(RollType.d8, gun.DamageDice.First());
            Assert.AreEqual(1, gun.Properties.Count);
            Assert.AreEqual(GunProperty.Light, gun.Properties.First());
            Assert.AreEqual(1, gun.Reload);
            Assert.AreEqual(1, gun.Misfire);
            Assert.AreEqual(0, gun.HitModifier.Get());
            Assert.AreEqual(0, gun.DamageModifier.Get());
        }

        private void Test_Get_Pistol()
        {
            IGun gun = null;
            Assert.DoesNotThrow(() => gun = factory.Get(GunType.Pistol));

            Assert.IsNotNull(gun);
            Assert.AreEqual(1, gun.DamageDice.Count);
            Assert.AreEqual(RollType.d10, gun.DamageDice.First());
            Assert.AreEqual(1, gun.Properties.Count);
            Assert.AreEqual(GunProperty.None, gun.Properties.First());
            Assert.AreEqual(4, gun.Reload);
            Assert.AreEqual(1, gun.Misfire);
            Assert.AreEqual(0, gun.HitModifier.Get());
            Assert.AreEqual(0, gun.DamageModifier.Get());
        }

        private void Test_Get_Musket()
        {
            IGun gun = null;
            Assert.DoesNotThrow(() => gun = factory.Get(GunType.Musket));

            Assert.IsNotNull(gun);
            Assert.AreEqual(1, gun.DamageDice.Count);
            Assert.AreEqual(RollType.d12, gun.DamageDice.First());
            Assert.AreEqual(1, gun.Properties.Count);
            Assert.AreEqual(GunProperty.TwoHand, gun.Properties.First());
            Assert.AreEqual(1, gun.Reload);
            Assert.AreEqual(2, gun.Misfire);
            Assert.AreEqual(0, gun.HitModifier.Get());
            Assert.AreEqual(0, gun.DamageModifier.Get());
        }

        private void Test_Get_Pepperbox()
        {
            IGun gun = null;
            Assert.DoesNotThrow(() => gun = factory.Get(GunType.Pepperbox));

            Assert.IsNotNull(gun);
            Assert.AreEqual(1, gun.DamageDice.Count);
            Assert.AreEqual(RollType.d10, gun.DamageDice.First());
            Assert.AreEqual(1, gun.Properties.Count);
            Assert.AreEqual(GunProperty.None, gun.Properties.First());
            Assert.AreEqual(6, gun.Reload);
            Assert.AreEqual(2, gun.Misfire);
            Assert.AreEqual(0, gun.HitModifier.Get());
            Assert.AreEqual(0, gun.DamageModifier.Get());
        }

        private void Test_Get_Blunderbuss()
        {
            IGun gun = null;
            Assert.DoesNotThrow(() => gun = factory.Get(GunType.Blunderbuss));

            Assert.IsNotNull(gun);
            Assert.AreEqual(2, gun.DamageDice.Count);
            Assert.AreEqual(2, gun.DamageDice
                                  .Where(x => RollType.d8 == x)
                                  .Count());
            Assert.AreEqual(1, gun.Properties.Count);
            Assert.AreEqual(GunProperty.None, gun.Properties.First());
            Assert.AreEqual(1, gun.Reload);
            Assert.AreEqual(2, gun.Misfire);
            Assert.AreEqual(0, gun.HitModifier.Get());
            Assert.AreEqual(0, gun.DamageModifier.Get());
        }

        private void Test_Get_PalmPistolArtificerReload()
        {
            IGun gun = null;
            Assert.DoesNotThrow(() => gun = factory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty));

            Assert.IsNotNull(gun);
            Assert.AreEqual(1, gun.DamageDice.Count);
            Assert.AreEqual(RollType.d8, gun.DamageDice.First());
            Assert.AreEqual(1, gun.Properties.Count);
            Assert.AreEqual(GunProperty.Light, gun.Properties.First());
            Assert.AreEqual(CommonConstants.InfiniteAmmo, gun.Reload);
            Assert.AreEqual(1, gun.Misfire);
            Assert.AreEqual(1, gun.HitModifier.Get());
            Assert.AreEqual(1, gun.DamageModifier.Get());
        }

        #endregion Get
    }
}
