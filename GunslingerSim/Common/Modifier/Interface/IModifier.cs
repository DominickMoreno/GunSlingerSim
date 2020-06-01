using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Modifier
{
    public interface IModifier
    {
        int BaseMod { get; }
        ICollection<RollType> Rolls { get; }
        Rng Rng { get; }

        IModifier Add(IModifier other);
        int Get();
        int GetCrit();
    }
}
