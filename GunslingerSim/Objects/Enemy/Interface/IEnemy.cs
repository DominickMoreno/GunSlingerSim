using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Objects
{
    public interface IEnemy
    {
        int ArmorClass { get; }
        int DamageTaken { get; }
        int HitsTaken { get; }
        IEnemy Copy();
        bool TryHit(int hit);
        void CritHit();
        void TakeDamage(int damage);
    }
}
