using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public interface IGunStatus
    {
        int CurrentAmmo { get; }
        GunFiringStatus Status { get; }
        int UniqueId { get; }
        int Cost { get; }   //In copper

        bool HasShotLoaded();
        bool CanFire();
        void ReloadChamber();
        bool FixMisfire(int tinkerToolProficiency);

        AttackSummary Shoot(IEnemy enemy, CombatStats combatStats);

        bool Equals(IGunStatus other);

    }
}
