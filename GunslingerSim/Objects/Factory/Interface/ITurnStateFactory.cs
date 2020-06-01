using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects.Factory
{
    public interface ITurnStateFactory
    {
        ITurnState Get(TurnStateEnum state);
    }
}
