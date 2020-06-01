using GunslingerSim.Common.Modifier;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Util
{
    public class CombatStats
    {
        public IModifier HitModifier { get; private set; }
        public IModifier DamageModifier { get; private set; }
        public int CritValue { get; private set; }

        public CombatStats(IModifier hitModifier,
                           IModifier damageModifier,
                           int critValue)
        {
            ValidateInput(hitModifier, damageModifier, critValue);

            HitModifier = hitModifier;
            DamageModifier = damageModifier;
            CritValue = critValue;
        }

        private void ValidateInput(IModifier hitModifier,
                                   IModifier damageModifier,
                                   int critValue)
        {
            Assert.IsNotNull(hitModifier);
            Assert.IsNotNull(damageModifier);
            Assert.IsTrue(critValue > 0);
            Assert.IsTrue(critValue <= 20);
        }
    }
}
