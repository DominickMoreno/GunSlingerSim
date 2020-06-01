using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Util
{
    public class GunRange
    {
        public int Range { get; private set; }
        public int LongRange { get; private set; }

        public GunRange(int range, int longRange)
        {
            Assert.IsTrue(range > 0);
            Assert.IsTrue(longRange > 0);
            Assert.IsTrue(longRange >= range);

            Range = range;
            LongRange = longRange;
        }
    }
}
