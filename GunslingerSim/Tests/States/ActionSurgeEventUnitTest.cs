using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class ActionSurgeEventUnitTest : BaseEventUnitTest
    {
        #region Setup

        public ActionSurgeEventUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);
            AddTest(nameof(Test_Execute_NullArgs), Test_Execute_NullArgs);
            AddTest(nameof(Test_Execute_ActionSurgeNotAvail), Test_Execute_ActionSurgeNotAvail);
            AddTest(nameof(Test_Execute_ActionStillAvail), Test_Execute_ActionStillAvail);
            AddTest(nameof(Test_Execute_SunnyDay), Test_Execute_SunnyDay);
        }

        protected override void OneTimeSetup()
        {
            base.OneTimeSetup();
        }

        protected override void Setup()
        {
            base.Setup();
            turnEvent = new ActionSurgeEvent();
        }

        protected override void TearDown()
        {
            base.TearDown();
            turnEvent = null;
        }

        #endregion Setup

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new ActionSurgeEvent());
        }

        private void Test_Execute_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(null, enemy));
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(status, null));
        }

        private void Test_Execute_ActionSurgeNotAvail()
        {
            status.MainHandAttack(enemy);
            status.ActionSurge();

            Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
        }

        private void Test_Execute_ActionStillAvail()
        {
            Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
        }

        private void Test_Execute_SunnyDay()
        {
            status.MainHandAttack(enemy);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.IsTrue(status.ActionAvailable);
            Assert.IsTrue(!status.ActionSurgeAvailable);
            Assert.AreEqual(TurnStateEnum.Action, ret);
        }
    }
}
