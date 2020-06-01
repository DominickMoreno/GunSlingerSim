using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public class EndTurnEvent : TurnState
    {
        public override TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            Validate(player, enemy);
            player.EndTurn();
            TurnComplete = true;
            return TurnStateEnum.Start;
        }
        
    }
}
