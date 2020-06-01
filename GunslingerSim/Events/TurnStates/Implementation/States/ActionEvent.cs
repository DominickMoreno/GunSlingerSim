using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public class ActionEvent : TurnState
    {
        public override TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            Validate(player, enemy);

            TurnStateEnum ret = TurnStateEnum.End;
            if (!IsBroken(player.MainHand))
            {
                ret = UseAction(player, enemy);
            }

            return ret;
        }

        private TurnStateEnum UseAction(IPlayerStatus player, IEnemy enemy)
        {
            TurnStateEnum ret = TurnStateEnum.End;
            if (IsMisfired(player.MainHand))
            {
                ret = UseActionFixMisfire(player);
            }
            else
            {
                ret = UseAttackAction(player, enemy);
            }

            return ret;
        }

        private TurnStateEnum UseActionFixMisfire(IPlayerStatus player)
        {
            bool success = player.FixMainHandMisfire();
            return TurnStateEnum.End;
        }

        private TurnStateEnum UseAttackAction(IPlayerStatus player, IEnemy enemy)
        {
            return player.MainHandAttack(enemy) &&
                   player.BonusActionAvailable
                ? TurnStateEnum.OffHandAttack
                : TurnStateEnum.End;
        }
    }
}
