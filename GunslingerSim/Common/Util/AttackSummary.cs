using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Util
{
    public class AttackSummary
    {
        public int AttackRoll { get; }
        public int Damage { get; }
        public bool Hit { get; }
        public bool Crit { get; }

        public AttackSummary(int attackRoll, int damage, bool hit, bool crit)
        {
            Assert.IsTrue(damage >= 0);

            AttackRoll = attackRoll;
            Damage = damage;
            Hit = hit;
            Crit = crit;
        }
    }
}
