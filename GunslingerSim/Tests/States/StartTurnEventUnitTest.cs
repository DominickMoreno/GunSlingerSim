using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class StartTurnEventUnitTest : BaseEventUnitTest
    {
        #region Setup

        public StartTurnEventUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);
            AddTest(nameof(Test_Execute_NullArgs), Test_Execute_NullArgs);
            AddTest(nameof(Test_Execute_ActionNotAvailable), Test_Execute_ActionNotAvailable);
            AddTest(nameof(Test_Execute_BonusActionNotAvailable), Test_Execute_BonusActionNotAvailable);
            AddTest(nameof(Test_Execute_FirstTurn), Test_Execute_FirstTurn);
            AddTest(nameof(Test_Execute_NotFirstTurn), Test_Execute_NotFirstTurn);
        }

        protected override void OneTimeSetup()
        {
            base.OneTimeSetup();
        }

        protected override void Setup()
        {
            base.Setup();
            turnEvent = new StartTurnEvent();
        }

        protected override void TearDown()
        {
            base.TearDown();
            turnEvent = null;
        }

        #endregion Setup

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new StartTurnEvent());
        }

        private void Test_Execute_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(status, null));
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(null, enemy));
        }

        private void Test_Execute_ActionNotAvailable()
        {
            status.MainHandAttack(enemy);
            Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
        }

        private void Test_Execute_BonusActionNotAvailable()
        {
            status.CastBuff(MagicInitiateSpell.Bless);
            Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
        }

        private void Test_Execute_FirstTurn()
        {
            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.FirstTurnSpellBuff, ret);
        }

        private void Test_Execute_NotFirstTurn()
        {
            status.EndTurn();

            for (int i = 0; i < 10; i++)
            {
                Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
                Assert.AreEqual(TurnStateEnum.Action, ret);
            }
        }
    }
}
