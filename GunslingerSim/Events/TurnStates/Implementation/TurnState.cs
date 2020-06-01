using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Events
{
    public abstract class TurnState : ITurnState
    {
        public abstract TurnStateEnum Execute(IPlayerStatus player, IEnemy enemy);
        public virtual bool TurnComplete { get; protected set; }

        public TurnState()
        {
            TurnComplete = false;
        }

        protected void Validate(IPlayerStatus player, IEnemy enemy)
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(enemy);
        }

        protected bool IsBroken(IGunStatus gun)
        {
            return gun.Status == GunFiringStatus.Broken;
        }

        protected bool IsMisfired(IGunStatus gun)
        {
            return gun.Status == GunFiringStatus.Misfired;
        }
    }
}
