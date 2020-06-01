using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class MockTurnState : ITurnState
    {
        public List<TurnStateEnum> ReturnExecute { get; set; } = new List<TurnStateEnum>();

        public bool TurnComplete { get; set; }

        private int current;

        public MockTurnState()
        {
            current = 0;
        }

        public TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            if (ReturnExecute[current] == TurnStateEnum.End)
            {
                MockPlayerStatus mockPlayer = (MockPlayerStatus)player;
                mockPlayer.NumberOfTurnsPassed++;
                TurnComplete = true;
            }
            else
            {
                TurnComplete = false;
            }

            TurnStateEnum next = GetNext();

            return next;
        }

        private TurnStateEnum GetNext()
        {
            TurnStateEnum next = ReturnExecute[current];
            current = ((current + 1) % ReturnExecute.Count);
            return next;
        }
    }
}
