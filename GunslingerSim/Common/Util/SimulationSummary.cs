using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Common.Util
{
    public class SimulationSummary
    {
        public ulong DamageDone { get; set; }
        public ulong Shots { get; set; }
        public ulong Hits { get; set; }
        public ulong Crits { get; set; }
        public ulong Cost { get; set; }
        public ulong NumberOfBrokenGuns { get; set; }
        public ulong ShotsLostToMisfire { get; set; }
    }
}
