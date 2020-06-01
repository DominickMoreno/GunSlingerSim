using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    class ActionSurgeEvent : TurnState
    {
        public override TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            Validate(player, enemy);
            ValidatePlayerState(player);
            player.ActionSurge();
            return TurnStateEnum.Action;
        }

        private void ValidatePlayerState(IPlayerStatus player)
        {
            Assert.IsTrue(player.ActionSurgeAvailable);
            Assert.IsTrue(!player.ActionAvailable);
        }
    }
}
