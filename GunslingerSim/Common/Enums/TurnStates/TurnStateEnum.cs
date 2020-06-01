using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Enums
{
    public enum TurnStateEnum
    {
        Error = 0,
        Start = 1,
        FirstTurnSpellBuff = 2,
        ActionSurge = 3,
        Action = 4,
        OffHandAttack = 5,
        End = 6
    }
}
