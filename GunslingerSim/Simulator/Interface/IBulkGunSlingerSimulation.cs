using GunslingerSim.Common.Util;
using GunslingerSim.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GunslingerSim.Simulator
{
    public interface IBulkGunSlingerSimulation
    {
        SimulationSummary BulkSimulate(IPlayer player, IEnemy enemy);
    }
}
