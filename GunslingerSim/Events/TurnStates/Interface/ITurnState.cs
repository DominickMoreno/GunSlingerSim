using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public interface ITurnState
    {
        bool TurnComplete { get; }
        TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy);
    }
}
