using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Events
{
    public class OffHandAttackEvent : TurnState
    {
        public override TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            Validate(player, enemy);
            Assert.IsTrue(player.BonusActionAvailable);
            Assert.IsTrue(player.OffhandAttackAvailable);

            bool hasValidOh = TrySetValidOffHand(player);
            if (hasValidOh)
            {
                player.OffHandAttack(enemy);
            }

            return TurnStateEnum.End;
        }

        private bool TrySetValidOffHand(IPlayerStatus player)
        {
            //If we don't have a currently valid offhand and a valid one exists (but is stowed), swap to it.
            //Note we only ever assume we swap OHs
            //TODO: multiple Main hands?
            bool hasValidOffHand = false;
            if (CanFireWithCurrentOffHand(player))
            {
                hasValidOffHand = true;
            }
            else if (player.CanSwapOffHand())
            {
                player.SwapOffHand();
                hasValidOffHand = true;
            }

            return hasValidOffHand;
        }

        private bool CanFireWithCurrentOffHand(IPlayerStatus player)
        {
            return player.CurrentOffHand != null &&
                   player.CurrentOffHand.CanFire() &&
                   player.CurrentOffHand.HasShotLoaded();
        }
    }
}
