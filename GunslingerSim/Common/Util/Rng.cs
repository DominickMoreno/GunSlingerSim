using GunslingerSim.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Util
{
    public class Rng
    {
        private static readonly int Minimum = 0;
        private static readonly int Maximum = 20;

        protected static Dictionary<RollType, int> RollTypeToDieMap = new Dictionary<RollType, int>()
        {
            { RollType.d4, 4 },
            { RollType.d6, 6 },
            { RollType.d8, 8 },
            { RollType.d10, 10 },
            { RollType.d12, 12 },
            { RollType.d20, 20 },
        };

        private Random rand;
        private int constant;

        public Rng()
        {
            rand = new Random();
            constant = 0;
        }

        public Rng(int constant)
        {
            Assert.IsTrue(constant >= Minimum);
            Assert.IsTrue(constant <= Maximum);

            rand = null;
            this.constant = constant;
        }

        public Rng Copy()
        {
            return constant == 0
                ? new Rng()
                : new Rng(constant);
        }

        public virtual int Roll(RollType roll)
        {
            ValidateInput(roll);

            return IsConstant()
                ? constant
                : GetRngRoll(roll);
        }

        private bool IsConstant()
        {
            return rand == null;
        }

        private int GetRngRoll(RollType roll)
        {
            int maxRoll = RollTypeToDieMap[roll];
            return (rand.Next() % maxRoll) + 1;
        }

        private void ValidateInput(RollType roll)
        {
            Assert.ValidEnum(roll);
            Assert.IsTrue(RollTypeToDieMap.ContainsKey(roll));
        }
    }
}
