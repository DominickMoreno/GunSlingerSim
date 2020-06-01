using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class EndTurnEventUnitTest : BaseEventUnitTest
    {
        #region Setup

        public EndTurnEventUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);
            AddTest(nameof(Test_Execute_NullArgs), Test_Execute_NullArgs);
            AddTest(nameof(Test_Execute), Test_Execute);
        }
        
        protected override void OneTimeSetup()
        {
            base.OneTimeSetup();
        }

        protected override void Setup()
        {
            base.Setup();
            turnEvent = new EndTurnEvent();
        }

        protected override void TearDown()
        {
            base.TearDown();
            turnEvent = null;
        }

        #endregion Setup

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new EndTurnEvent());
        }

        private void Test_Execute_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(null, enemy));
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(status, null));
        }

        private void Test_Execute()
        {
            int turnCount;
            for (int i = 0; i < 10; i++)
            {
                turnCount = status.NumberOfTurnsPassed;
                status.MainHandAttack(enemy);

                if (status.CurrentOffHand.CanFire() &&
                    status.CurrentOffHand.HasShotLoaded())
                {
                    status.OffHandAttack(enemy);
                }

                ret = (TurnStateEnum)(-400);
                Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
                Assert.IsTrue(status.ActionAvailable);
                Assert.IsTrue(status.BonusActionAvailable);
                Assert.AreEqual((turnCount + 1), status.NumberOfTurnsPassed);
                Assert.AreEqual(TurnStateEnum.Start, ret);
            }
        }
    }
}
