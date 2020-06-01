using GunslingerSim.Common.Enums;
using GunslingerSim.Events;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class BaseEventUnitTest : BaseUnitTest
    {
        #region Setup

        protected IEnemy enemy;
        protected IPlayer player;
        protected IPlayerStatus status;

        protected TurnState turnEvent;
        protected TurnStateEnum ret;

        protected override void Setup()
        {
            player = new Player(always10, 11, 11, FightingStyle.Archery, mh, ohs, feats, buffs);
            enemy = new Enemy(14);
            status = new PlayerStatus(always10, player);

            ret = (TurnStateEnum)(999);
        }

        protected override void TearDown()
        {
            player = null;
            enemy = null;
            status = null;
            turnEvent = null;
        }

        #endregion Setup
    }
}
