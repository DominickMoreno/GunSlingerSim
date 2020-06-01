using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class FirstTurnSpellBuffEventUnitTest : BaseEventUnitTest
    {
        #region Setup

        public FirstTurnSpellBuffEventUnitTest()
        {
            AddTest(nameof(Test_Constructor), Test_Constructor);
            AddTest(nameof(Test_Execute_NullArgs), Test_Execute_NullArgs);
            AddTest(nameof(Test_Execute_NotFirstTurn), Test_Execute_NotFirstTurn);
            AddTest(nameof(Test_Execute_CurrentConcentrating), Test_Execute_CurrentConcentrating);
            AddTest(nameof(Test_Execute_Bless), Test_Execute_Bless);
            AddTest(nameof(Test_Execute_Hex), Test_Execute_Hex);
        }

        protected override void OneTimeSetup()
        {
            base.OneTimeSetup();
        }

        protected override void Setup()
        {
            base.Setup();
            turnEvent = new FirstTurnSpellBuffEvent();
        }

        protected override void TearDown()
        {
            base.TearDown();
            turnEvent = null;
        }

        #endregion Setup

        private void Test_Constructor()
        {
            Assert.DoesNotThrow(() => new FirstTurnSpellBuffEvent());
        }

        private void Test_Execute_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(null, enemy));
            Assert.Throws<ArgumentNullException>(() => turnEvent.Execute(status, null));
        }

        private void Test_Execute_NotFirstTurn()
        {
            status.EndTurn();

            for (int i = 0; i < 10; i++)
            {
                Assert.Throws<ArgumentException>(() => turnEvent.Execute(status, enemy));
                status.EndTurn();
            }
        }

        private void Test_Execute_CurrentConcentrating()
        {
            status.CastBuff(MagicInitiateSpell.Hex);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.AreEqual(TurnStateEnum.Action, ret);
        }

        private void Test_Execute_Bless()
        {
            List<MagicInitiateSpell> bless = new List<MagicInitiateSpell>() { MagicInitiateSpell.Bless };
            Player player = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, bless);
            PlayerStatus status = new PlayerStatus(always10, player);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.IsTrue(status.BonusActionAvailable);
            Assert.IsTrue(!status.ActionAvailable);
            Assert.AreEqual(TurnStateEnum.ActionSurge, ret);
        }

        private void Test_Execute_Hex()
        {
            List<MagicInitiateSpell> hex = new List<MagicInitiateSpell>() { MagicInitiateSpell.Hex };
            Player player = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, hex);
            PlayerStatus status = new PlayerStatus(always10, player);

            Assert.DoesNotThrow(() => ret = turnEvent.Execute(status, enemy));
            Assert.IsTrue(!status.BonusActionAvailable);
            Assert.IsTrue(status.ActionAvailable);
            Assert.AreEqual(TurnStateEnum.Action, ret);
        }
    }
}
