using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Modifier;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Tests
{
    public class MockPlayer : IPlayer
    {
        public IModifier HitModifier { get; set; } = new Modifier();

        public IModifier MainHandDamageModifier { get; set; } = new Modifier();

        public IModifier OffHandDamageModifier { get; set; } = new Modifier();

        public IGun MainHand { get; set; }

        public IList<IGun> OffHands { get; set; } = new List<IGun>();

        public int NumberOfAttacks { get; set; }

        public int Proficiency { get; set; }

        public int CritValue { get; set; }

        public bool CanCastBuff(MagicInitiateSpell spell)
        {
            throw new NotImplementedException();
        }

        public IPlayerStatus GetStatus()
        {
            throw new NotImplementedException();
        }
    }
}
