using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class OffHandAttackEventUnitTest : BaseEventUnitTest
    {
        #region Setup

        public OffHandAttackEventUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);
            AddTest(nameof(Test_Execute_NullArgs), Test_Execute_NullArgs);
            AddTest(nameof(Test_Execute_CantFire_OhAttackNotAvail), Test_Execute_CantFire_OhAttackNotAvail);
            AddTest(nameof(Test_Execute_CantFire_BonusActionNotAvail), Test_Execute_CantFire_BonusActionNotAvail);
            AddTest(nameof(Test_Execute_CantFire_NoOh), Test_Execute_CantFire_NoOh);
            AddTest(nameof(Test_Execute_CantFire_Misfired), Test_Execute_CantFire_Misfired);
            AddTest(nameof(Test_Execute_CantFire_Broken), Test_Execute_CantFire_Broken);
            AddTest(nameof(Test_Execute_CantFire_BrokenSwap), Test_Execute_CantFire_BrokenSwap);
            AddTest(nameof(Test_Execute_CantFire_OneOh), Test_Execute_CantFire_OneOh);
            AddTest(nameof(Test_Execute_CantFire_OtherOhOutOfAmmo), Test_Execute_CantFire_OtherOhOutOfAmmo);
        }

        protected override void OneTimeSetup()
        {
            base.OneTimeSetup();
        }

        protected override void Setup()
        {
            base.Setup();
            turnEvent = new OffHandAttackEvent();
        }

        protected override void TearDown()
        {
            base.TearDown();
            turnEvent = null;
        }

        #endregion Setup

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new OffHandAttackEvent());
        }

        private void Test_Execute_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(null, enemy));
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(status, null));
        }

        private void Test_Execute_CantFire_OhAttackNotAvail()
        {
            Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
            Assert.AreEqual(0, status.NumberOfShots);
        }

        private void Test_Execute_CantFire_BonusActionNotAvail()
        {
            status.CastBuff(MagicInitiateSpell.Hex);
            status.MainHandAttack(enemy);

            Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
            Assert.AreEqual(3, status.NumberOfShots);   //didn't fire OH shot
        }

        private void Test_Execute_CantFire_NoOh()
        {
            player = new Player(always10, 11, 11, FightingStyle.Archery, mh, new List<IGun>(), feats, buffs);
            status = new PlayerStatus(always10, player);

            status.MainHandAttack(enemy);
            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(3, status.NumberOfShots);   //didn't fire OH shot
        }

        private void Test_Execute_CantFire_Misfired()
        {
            //Action: atk (3) atk (3) atk (3) -> BA (misfire) (1)
            List<int> sequence = new List<int>() { 3, 3, 3, 1};
            Rng sequenceRng = new MockRng(sequence);
            List<IGun> singleOh = new List<IGun>() { gunFactory.Get(GunType.PalmPistol) };

            player = new Player(sequenceRng, 11, 11, FightingStyle.Archery, mh, singleOh, feats, buffs);
            status = new PlayerStatus(sequenceRng, player);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(7, status.NumberOfShots);   //Didn't do second OH shot
            Assert.IsTrue(status.BonusActionAvailable);
        }

        private void Test_Execute_CantFire_Broken()
        {
            //Action: atk (3) hit (1) atk (3) hit (1) atk (3) hit (1) -> BA (misfire) (1) -> try to fix with reaction (fail) (1)
            List<int> sequence = new List<int>() { 3, 1, 3, 1, 3, 1,
                                                   1, 1};
            Rng sequenceRng = new MockRng(sequence);
            List<IGun> singleArtificerOh = new List<IGun>() { gunFactory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty) };

            player = new Player(sequenceRng, 11, 11, FightingStyle.Archery, mh, singleArtificerOh, feats, buffs);
            status = new PlayerStatus(sequenceRng, player);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(7, status.NumberOfShots);   //Didn't do second OH shot
            Assert.IsTrue(status.BonusActionAvailable);
        }

        private void Test_Execute_CantFire_BrokenSwap()
        {
            //Action: atk (3) hit (1) atk (3) hit (1) atk (3) hit (1) -> BA (misfire) (1) -> try to fix with reaction (fail) (1)
            //turn 2: atk (3) hit (1) atk (3) hit (1) atk (3) hit (1) -> BA atk (2) (doesn't roll bc OH) ->
            //turn 3: Reload, atk (3) hit (1) atk (3) hit (1) -> BA cannot swap to OH because misfire
            List<int> sequence = new List<int>() { 3, 1, 3, 1, 3, 1,
                                                   1, 1,
                                                   3, 1, 3, 1, 3, 1,
                                                   2,
                                                   3, 1, 3, 1 };
            Rng sequenceRng = new MockRng(sequence);
            List<IGun> singleArtificerOh = new List<IGun>() { gunFactory.Get(GunType.PalmPistol, WeaponTier.ArtificerReloadProperty),
                                                              gunFactory.Get(GunType.PalmPistol) };

            player = new Player(sequenceRng, 11, 11, FightingStyle.Archery, mh, singleArtificerOh, feats, buffs);
            status = new PlayerStatus(sequenceRng, player);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            Assert.AreEqual(GunFiringStatus.Broken, status.CurrentOffHand.Status);
            status.EndTurn();
            status.MainHandAttack(enemy);
            status.SwapOffHand();
            Assert.AreEqual(GunFiringStatus.Okay, status.CurrentOffHand.Status);
            status.OffHandAttack(enemy);
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
            Assert.IsTrue(!status.CanSwapOffHand());
            status.EndTurn();
            status.MainHandAttack(enemy);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(10, status.NumberOfShots);   //Didn't do second OH shot, had to reload
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(!status.CurrentOffHand.HasShotLoaded());
        }


        private void Test_Execute_CantFire_OneOh()
        {
            List<IGun> singleOh = new List<IGun>() { gunFactory.Get(GunType.PalmPistol) };
            player = new Player(always10, 11, 11, FightingStyle.Archery, mh, singleOh, feats, buffs);
            status = new PlayerStatus(always10, player);

            status.MainHandAttack(enemy);
            status.OffHandAttack(enemy);
            status.EndTurn();
            status.MainHandAttack(enemy);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(7, status.NumberOfShots);   //Didn't do second OH shot
            Assert.IsTrue(status.BonusActionAvailable);
        }

        private void Test_Execute_CantFire_OtherOhOutOfAmmo()
        {
            player = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            status = new PlayerStatus(always10, player);

            status.MainHandAttack(enemy);   //3
            status.OffHandAttack(enemy);    //1
            status.EndTurn();
            status.SwapOffHand();
            status.MainHandAttack(enemy);   //3
            status.OffHandAttack(enemy);    //1
            status.EndTurn();
            status.MainHandAttack(enemy);   //2

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(10, status.NumberOfShots);   //Didn't do third OH shot, minus reload
            Assert.IsTrue(status.BonusActionAvailable);
        }
    }
}
