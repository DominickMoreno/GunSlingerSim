using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class ActionEventUnitTest : BaseEventUnitTest
    {
        #region Setup

        private MockPlayerStatus mockStatus;
        private MockGunStatus mockGun;
        private MockPlayer mockPlayer;

        public ActionEventUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);
            AddTest(nameof(Test_Execute_NullArgs), Test_Execute_NullArgs);
            AddTest(nameof(Test_Execute_GunBroken), Test_Execute_GunBroken);
            AddTest(nameof(Test_Execute_FixMisfire), Test_Execute_FixMisfire);
            AddTest(nameof(Test_Execute_AttackReturnsOhAvail), Test_Execute_AttackReturnsOhAvail);
            AddTest(nameof(Test_Execute_AttackReturnsEnd), Test_Execute_AttackReturnsEnd);
        }

        protected override void OneTimeSetup()
        {
            base.OneTimeSetup();
        }

        protected override void Setup()
        {
            base.Setup();
            turnEvent = new ActionEvent();
            mockGun = new MockGunStatus();
            mockPlayer = new MockPlayer();
            mockStatus = new MockPlayerStatus()
            {
                MainHand = mockGun,
                PlayerBase = mockPlayer
            };
        }

        protected override void TearDown()
        {
            base.TearDown();
            turnEvent = null;
            mockStatus = null;
            mockGun = null;
        }

        #endregion Setup

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new ActionEvent());
        }

        private void Test_Execute_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(null, enemy));
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(mockStatus, null));
        }

        private void Test_Execute_GunBroken()
        {
            mockGun.Status = GunFiringStatus.Broken;

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(mockStatus, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(0, enemy.DamageTaken);
        }

        private void Test_Execute_FixMisfire()
        {
            mockGun.Status = GunFiringStatus.Misfired;

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(mockStatus, enemy));
            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(0, enemy.DamageTaken);
            

        }

        private void Test_Execute_AttackReturnsOhAvail()
        {
            mockStatus.SetMainHandAttack = true;
            Assert.DoesNotThrow(() => ret = turnEvent.Execute(mockStatus, enemy));

            Assert.AreEqual(TurnStateEnum.OffHandAttack, ret);
            Assert.AreEqual(1, enemy.DamageTaken);
        }

        private void Test_Execute_AttackReturnsEnd()
        {
            Assert.DoesNotThrow(() => ret = turnEvent.Execute(mockStatus, enemy));

            Assert.AreEqual(TurnStateEnum.End, ret);
            Assert.AreEqual(1, enemy.DamageTaken);
        }
    }
}
