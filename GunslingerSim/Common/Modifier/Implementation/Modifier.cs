using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Common.Modifier
{
    public class Modifier : IModifier
    {
        public int BaseMod { get; private set; }
        public ICollection<RollType> Rolls { get; private set; }

        public Rng Rng { get; private set; }

        public Modifier()
        {
            Init(null, new List<RollType>(), 0);
        }

        public Modifier(int baseMod)
        {
            Init(null, new List<RollType>(), baseMod);
        }

        public Modifier(Rng rng, List<RollType> rolls, int baseMod)
        {
            Init(rng, rolls, baseMod);
        }

        public IModifier Add(IModifier other)
        {
            Assert.IsNotNull(other);
            int sumBase = BaseMod + other.BaseMod;
            List<RollType> sumRolls = Rolls.ToList();
            Rng rngToUse = Rng ?? other.Rng;
            sumRolls.AddRange(other.Rolls);

            return new Modifier(rngToUse, sumRolls, sumBase);
        }

        public int Get()
        {
            return BaseMod + GetRolls();
        }

        public int GetCrit()
        {
            return BaseMod + GetCritRolls();
        }

        private void Init(Rng rng,
                          ICollection<RollType> rolls,
                          int baseMod)
        {
            ValidateInput(rng, rolls);

            Rng = rng;
            Rolls = rolls;
            BaseMod = baseMod;
        }

        private void ValidateInput(Rng rng,
                                   ICollection<RollType> rolls)
        {
            Assert.HasNoNullEntries(rolls);
            if (rolls.Any())
            {
                Assert.IsNotNull(rng);

                foreach (RollType roll in rolls)
                {
                    Assert.ValidEnum(roll);
                }
            }
        }

        private int GetRolls()
        {
            return Rolls.Select(x => Rng.Roll(x)).Sum();
        }

        private int GetCritRolls()
        {
            return Rolls.Select(x => Rng.Roll(x) + Rng.Roll(x)).Sum();
        }
    }
}
