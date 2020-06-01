using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public class StartTurnEvent : TurnState
    {
        public override TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            Validate(player, enemy);
            ValidatePlayerCanStartTurn(player);

            return player.NumberOfTurnsPassed == 0
                ? TurnStateEnum.FirstTurnSpellBuff
                : TurnStateEnum.Action;
        }

        private void ValidatePlayerCanStartTurn(IPlayerStatus player)
        {
            Assert.IsTrue(player.ActionAvailable);
            Assert.IsTrue(player.BonusActionAvailable);
        }
    }
}
