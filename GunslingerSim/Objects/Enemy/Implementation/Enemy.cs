using GunslingerSim.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public class Enemy : IEnemy
    {
        public int ArmorClass { get; }
        public int DamageTaken { get; private set; }
        public int HitsTaken { get; private set; }

        public Enemy(int armorClass)
        {
            Assert.IsTrue(armorClass > 0);

            ArmorClass = armorClass;
            DamageTaken = 0;
            HitsTaken = 0;
        }

        public IEnemy Copy()
        {
            return new Enemy(ArmorClass);
        }

        public bool TryHit(int hit)
        {
            bool ret = false;
            if (hit >= ArmorClass)
            {
                HitsTaken++;
                ret = true;
            }

            return ret;
        }

        public void CritHit()
        {
            HitsTaken++;
        }

        public void TakeDamage(int damage)
        {
            DamageTaken += damage;
        }
    }
}
