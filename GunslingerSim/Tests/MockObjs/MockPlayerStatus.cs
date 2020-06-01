using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class MockPlayerStatus : IPlayerStatus
    {
        public bool HasMisfireReaction { get; set; }

        public MagicInitiateSpell CurrentSpell { get; set; }

        public int NumberOfShots { get; set; }

        public int NumberOfHits { get; set; }

        public bool ActionAvailable { get; set; }

        public bool BonusActionAvailable { get; set; }

        public bool OffhandAttackAvailable { get; set; }

        public bool ActionSurgeAvailable { get; set; }

        public int NumberOfTurnsPassed { get; set; }
        public int NumberOfShotsLostToMisfire { get; set; } = 0;
        public int NumberOfCrits { get; set; }

        public IGunStatus MainHand { get; set; }

        public IList<IGunStatus> OffHands { get; set; }

        public IGunStatus CurrentOffHand { get; set; }

        public IPlayer PlayerBase { get; set; }

        public bool SetMainHandAttack { get; set; } = false;
        public bool SetFixMainHandMisfire { get; set; } = false;

        public void ActionSurge()
        {
            throw new NotImplementedException();
        }

        public bool CanSwapOffHand()
        {
            throw new NotImplementedException();
        }

        public ActionEconomy CastBuff(MagicInitiateSpell spell)
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }

        public bool FixMainHandMisfire()
        {
            return SetFixMainHandMisfire;
        }

        public bool MainHandAttack(IEnemy enemy)
        {
            enemy.TakeDamage(1);
            return SetMainHandAttack;
        }

        public void OffHandAttack(IEnemy enemy)
        {
            throw new NotImplementedException();
        }

        public bool SwapOffHand()
        {
            throw new NotImplementedException();
        }
    }
}
