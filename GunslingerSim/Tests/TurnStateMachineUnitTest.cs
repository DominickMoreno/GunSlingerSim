using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;

namespace GunslingerSim.Tests
{
    public class TurnStateMachineUnitTest : BaseUnitTest
    {
        #region Setup

        private MockTurnState turnState;
        private TurnStateMachine stateMachine;
        private List<TurnStateEnum> states;
        private Player player;
        private MockPlayerStatus status;
        private Enemy enemy;
        private MockTurnStateFactory factory;

        public TurnStateMachineUnitTest()
        {
            AddTest(nameof(Test_Constructor_SunnyDay), Test_Constructor_SunnyDay);
            AddTest(nameof(Test_Constructor_NullArg), Test_Constructor_NullArg);

            AddTest(nameof(Test_TakeTurn_NullArgs), Test_TakeTurn_NullArgs);
            AddTest(nameof(Test_TakeTurn_OneTurn), Test_TakeTurn_OneTurn);
            AddTest(nameof(Test_TakeTurn_TenTurns), Test_TakeTurn_TenTurns);
        }

        protected override void OneTimeSetup()
        {
            states = new List<TurnStateEnum>()
            {
                TurnStateEnum.Start,
                TurnStateEnum.FirstTurnSpellBuff,
                TurnStateEnum.ActionSurge,
                TurnStateEnum.Action,
                TurnStateEnum.OffHandAttack,
                TurnStateEnum.End,
            };

        }

        protected override void Setup()
        {
            turnState = new MockTurnState()
            {
                ReturnExecute = states
            };

            factory = new MockTurnStateFactory()
            {
                ReturnState = turnState
            };

            stateMachine = new TurnStateMachine(factory);
            player = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            status = new MockPlayerStatus();
            enemy = new Enemy(14);
        }

        protected override void TearDown()
        {
            factory = null;
            turnState = null;
            stateMachine = null;
            enemy = null;
        }

        private void CompleteTurn(TurnStateMachine fsm,
                                  IPlayerStatus status,
                                  IEnemy enemy)
        {
            int currentTurn = status.NumberOfTurnsPassed;

            while (status.NumberOfTurnsPassed == currentTurn)
            {
                Assert.DoesNotThrow(() => fsm.TakeTurn(status, enemy));
            }
        }

        #endregion Setup

        #region Constructor

        private void Test_Constructor_NullArg()
        {
            Assert.Throws<ArgumentNullException>(() => new TurnStateMachine(null));
        }

        private void Test_Constructor_SunnyDay()
        {
            TurnStateMachine machine = null;
            Assert.DoesNotThrow(() => machine = new TurnStateMachine(factory));
            Assert.IsNotNull(machine);
        }

        #endregion Constructor

        #region Take Turn

        private void Test_TakeTurn_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => stateMachine.TakeTurn(null, enemy));
            Assert.Throws<ArgumentNullException>(() => stateMachine.TakeTurn(status, null));
        }

        private void Test_TakeTurn_OneTurn()
        {
            int numTurns = 1;

            for (int i = 0; i < numTurns; i++)
            {
                Assert.DoesNotThrow(() => stateMachine.TakeTurn(status, enemy));
                Assert.AreEqual(status.NumberOfTurnsPassed, (i + 1));
            }
        }

        private void Test_TakeTurn_TenTurns()
        {
            int numTurns = 10;

            for (int i = 0; i < numTurns; i++)
            {
                Assert.DoesNotThrow(() => stateMachine.TakeTurn(status, enemy));
                Assert.AreEqual(status.NumberOfTurnsPassed, (i + 1));
            }
        }

        #endregion Take Turn
    }
}
