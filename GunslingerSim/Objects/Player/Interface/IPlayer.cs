using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public interface IPlayer
    {
        IModifier HitModifier { get; }
        IModifier MainHandDamageModifier { get; }
        IModifier OffHandDamageModifier { get; }
        IGun MainHand { get; }
        IList<IGun> OffHands { get; }
        int NumberOfAttacks { get; }
        int Proficiency { get; }
        int CritValue { get; }
        IPlayerStatus GetStatus();
        bool CanCastBuff(MagicInitiateSpell spell);
    }
}
