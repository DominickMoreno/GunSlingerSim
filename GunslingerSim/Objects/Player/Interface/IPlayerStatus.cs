using GunslingerSim.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public interface IPlayerStatus
    {
        bool HasMisfireReaction { get; }
        MagicInitiateSpell CurrentSpell { get; }
        int NumberOfShots { get; }
        int NumberOfHits { get; }
        bool ActionAvailable { get; }
        bool BonusActionAvailable { get; }
        bool OffhandAttackAvailable { get; }
        bool ActionSurgeAvailable { get; }
        int NumberOfTurnsPassed { get; }
        int NumberOfShotsLostToMisfire { get; }
        int NumberOfCrits { get; }

        IGunStatus MainHand { get; }
        IList<IGunStatus> OffHands { get; }
        IGunStatus CurrentOffHand { get; }
        IPlayer PlayerBase { get; }

        ActionEconomy CastBuff(MagicInitiateSpell spell);
        void ActionSurge();
        void EndTurn();
        bool MainHandAttack(IEnemy enemy);
        void OffHandAttack(IEnemy enemy);
        bool FixMainHandMisfire();
        bool CanSwapOffHand();
        bool SwapOffHand();
    }
}
