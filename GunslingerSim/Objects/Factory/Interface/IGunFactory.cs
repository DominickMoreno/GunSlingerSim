using GunslingerSim.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects.Factory
{
    public interface IGunFactory
    {
        IGun Get(GunType type);
        IGun Get(GunType type, WeaponTier weaponTier);

    }
}
