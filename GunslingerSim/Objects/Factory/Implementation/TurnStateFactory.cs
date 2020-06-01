using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using System;

namespace GunslingerSim.Objects.Factory
{
    public class TurnStateFactory : ITurnStateFactory
    {
        public ITurnState Get(TurnStateEnum state)
        {
            Assert.ValidEnum(state);
            Assert.IsTrue(state != TurnStateEnum.Error);

            TurnState turnState = null;
            switch (state)
            {
                case TurnStateEnum.Start:
                {
                    turnState = new StartTurnEvent();
                    break;
                }
                case TurnStateEnum.FirstTurnSpellBuff:
                {
                    turnState = new FirstTurnSpellBuffEvent();
                    break;
                }
                case TurnStateEnum.ActionSurge:
                {
                    turnState = new ActionSurgeEvent();
                    break;
                }
                case TurnStateEnum.Action:
                {
                    turnState = new ActionEvent();
                    break;
                }
                case TurnStateEnum.OffHandAttack:
                {
                    turnState = new OffHandAttackEvent();
                    break;
                }
                case TurnStateEnum.End:
                {
                    turnState = new EndTurnEvent();
                    break;
                }
                default:
                {
                    throw new ArgumentException($"Invalid turn state {state}.");
                }
            }

            return turnState;
        }
    }
}
