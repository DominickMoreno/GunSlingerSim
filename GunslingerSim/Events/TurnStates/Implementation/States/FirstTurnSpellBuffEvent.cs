using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    class FirstTurnSpellBuffEvent : TurnState
    {
        private static Dictionary<ActionEconomy, TurnStateEnum> ActionUsedToTurnStateEnumMap = new Dictionary<ActionEconomy, TurnStateEnum>()
        {
            { ActionEconomy.Action, TurnStateEnum.ActionSurge },
            { ActionEconomy.BonusAction, TurnStateEnum.Action },
            { ActionEconomy.FreeAction, TurnStateEnum.Action },
            { ActionEconomy.MoveAction, TurnStateEnum.Action },
            { ActionEconomy.StowAction, TurnStateEnum.Action },
        };

        public FirstTurnSpellBuffEvent()
        {
            //Empty
        }

        public override TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy)
        {
            Validate(player, enemy);
            Assert.AreEqual(0, player.NumberOfTurnsPassed);

            ActionEconomy actionUsed = ActionEconomy.FreeAction;
            if (!CurrentlyConcentrating(player))
            {
                MagicInitiateSpell spell = DetermineSpellToCast(player.PlayerBase);
                if (spell != MagicInitiateSpell.None)
                {
                    actionUsed = player.CastBuff(spell);
                }
            }

            return ActionUsedToTurnStateEnumMap[actionUsed];
        }

        private MagicInitiateSpell DetermineSpellToCast(IPlayer player)
        {
            MagicInitiateSpell spell = MagicInitiateSpell.None;

            if (player.CanCastBuff(MagicInitiateSpell.Bless))
            {
                spell = MagicInitiateSpell.Bless;
            }
            else if (player.CanCastBuff(MagicInitiateSpell.Hex))
            {
                spell = MagicInitiateSpell.Hex;
            }

            return spell;
        }

        private bool CurrentlyConcentrating(IPlayerStatus player)
        {
            return player.CurrentSpell != MagicInitiateSpell.None;
        }
    }
}
