using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public interface IGun
    {
        ICollection<GunProperty> Properties { get; }
        int Reload { get; }
        int Misfire { get; }
        GunRange Range { get; }
        int GunCost { get; }
        int AmmoCost { get; }
        IModifier HitModifier { get; }
        IModifier DamageModifier { get; }
        ICollection<RollType> DamageDice { get; }
    }
}
