using GunslingerSim.Common;
using GunslingerSim.Common.Enums;
using GunslingerSim.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunslingerSim.Tests
{
    public class MockRng : Rng
    {
        private IList<int> sequence;
        private int currentPlaceInSequence;

        public MockRng(IList<int> sequence)
        {
            Assert.IsNotNull(sequence);
            Assert.HasNoNullEntries(sequence);
            Assert.IsNotEmpty(sequence);

            this.sequence = sequence;
            currentPlaceInSequence = 0;
        }

        public override int Roll(RollType roll)
        {
            int nextEntryInSequence = GetNextEntryInSequence();
            return MaxValue(roll, nextEntryInSequence );
        }

        private int GetNextEntryInSequence()
        {
            int nextEntry = sequence[currentPlaceInSequence];

            currentPlaceInSequence = (currentPlaceInSequence == (sequence.Count - 1))
                ? 0
                : (currentPlaceInSequence + 1);

            return nextEntry;
        }

        private int MaxValue(RollType roll, int val)
        {
            int maxRollValue = RollTypeToDieMap[roll];

            return (val > maxRollValue)
                ? maxRollValue
                : val;
        }
    }
}
