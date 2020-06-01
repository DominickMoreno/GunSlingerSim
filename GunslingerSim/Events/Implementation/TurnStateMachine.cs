using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using GunslingerSim.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public class TurnStateMachine : ITurnStateMachine
    {
        private ITurnState currentState;
        private Dictionary<TurnStateEnum, ITurnState> stateMachine;

        public TurnStateMachine(ITurnStateFactory factory)
        {
            Assert.IsNotNull(factory);
            stateMachine = InitStateMachine(factory);
            currentState = stateMachine[TurnStateEnum.Start];
        }

        public void TakeTurn(IPlayerStatus status, IEnemy enemy)    //TODO: ret?
        {
            //TODO: change this. Not sure what role I wnat to have vs the simulator
                //This completes a turn. Simulator aggregates per turn data? not sure
            Assert.IsNotNull(status);
            Assert.IsNotNull(enemy);

            TurnStateEnum nextState;
            do
            {
                nextState = currentState.Execute(status, enemy);
                currentState = stateMachine[nextState];
            }
            while (!currentState.TurnComplete);
        }

        private Dictionary<TurnStateEnum, ITurnState> InitStateMachine(ITurnStateFactory factory)
        {
            return new Dictionary<TurnStateEnum, ITurnState>()
            {
                {TurnStateEnum.Start, factory.Get(TurnStateEnum.Start) },
                {TurnStateEnum.FirstTurnSpellBuff, factory.Get(TurnStateEnum.FirstTurnSpellBuff) },
                {TurnStateEnum.Action, factory.Get(TurnStateEnum.Action) },
                {TurnStateEnum.ActionSurge, factory.Get(TurnStateEnum.ActionSurge) },
                {TurnStateEnum.OffHandAttack, factory.Get(TurnStateEnum.OffHandAttack) },
                {TurnStateEnum.End, factory.Get(TurnStateEnum.End) },
                //TODO: error?
            };
        }
    }
}
