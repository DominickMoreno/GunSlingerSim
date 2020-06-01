using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public interface ITurnStateMachine
    {
        void TakeTurn(IPlayerStatus status, IEnemy enemy);
    }
}
