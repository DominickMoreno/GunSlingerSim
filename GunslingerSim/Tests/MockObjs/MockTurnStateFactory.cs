using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class MockTurnStateFactory : ITurnStateFactory
    {
        public MockTurnState ReturnState { get; set; }
        public ITurnState Get(TurnStateEnum state)
        {
            return ReturnState;
        }
    }
}
