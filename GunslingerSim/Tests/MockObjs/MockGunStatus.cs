using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class MockGunStatus : IGunStatus
    {
        public int CurrentAmmo { get; set; }

        public GunFiringStatus Status { get; set; } = GunFiringStatus.Okay;

        public int Cost { get; set; }   //In copper
        public int UniqueId { get; set; }

        public bool CanFire()
        {
            throw new NotImplementedException();
        }

        public bool Equals(IGunStatus other)
        {
            throw new NotImplementedException();
        }

        public bool FixMisfire(int tinkerToolProficiency)
        {
            throw new NotImplementedException();
        }

        public bool HasShotLoaded()
        {
            throw new NotImplementedException();
        }

        public void ReloadChamber()
        {
            throw new NotImplementedException();
        }

        public AttackSummary Shoot(IEnemy enemy, CombatStats combatStats)
        {
            throw new NotImplementedException();
        }
    }
}
